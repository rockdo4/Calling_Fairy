using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopPurchaseStone : MonoBehaviour
{
    [HideInInspector]
    public int StoneAmount;
    [HideInInspector]
    public int Price;
    private Button PurchaseButton;
    private Button button;
    private GameObject go;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ButtonCkilcked());
    }

    public void ButtonCkilcked()
    {
        PurchaseButton.onClick.RemoveAllListeners();
        PurchaseButton.onClick.AddListener(() => Purchase());
        PurchaseButton.onClick.AddListener(() => go.SetActive(false));
    }

    public void SetData(int StoneAmount, int Price)
    {
        this.StoneAmount = StoneAmount;
        this.Price = Price;
    }

    public void SetButton(Button btn, GameObject go)
    {
        PurchaseButton = btn;
        this.go = go;
        button.onClick.AddListener(() => go.SetActive(true));
    }

    public void Purchase()
    {
        if (!CheckPurchase())
        {
            Debug.Log("돈이 부족합니다.");
            return;
        }
        Player.Instance.GetSummonStone(StoneAmount);
    }

    private bool CheckPurchase()
    {
        if (!Player.Instance.UseGold(Price))
        {
            return false;
        }
        return true;
    }
}
