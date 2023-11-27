using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfoUI : MonoBehaviour, IUI
{
    public void ActiveUI()
    {
        gameObject.SetActive(true);
    }

    public void NonActiveUI()
    {
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

        }
        else
        {
            //Set SupCard
        }

    }
}
