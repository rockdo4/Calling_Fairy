using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashUI : MonoBehaviour
{
    public Text summonStoneText;

    private void Awake()
    {
        UIManager.Instance.OnMainSceneUpdateUI += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        summonStoneText.text = $"{Player.Instance.SummonStone}";
    }
}
