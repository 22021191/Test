using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private float speed;
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        exitState = false;
        DoChecks();
        startTime = Time.time;
        if (player.BoxCheck())
        {
            speed = data.pushSpeed;
            player.ChangeAnim("Push");
        }
        else
        {
            speed = data.maxSpeed;
            player.ChangeAnim(animName);

        }
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

        player._horizontalSpeed = Mathf.MoveTowards(player._horizontalSpeed, data.maxSpeed * inputX, data.acceleration * Time.deltaTime);
        player.SetVelocityX(player._horizontalSpeed);
        player.CheckFlip(inputX);

        if (inputX == 0)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player._rb2d.drag = data._groundLinearDrag;

        if (player.BoxCheck())
        {
            speed = data.pushSpeed;
            player.ChangeAnim("Push");
        }
        else
        {
            speed = data.maxSpeed;
            player.ChangeAnim(animName);

        }
    }
}
