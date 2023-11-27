using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyInfoUI : MonoBehaviour, IUI
{
    public LvUpSimulation lvUpView;

    public void ActiveUI()
    {
        lvUpView.Clear();
        gameObject.SetActive(true);
    }

    public void NonActiveUI()
    {
        lvUpView.Clear();
        gameObject.SetActive(false);
    }

    public void SetLeftPanel(Card card)
    {
        if (card is FairyCard)
        {
            
        }
        else
        {
            //Set SupCard
        }
    }

    public void SetRightPanel(Card card)
    {
        if (card is FairyCard)
        {
            lvUpView.SetView(card);

            //for (int i = )
        }
        else
        {
            //Set SupCard
        }

    }


    
}
