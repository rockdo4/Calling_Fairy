using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityRow : MonoBehaviour
{
    public Text level;
    public TextMeshProUGUI abilityTooltip;
    public GameObject filter;

    public void SetInfo(PlayerData playerData, bool activeFilter)
    {
        level.text = playerData.PlayerLevel.ToString();

        var abilityTable = DataTableMgr.GetTable<PlayerAbilityTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        if (abilityTable.dic.TryGetValue(playerData.PlayerAbility, out var abilityData))
        {
            if (stringTable.dic.TryGetValue(abilityData.AbilityTooltip, out var tooltip))
            {
                abilityTooltip.text = tooltip.Value;
            }
        }

        filter.SetActive(activeFilter);
    }
}
