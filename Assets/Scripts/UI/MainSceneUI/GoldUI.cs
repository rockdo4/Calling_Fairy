using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    public Text goldText;

    private void Awake()
    {
        UIManager.Instance.OnMainSceneUpdateUI += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        goldText.text = $"{Player.Instance.Gold}";
    }
}
