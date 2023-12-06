using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public Tab selectedTab;
    public List<GameObject> objectsToSwap;
    
    public void Subscribe(Tab button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<Tab>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(Tab button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(Tab button)
    {
        ResetTabs();
    }

    public void OnTabSelected(Tab button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (Tab button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
                continue;
            button.background.sprite = tabIdle;
        }
    }
}
