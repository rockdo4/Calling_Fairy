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
        while (UIManager.Instance.CurrentUI != null && UIManager.Instance.CurrentUI != parentWindow)
        {
            UIManager.Instance.CurrentUI.NonActiveUI();
        }
        if (OnActive != null)
        {
            OnActive();
        }

        if (UIManager.Instance.CurrentUI != null)
        {
            parentWindow = UIManager.Instance.CurrentUI;
            UIManager.Instance.CurrentUI.childrenWindow = this;
        }
        
        UIManager.Instance.CurrentUI = this;
        gameObject.SetActive(true);
    }
     
    public virtual void NonActiveUI()
    {
        if(childrenWindow != null)
        {
            childrenWindow.NonActiveUI();
        }
        if (OnNonActive != null)
        {
            OnNonActive();
        }
        gameObject.SetActive(false);
        if (parentWindow != null)
        {
            UIManager.Instance.CurrentUI = parentWindow;
            return;
        }
        UIManager.Instance.CurrentUI = null;
    }
}
