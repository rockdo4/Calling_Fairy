using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Info.Asset", menuName = "Stage/StageInfo")]
public class SOStageInfo : ScriptableObject
{
    Tuple<int, GameObject>[] StageInfo;
}
