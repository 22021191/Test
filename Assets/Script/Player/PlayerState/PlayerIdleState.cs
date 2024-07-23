using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player._rb2d.gravityScale = player.reverse * player._rb2d.gravityScale;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (exitState)
        {
            return;
        }

        player._horizontalSpeed = Mathf.MoveTowards(player._horizontalSpeed, 0, data.deceleration * Time.deltaTime);
        player.SetVelocityX(player._horizontalSpeed);
        if (inputX != 0)
        {
            
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
