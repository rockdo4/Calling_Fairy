using System.Collections;
using UnityEngine;

public class CreatureAttackState : CreatureBase
{ 
    private IDamaged.DamageType damageType;
    private float damage;
    public CreatureAttackState(CreatureController cc) : base(cc)
    {
        if(creature.basicStatus.physicalAttack != 0)
        {
            damage = creature.basicStatus.physicalAttack;
            damageType = IDamaged.DamageType.Physical;
        }
        else
        {
            damage = creature.basicStatus.magicalAttack;
            damageType = IDamaged.DamageType.Magical;
        }        
    }
    public override void OnEnter()
    {
        base.OnEnter();
        var targetable = creature.target;
        if (targetable == null)
            return;
        targetable.OnDamaged(damage, damageType);
        
    }
    public override void OnExit()
    {
        base.OnExit();
        creature.target = null;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        creature.StartAttackTimer();
        //if(timer > creature.basicStatus.AttackSpeed)
        {
            creatureController.ChangeState(StateController.State.Idle);
            return;
        }
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

}
