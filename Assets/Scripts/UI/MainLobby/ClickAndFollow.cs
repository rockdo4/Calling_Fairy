using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndFollow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isHolding = false;
    private bool isFalling = false;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float fallSpeed = 8f;
    [SerializeField] private float YFallBound;

    public event Action OnStartHolding;
    public event Action OnEndHolding;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (isFalling)
        {
            gameObject.transform.position += Vector3.down * (fallSpeed * Time.deltaTime);
            if (YFallBound > gameObject.transform.position.y)
            {
                isFalling = false;
            }
        }

        if(!isHolding)
            return;

        var pos = cam.ScreenToWorldPoint(Input.mousePosition + offset);
        pos.z = 0;

        gameObject.transform.position = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject != gameObject)
            return;

        isHolding = true;
        OnStartHolding?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndHolding?.Invoke();
        isHolding = false;
        isFalling = true;
    }

    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }
}