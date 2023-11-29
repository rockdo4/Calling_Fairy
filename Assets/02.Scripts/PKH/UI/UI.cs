using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UI : MonoBehaviour, IUI
{
    public UI parentWindow = null;
    public UI childrenWindow = null;
    public event Action OnAction = null;

    public virtual void ActiveUI()
    {
        while (UIManager.Instance.currentUI != null && UIManager.Instance.currentUI != parentWindow)
        {
            UIManager.Instance.currentUI.NonActiveUI();
        }
        if (OnAction != null)
        {
            OnAction();
        }
        gameObject.SetActive(true);
        UIManager.Instance.currentUI = this;
    }

    public virtual void NonActiveUI()
    {
        gameObject.SetActive(false);
        if (parentWindow != null)
        {
            UIManager.Instance.currentUI = parentWindow;
            return;
        }
        UIManager.Instance.currentUI = null;
    }
}
