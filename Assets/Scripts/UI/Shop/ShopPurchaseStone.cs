using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPurchaseStone : MonoBehaviour
{
    [HideInInspector]
    public int StoneAmount;
    [HideInInspector]
    public int Price;
    [SerializeField]
    private Button PurchaseButton;

    private void Awake()
    {

        PurchaseButton.onClick.AddListener(() => Purchase());
    }

    public void SetData(int StoneAmount, int Price)
    {
        this.StoneAmount = StoneAmount;
        this.Price = Price;
    }

    public void Purchase()
    {
        if (CheckPurchase())
        {
            Debug.Log("���� �����մϴ�.");
            return;
        }
        Player.Instance.GetSummonStone(StoneAmount);
    }

    private bool CheckPurchase()
    {
        return true;
    }
}
