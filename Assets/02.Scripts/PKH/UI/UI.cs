using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UI : MonoBehaviour, IUI
{
    public UI parentWindow = null;
    public UI childrenWindow = null;
    public event Action OnActive = null;
    public event Action OnNonActive = null;

    public virtual void ActiveUI()
    {
        while (UIManager.Instance.currentUI != null && UIManager.Instance.currentUI != parentWindow)
        {
            UIManager.Instance.currentUI.NonActiveUI();
        }
        if (OnActive != null)
        {
            OnActive();
        }
        gameObject.SetActive(true);
        UIManager.Instance.currentUI = this;
    }

    public virtual void NonActiveUI()
    {
        if (OnNonActive != null)
        {
            OnNonActive();
        }
        gameObject.SetActive(false);
        if (parentWindow != null)
        {
            UIManager.Instance.currentUI = parentWindow;
            return;
        }
        UIManager.Instance.currentUI = null;
    }
}
