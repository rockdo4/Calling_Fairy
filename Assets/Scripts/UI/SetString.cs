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
            if(StringTable.Lang == StringTable.Language.Korean)
            {
                text.font = Resources.Load<TMPro.TMP_FontAsset>("Fonts/DNFBitBitOTF SDF");
            }
            text.text = message;
        }
        else if(textLagacy != null)
        {
            if (StringTable.Lang == StringTable.Language.Korean)
            {
                textLagacy.font = Resources.Load<Font>("Fonts/DNFBitBitOTF");
            }
            textLagacy.text = message;
        }
    }

    private void OnDestroy()
    {
        StringTable.OnLanguageChanged -= SetText;
    }
}
