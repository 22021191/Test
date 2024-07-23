using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Data data;
    protected float startTime;
    protected bool exitState;
    protected bool isAnimationFinished;
    protected string animName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, Data playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.data = playerData;
        this.animName = animBoolName;
    }

    public virtual void Enter()
    {
        exitState = false;
        DoChecks();
        player.ChangeAnim(animName);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        exitState = true;
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
