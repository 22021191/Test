using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    protected int inputX;
    protected int inputY;
    protected bool onGround;
    protected bool jumpInput;
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        onGround = player.GroundCheck();
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpState.doubleJump = true;
        player._rb2d.gravityScale = 1;
        player._rb2d.drag = data._groundLinearDrag;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        inputX = player.input.inputX;
        inputY = player.input.inputY;
        jumpInput=player.input.jumpInput;
        if(jumpInput )
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }
        else if (!onGround)
        {
            player.airState.StartCoyoteTime();
            stateMachine.ChangeState(player.airState);
        }
    }
}
