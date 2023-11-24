using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, IUI
{
    public Transform contentTrsf;

    public void ActiveUI()
    {
        if (UIManager.Instance.currentUI != null)
        {
            UIManager.Instance.currentUI.NonActiveUI();
        }
        gameObject.SetActive(true);
        UIManager.Instance.currentUI = this;
    }

    public void NonActiveUI()
    {
        gameObject.SetActive(false);
        UIManager.Instance.currentUI = null;
    }
}
