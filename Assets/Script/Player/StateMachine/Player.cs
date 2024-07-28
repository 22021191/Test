using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerData")]
    [SerializeField] private Data data;
    [SerializeField] private GameObject door;

    #region Value
    [Header("Component")]
    public Rigidbody2D _rb2d;
    public BoxCollider2D collider;
    public PlayerInput input;
    
    [Header("Check Value")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform boxCheck;


    [Header("Variable")]
    private Vector2 workSpace;
    private Vector2 curVerlocity;
    [SerializeField] private string changeAnimName;
    public int facingRight;
    public int reverse;
    bool canReverse = true;
    public float _horizontalSpeed;
    
    public Animator anim { get; private set; }
    
    #endregion

    #region State Value
    [Header("StateMachine")]
    public PlayerStateMachine stateMachine;

    [Header("Player State")]
    public PlayerIdleState idleState;
    public PlayerAirState airState;
    public PlayerJumpState jumpState;
    public PlayerMoveState moveState;
    public PlayerWallJumpState wallJumpState;
    public PlayerWallSliceState wallSliceState;
    public PlayerDashState dashState;
    
    #endregion

    private void Awake()
    {
        //Khoi tao Component
        anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        input= GetComponent<PlayerInput>();
        //Khoi tao StateMachine
        stateMachine = new PlayerStateMachine();
        GameManager.Instance.player=this;
        GameManager.Instance.door = this.door;

        //Khoi tao Player State
        idleState = new PlayerIdleState(this, stateMachine, data, "Idle");
        airState = new PlayerAirState(this, stateMachine, data, "Jump");
        jumpState = new PlayerJumpState(this, stateMachine, data, "Jump");
        moveState = new PlayerMoveState(this, stateMachine, data, "Move");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, data, "Jump");
        wallSliceState = new PlayerWallSliceState(this, stateMachine, data, "Jump");
        dashState = new PlayerDashState(this, stateMachine, data, "Jump");
    }

    void Start()
    {
        stateMachine.Initialize(idleState);
    }


    void Update()
    {
        curVerlocity = _rb2d.velocity;
        stateMachine.CurrentState.LogicUpdate();
        

    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }
    #region Set Function
    public void SetVelocityY(float force)
    {
        workSpace.Set(curVerlocity.x, force);
        _rb2d.velocity = workSpace;
        curVerlocity = _rb2d.velocity;
    }

    public void SetVelocityX(float speed)
    {
        workSpace.Set(speed, curVerlocity.y);
        _rb2d.velocity = workSpace;
        curVerlocity = workSpace;
    }

    public void SetVelocityZero()
    {
        _rb2d.velocity = Vector2.zero;
        curVerlocity = Vector2.zero;

    }
    public void NextRoom()
    {
        
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workSpace = direction * velocity;
        _rb2d.velocity = workSpace;
        curVerlocity = workSpace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        _rb2d.velocity = workSpace;
        curVerlocity = workSpace;
    }
    #endregion

    #region Check Functions
    public void CheckFlip(float inputX)
    {
        if (inputX != 0 && inputX != facingRight)
        {
            Flip();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if ((data.trapMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Trap detected.");
            GameManager.Instance.RePlay();
        }
        else if ((data.reverseMask.value & (1 << collision.gameObject.layer)) != 0&&canReverse)
        {
            StartCoroutine(DelayReverse());
            Reverse();
            Debug.Log("ok");
        }
        else if ((data.keyMask.value & (1 << collision.gameObject.layer)) != 0 )
        {
            GameManager.Instance.key = true;
            Destroy(collision.gameObject);
            AudioManager.Instance.PlaySfx("Coin");
            GameManager.Instance.door.GetComponent<Collider2D>().isTrigger = true;
            GameManager.Instance.door.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.doorOpen;
        }
        else if ((data.dashMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            stateMachine.ChangeState(dashState);
        }
    }
    IEnumerator DelayReverse()
    {
        canReverse = false;
        yield return new WaitForSeconds(1f);
        canReverse = true;

    }
    public void Reverse()
    {
        reverse *= -1;
        transform.Rotate(180,0,0);
        
    }

    public bool GroundCheck()
    {
        return Physics2D.Raycast(groundCheck.position + data._groundRaycastOffset, Vector2.down*reverse, data.groundRadius, data.groundMask) ||
                    Physics2D.Raycast(groundCheck.position - data._groundRaycastOffset, Vector2.down*reverse, data.groundRadius, data.groundMask);

    }
    public bool WallFrontCheck()
    {
        return Physics2D.Raycast(boxCheck.position, Vector2.right * facingRight, data.boxDistance, data.groundMask);
    }
    public bool WallCheckBack()
    {

        return Physics2D.Raycast(boxCheck.position, Vector2.left * facingRight, data.boxDistance, data.groundMask);
    }
    public bool BoxCheck()
    {
        return Physics2D.Raycast(boxCheck.position, Vector2.right * facingRight, data.boxDistance, data.boxMask);
    }


    #endregion

    #region Other Functions

    public void EnableControls()
    {
        input.enabled = true;
    }
    public void DisableControls()
    {
        input.enabled = false;
    }

    private void Flip()
    {
        facingRight *= -1;
        transform.Rotate(0, 180, 0);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Ground Check
        Gizmos.DrawLine(groundCheck.position + data._groundRaycastOffset, groundCheck.position + data._groundRaycastOffset + Vector3.down * data.groundRadius*reverse);
        Gizmos.DrawLine(groundCheck.position - data._groundRaycastOffset, groundCheck.position - data._groundRaycastOffset + Vector3.down * data.groundRadius*reverse);
        //Wall Check
        Gizmos.color = Color.red;
        Gizmos.DrawLine(boxCheck.position, boxCheck.position + Vector3.right * data.boxDistance);
        
    }

    private void AnimationTrigger()
    {
        stateMachine.CurrentState.AnimationTrigger();

    }

    private void AnimtionFinishTrigger()
    {
        stateMachine.CurrentState.AnimationFinishTrigger();

    }

    public void ChangeAnim(string AnimName)
    {
        if (changeAnimName == AnimName) return;

        anim.ResetTrigger(AnimName);
        changeAnimName = AnimName;
        anim.SetTrigger(AnimName);
    }

   
    public void ResetData(Vector3 pos)
    {
        transform.position = pos;
        stateMachine.ChangeState(idleState);
        input.moveInput.x = 0;
        input.inputX = 0;
        reverse = 1;
        transform.rotation = Quaternion.EulerAngles(0, 0, 0);
        facingRight = 1;
        SetVelocityZero();
        DisableControls();
    }
    
    #endregion
}
