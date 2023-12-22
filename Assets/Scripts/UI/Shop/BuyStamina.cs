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
            Debug.Log("소환석이 부족합니다.");
            return;
        }
        Player.Instance.Stamina += Player.Instance.MaxStamina;
    }
}
