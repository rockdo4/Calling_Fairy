using UnityEngine;

public abstract class StateController
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        Dead,
        Count,
    }

    protected BaseState[] States;
    public BaseState curState;

    public abstract void SetController();
    public void ChangeState(State state)
    {
        if (States[(int)state] == null)
        {
           Debug.Log("잘못된 상태 접근입니다. 확인 부탁드립니다.");
            return;
        }
        curState.OnExit();
        curState = States[(int)state];
        curState.OnEnter();
    }
}

public class CreatureController : StateController
{
    public Creature creature;
    public CreatureController(Creature creature)
    {
        this.creature = creature;
        SetController();
    }
    public override void SetController()
    {
        States = new BaseState[(int)State.Count];
        States[(int)State.Idle] = new CreatureIdleState(this);
        States[(int)State.Move] = new CreatureMoveState(this);
        States[(int)State.Attack] = new CreatureAttackState(this);
        States[(int)State.Dead] = new CreatureDeadState(this);        

        curState = States[(int)State.Idle];
        ChangeState(State.Move);
    }
}