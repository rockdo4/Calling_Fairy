using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyStamina : MonoBehaviour
{
    [SerializeField]
    private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (Player.Instance.SummonStone < 50)
        {
            Debug.Log("��ȯ���� �����մϴ�.");
            return;
        }
        Player.Instance.Stamina += Player.Instance.MaxStamina;
    }
}
