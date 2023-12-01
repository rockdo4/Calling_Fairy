using UnityEngine;

public abstract class BuffBase : MonoBehaviour
{
    public enum BuffType
    {
        Revival,
    }
    public static BuffBase MakeBuff(BuffType buffType)
    {
        BuffBase rtn;
        rtn = buffType switch
        {
            BuffType.Revival => new Revival(),
            _ => null,
        };
        return rtn;
    }
    public abstract void SetBuff(BuffInfo buffInfo);
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();

}
public struct BuffInfo
{
    public bool HasInfo;
    public BuffBase.BuffType buffType;
}
