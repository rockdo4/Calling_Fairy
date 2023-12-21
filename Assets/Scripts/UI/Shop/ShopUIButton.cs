using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIButton : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField] 
    private Image itemIcon;
    [SerializeField]
    private Text price;
    [SerializeField]
    private ShopPurchaseStone shopPurchaseStone;


    public void SetData(string infoText, Sprite sprite, int itemPrice, int amount)
    {
        text.text = infoText;
        itemIcon.sprite = sprite;
        price.text = itemPrice.ToString();
        shopPurchaseStone.SetData(amount, itemPrice);
    }

    public void SetButton(Button btn, UI uI)
    {
        shopPurchaseStone.SetButton(btn, uI);
    }
}
