public class Shield : BuffBase
{
    public float leftshield;
    public override void OnEnter()
    {
        //Debug.Log("Shield OnEnter");
        base.OnEnter();
        buffInfo.isDebuff = false;
        leftshield = buffInfo.value;
        creature.shields.AddFirst(this);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(leftshield <= 0)
        {
            creature.RemoveBuff(this);
        }
    }
    public override void OnExit()
    {        
        //Debug.Log("Shield OnExit");
        creature.shields.Remove(this);
        creature.LerpHpUI();
    }
    public float DamagedShield(float damage)
    {
        if (leftshield > damage)
        {
            leftshield -= damage;
            //Debug.Log("leftshield : " + leftshield);
            return 0;
        }
        else
        {
            damage -= leftshield;
            //Debug.Log("Shield is broken!");
            leftshield = 0;
            return damage;
        }
    }
}
