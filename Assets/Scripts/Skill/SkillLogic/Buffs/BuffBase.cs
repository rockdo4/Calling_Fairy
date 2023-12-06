public abstract class BuffBase
{
    public enum BuffType
    {
        Revival,
        AtkDmgBuff,
        AtkSpdBuff,
        CritRateBuff,
        Heal,
        MDefBuff,
        PDefBuff,
    }

    protected Creature creature; 
    public static BuffBase MakeBuff(BuffType buffType)
    {
        BuffBase rtn;
        rtn = buffType switch
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
        return rtn;
    }
    public virtual void SetBuff(BuffInfo buffInfo, Creature creature)
    {
        this.creature = creature;
    }
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}
public struct BuffInfo
{
    public BuffBase.BuffType buffType;
    public float duration;
    public float value;
    public bool isPercent;
}
