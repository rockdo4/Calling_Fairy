using System;
using UnityEngine;

public class UI : MonoBehaviour, IUI
{
    public event Action OnActive = null;
    public event Action OnNonActive = null;

    public virtual void ActiveUI()
    {
        if (OnActive != null)
        {
            OnActive();
        }
        UIManager.Instance.PushUI(this);
    }
     
    public virtual void NonActiveUI()
    { 
        if (OnNonActive != null)
        {
            OnNonActive();
        }
        UIManager.Instance.PopUI();
    }
}
