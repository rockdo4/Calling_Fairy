using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashUIAwake : MonoBehaviour
{
    public Text summonStoneText;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(UpdateUI);
    }

    public void UpdateUI()
    {
        summonStoneText.text = $"{Player.Instance.SummonStone}";
    }
}
