using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Text staminaText;

    private void Awake()
    {
        UIManager.Instance.OnMainSceneUpdateUI += UpdateUI;
    }

    public void UpdateUI()
    {
        var table = DataTableMgr.GetTable<PlayerTable>();
        staminaText.text = $"{Player.Instance.Stamina}/{table.dic[Player.Instance.Level].PlayerMaxStamina}";
    }
}
