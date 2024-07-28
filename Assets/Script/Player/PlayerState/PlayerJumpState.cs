using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private bool _OnGround;
    public bool doubleJump;
    protected bool isAbilityDone;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();
        _OnGround = player.GroundCheck();
    }
    public override void Enter()
    {
        base.Enter();
        player.input.jumpInput = false;
        player.SetVelocityY(data.jumpForce*player.reverse);
        isAbilityDone = true;
        AudioManager.Instance.PlaySfx("Jump");
        
    }
    public override void Exit()
    {
        base.Exit();
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
    }

}
