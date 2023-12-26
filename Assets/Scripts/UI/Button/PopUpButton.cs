using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PopUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool isHolding = false;
    private bool isInside = false;
    private bool isActived = false;

    [SerializeField]
    private float popUpTime = 0.5f;
    private float popUpTimer = 0f;

    public UnityEvent keepPressed;
    public UnityEvent released;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        released?.Invoke();
        isHolding = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    private void Update()
    {
        if(isHolding && isInside)
        {
            popUpTimer += Time.deltaTime;
        }
        else
        {
            popUpTimer = 0f;
            isActived = false;
        }

        if((popUpTime < popUpTimer) && !isActived)
        {
            isActived = true;
            keepPressed?.Invoke();
        }
    }
}
