using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;

    //Checks
    private bool isGrounded;
    private bool coyoteTime;
    private bool isJumping;
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.GroundCheck();
    }
    public override void Exit()
    {
        base.Exit();

        player._rb2d.drag = 1;

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player._rb2d.drag = data.airLinearDrag;

        if (player._rb2d.velocity.y < 0)
        {
            player._rb2d.gravityScale = data.fallMultiplier*player.reverse;
        }
        else
        {
            player._rb2d.gravityScale = data.jumpMultiplier*player.reverse;
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + data.coyoteTime)
        {
            coyoteTime = false;

        }
    }


    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckCoyoteTime();

        xInput = player.input.inputX;
        jumpInput = player.input.jumpInput;
        if (isGrounded )
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        else if (jumpInput && coyoteTime)
        {
            Debug.Log("nhay");
            stateMachine.ChangeState(player.jumpState);
            return;
        }
        else if (jumpInput && player.jumpState.doubleJump)
        {
            player.jumpState.doubleJump = false;
            Debug.Log("double Jump");
            stateMachine.ChangeState(player.jumpState);

        }
        else
        {
            player.CheckFlip(xInput);
            player.SetVelocityX(data.maxSpeed * xInput);

        }
    }
}
