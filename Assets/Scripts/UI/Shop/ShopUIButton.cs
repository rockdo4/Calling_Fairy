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

    public void SetData(string infoText, Sprite sprite, int itemPrice)
    {
        text.text = infoText;
        itemIcon.sprite = sprite;
        price.text = itemPrice.ToString();
    }
}
