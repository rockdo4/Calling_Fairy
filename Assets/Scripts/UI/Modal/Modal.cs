using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Modal : MonoBehaviour
{
    public UnityAction OnOpenModal;
    public UnityAction OnCloseModal;

    private Button button;
    private Transform popupTrsf;
    private int originOrder;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CloseModal);
    }

    public void OpenModal(Transform transform)
    {
        gameObject.SetActive(true);
        popupTrsf = transform;
        originOrder = transform.GetSiblingIndex();
        transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

        if (OnOpenModal != null)
        {
            OnOpenModal.Invoke();
        }
    }

    public void CloseModal()
    {
        popupTrsf.SetSiblingIndex(originOrder);
        gameObject.SetActive(false);

        if (OnCloseModal != null)
        {
            OnCloseModal.Invoke();
        }
    }

}
