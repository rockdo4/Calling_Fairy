using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSetting : MonoBehaviour
{
    [SerializeField]
    private Transform ShopItems;
    [SerializeField] 
    private GameObject ShopItemsPrefab;
    [SerializeField]
    private Button ConfirmButton;
    [SerializeField]
    private UI Confirm;
    [SerializeField]
    private Text text;

    private void Awake()
    {
        var data = DataTableMgr.GetTable<ShopTable>();
        foreach (var item in data.dic.Values)
        {
            var obj = Instantiate(ShopItemsPrefab, ShopItems);
            var sprite = Resources.Load<Sprite>($"ShopIcon/{item.Icon}");
            var script = obj.GetComponent<ShopUIButton>();
            script.SetData(GameManager.stringTable[item.ItemName].Value, sprite, item.Price, item.Value);
            script.SetButton(ConfirmButton, Confirm);
        }        
    }

    private void OnEnable()
    {
        text.text = $"{Player.Instance.SummonStone}";
    }
}
