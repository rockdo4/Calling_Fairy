using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipConfirm : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI skipNumText;

    public void SetString()
    {
        var stamina = Skip.skipNum * DataTableMgr.GetTable<StageTable>().dic[ GameManager.Instance.StageId].useStamina;
        skipNumText.text = $"��ŵ�� {Skip.skipNum} ,���¹̳� {stamina}�� �����\n������ ��ŵ�Ͻðڽ��ϱ�?";
    }
}
