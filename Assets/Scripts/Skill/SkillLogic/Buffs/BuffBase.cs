using UnityEngine;

public abstract class BuffBase
{

    protected Creature creature;
    protected BuffInfo buffInfo;
    protected float timer;
    public static BuffBase MakeBuff(in BuffInfo buffInfo)
    {
        BuffBase rtn;
        rtn = buffInfo.buffType switch
        {
            BuffType.Revival => new Revival(),
            BuffType.AtkDmgBuff => new AtkDmgBuff(),
            BuffType.AtkSpdBuff => new AtkSpeedBuff(),
            BuffType.CritRateBuff => new CriticalRateBuff(),
            BuffType.Heal => new Heal(),
            BuffType.MDefBuff => new MDefBuff(),
            BuffType.PDefBuff => new PDefBuff(),
            _ => null,
        };
        rtn.buffInfo = buffInfo;
        return rtn;
    }

    public void SetCreature(Creature creature)
    {
        this.creature = creature;
    }
    public virtual void OnEnter()
    {
        timer = buffInfo.duration + Time.time;
    }
    public abstract void OnExit();
    public virtual void OnUpdate()
    {
        if(Time.time > timer)
        {
            creature.RemoveBuff(this);
        }
    }
}
public struct BuffInfo
{
    public bool isDebuff;
    public BuffType buffType;
    public float duration;
    public float value;
    public bool isPercent;
    public string buffName;
    public int buffPriority;
}
