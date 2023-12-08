using System.Collections.Generic;
using UnityEngine;

public class StageTableInfo : MonoBehaviour
{
    public Dictionary<int, StageData> tableInfo = new();
    private void Awake()
    {
        tableInfo = DataTableMgr.GetTable<StageTable>().dic;
    }
}
