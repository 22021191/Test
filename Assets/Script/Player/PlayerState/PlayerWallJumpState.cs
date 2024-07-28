using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private int direction;
    private bool isAbilityDone;
    private bool _OnGround;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

   

    public override void DoChecks()
    {
        base.DoChecks();
        _OnGround = player.GroundCheck();
    }

    public override void Enter()
    {
        player.SetVelocity(data.wallJumpVelocity, data.wallAngle, direction);
        player.CheckFlip(direction);
        isAbilityDone = false;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
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
        }else if (Time.time >= startTime + data.wallJumpTime)
        {
            isAbilityDone = true;
        }

    }

    public void WallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            direction = -player.facingRight;
        }
        else
        {
            direction = player.facingRight;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
