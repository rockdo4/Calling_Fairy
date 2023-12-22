using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetString : MonoBehaviour
{
    public int ID;
    private TMPro.TextMeshProUGUI text;
    private UnityEngine.UI.Text textLagacy;

    private void Awake()
    {
        StringTable.OnLanguageChanged += SetText;
    }

    private void OnEnable()
    {
        if(text == null && textLagacy == null)
        {
            TryGetComponent(out text);
            TryGetComponent(out textLagacy);
        }    
        SetText();
    }

    public void SetText()
    {
        var message = GameManager.stringTable[ID].Value;
        if(text != null)
        {
            text.text = message;
        }
        else if(textLagacy != null)
        {
            textLagacy.text = message;
        }
    }

    private void OnDestroy()
    {
        StringTable.OnLanguageChanged -= SetText;
    }
}
