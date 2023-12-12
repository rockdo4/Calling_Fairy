using UnityEngine;

public class CreatureMoveState : CreatureBase
{
    protected StageManager stageManager;
    public CreatureMoveState(CreatureController sc) : base(sc)
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        creature.Animator.SetBool("IsMoving", true);
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if(creature.isKnockbacking || stageManager.isReordering)         
            return;
        
        var moveAmount = new Vector2(creature.Status.basicMoveSpeed, creature.Rigidbody.velocity.y);
        moveAmount.x *= creature.Status.moveSpeed;
        moveAmount.x /= 30f;
        creature.Rigidbody.velocity = moveAmount;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();  
        if(CheckRange())
        {
            if (creature.isAttacking)
            {
                creatureController.ChangeState(StateController.State.Idle);
                return;
            }
            else
            {
                creatureController.ChangeState(StateController.State.Attack);
                return;
            }
        }
    }
}
