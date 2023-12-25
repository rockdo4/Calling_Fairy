using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinuousButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Button button = null;

    [SerializeField]
    [Tooltip("���� ���������δ������� �˻��ϴ� �ð�")]
    private float delay = 0.1f;
    [SerializeField]
    [Tooltip("���������� ������ �ð� ����")]
    private float repeatRate = 0.1f;
    
    private bool isTouchInit = false;
    private bool isHolding = false;
    private bool isInside = false;
    private bool isDelayTime = false;

    private int TestCount = 0;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
        TestCount = 0;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        TestCount = 0;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouchInit = true;
    }

    private void Update()
    {
        if (isTouchInit && isInside) 
        {
            StartCoroutine(DelayCounter());
            return;
        }
        if(isHolding && isInside && !isDelayTime)
        {
            StartCoroutine(RepeatRateCounter());
            button?.onClick.Invoke();
        }
    }

    IEnumerator RepeatRateCounter()
    {
        isDelayTime = true;
        yield return new WaitForSeconds(repeatRate);
        isDelayTime = false;
    }
    IEnumerator DelayCounter()
    {
        isTouchInit = false;
        yield return new WaitForSeconds(delay);
        isHolding = true;
    }

    public void Test()
    {
        Debug.Log($"Test{TestCount++}");
    }

}
