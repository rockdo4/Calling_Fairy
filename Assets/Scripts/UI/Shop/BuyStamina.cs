using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyStamina : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text text;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnEnable()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = $"{Player.Instance.SummonStone}";
    }

    private void OnClick()
    {
        if (Player.Instance.SummonStone < 50)
        {
            Debug.Log("��ȯ���� �����մϴ�.");
            return;
        }
        Player.Instance.Stamina += Player.Instance.MaxStamina;
        Player.Instance.UseSummonStone(50);
        UpdateText();
    }
}
