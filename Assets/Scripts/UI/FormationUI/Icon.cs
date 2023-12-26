using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected IconType iconType;
    protected int id;
    protected PopUpTooltip popUpTooltip;
    //protected PopUpButton popUpButton;

    private void Awake()
    {
        var canvas = GameObject.FindWithTag(Tags.Canvas);
        popUpTooltip = canvas.GetComponentInChildren<PopUpTooltip>(true);
        if(TryGetComponent<PopUpButton>(out var popUpButton))
        {
            popUpButton.keepPressed.AddListener(SetPopUp);
            popUpButton.keepPressed.AddListener(()=> popUpTooltip.gameObject.SetActive(true));
            popUpButton.released.AddListener(() => popUpTooltip.gameObject.SetActive(false));
        }
    }

    public void SetIcon(int id, Sprite icon)
    {
        iconImage.sprite = icon;
        this.id = id;
    }   

    public void SetPopUp()
    {        
        popUpTooltip.SetData(iconType, id, transform.position.x > (1080 / 2));
    }
}
