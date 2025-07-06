using UnityEngine;

public class CreatureMoveState : CreatureBase
{
    private const float MoveFactor = 30f;

    protected StageManager stageManager;
    public CreatureMoveState(CreatureController sc) : base(sc)
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        creature.animator.SetBool(Triggers.IsMoving, true);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if(creature.isKnockbacking || stageManager.isReordering || creature.isSkillUsing)
            return;
        
        var moveAmount = new Vector2(creature.Status.basicMoveSpeed, creature.Rigidbody.velocity.y);
        moveAmount.x *= creature.Status.moveSpeed;
        moveAmount.x /= MoveFactor;
        creature.Rigidbody.velocity = moveAmount;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();  
        if(CheckRange())
        {
            creatureController.ChangeState(creature.isAttacking
                ? StateController.State.Idle
                : StateController.State.Attack);
        }
    }
}
