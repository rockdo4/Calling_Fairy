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
        skipNumText.text = $"스킵권 {Skip.skipNum} ,스태미너 {stamina}을 사용해\n던전을 스킵하시겠습니까?";
    }
}
