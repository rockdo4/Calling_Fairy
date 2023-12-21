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
    private UI ui;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ButtonCkilcked());
    }
    
    public void ButtonCkilcked()
    {
        PurchaseButton.onClick.RemoveAllListeners();
        PurchaseButton.onClick.AddListener(() => Purchase());
        PurchaseButton.onClick.AddListener(() => ui.NonActiveUI());
    }

    public void SetData(int StoneAmount, int Price)
    {
        this.StoneAmount = StoneAmount;
        this.Price = Price;
    }

    public void SetButton(Button btn, UI uI)
    {        
        PurchaseButton = btn;
        ui = uI;
        button.onClick.AddListener(() => uI.ActiveUI());
    }

    public void Purchase()
    {
        if (!CheckPurchase())
        {
            Debug.Log("µ∑¿Ã ∫Œ¡∑«’¥œ¥Ÿ.");
            return;
        }
        Player.Instance.GetSummonStone(StoneAmount);
    }

    private bool CheckPurchase()
    {
        Debug.Log("∞·¿Áµ ");
        return true;
    }
}
