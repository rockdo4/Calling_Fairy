using UnityEngine;

[CreateAssetMenu(fileName = "Buff Info.Asset", menuName = "Status/Buff Info")]
public class SOBuffInfo : ScriptableObject
{
    public int ID;
    public BuffType buffType;
    public IngameStatus.StatusType statusType;
    public IngameStatus.StatusType targetStatusType;
    public GetTarget.TargettingType targettingType;
}
