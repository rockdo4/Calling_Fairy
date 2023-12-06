using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour, IUIElement
{
    public void Init(Card card)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            if (childTransform.TryGetComponent(out IUIElement component))
            {
                component.Init(card);
            }
        }
    }
}
