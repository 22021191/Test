using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float lastDashTime;
    protected bool isAbilityDone;
    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private bool _OnGround;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.gameObject.layer = LayerMask.NameToLayer("Dash");

        isAbilityDone = false;
        dashDirection = Vector2.right * player.facingRight;

        dashDirectionInput = player.input.moveInput;
        if (dashDirectionInput != Vector2.zero)
        {
            dashDirection = dashDirectionInput.normalized;
/*            dashDirection.Normalize();
*/        }
        float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
        Time.timeScale = 1f;
        startTime = Time.time;
        player.CheckFlip(Mathf.RoundToInt(dashDirection.x));
        player.SetVelocity(data.dashSpeed, dashDirection);
    }
    public override void Exit()
    {
        base.Exit();

        player.gameObject.layer = LayerMask.NameToLayer("Player");
        isAbilityDone = false;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAbilityDone)
        {
            if (_OnGround && player._rb2d.velocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                Debug.Log("Ok");
                stateMachine.ChangeState(player.airState);
            }
        }
        else if (!exitState)
        {
            player.SetVelocity(data.dashSpeed, dashDirection);

            if (Time.time >= startTime + data.dashLength)
            {
                player._rb2d.drag = 0f;
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
            

        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _OnGround = player.GroundCheck();
    }
}
