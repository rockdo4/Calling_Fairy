using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage.Asset", menuName = "Stage/NewStageInfo")]
public class SOStageInfo : ScriptableObject
{
    public List<ScriptableObject> EnemyInfo;
    public int[] waveInfo;
}
