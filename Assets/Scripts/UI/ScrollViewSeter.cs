using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewSeter : MonoBehaviour
{
    public Transform contentTrsf;

    public void SetContents(List<GameObject> contents)
    {
        Clear();

        foreach (GameObject content in contents)
        {
            content.transform.SetParent(contentTrsf);
        }
    }

    public void Clear()
    {
        foreach (Transform child in contentTrsf)
        {
            Destroy(child.gameObject);
        }
    }
}
