using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringChange : MonoBehaviour
{
    private StringTable.Language lang = StringTable.Lang;
    
    [SerializeField]
    private GameObject[] langSwitch = new GameObject[2];
    private void SwitchSetting()
    {
        if (langSwitch[0].activeSelf)
        {
            langSwitch[0].SetActive(false);
            langSwitch[1].SetActive(true);
        }
        else
        {
            langSwitch[0].SetActive(true);
            langSwitch[1].SetActive(false);
        }
    }

    public void ChangeLanguage()
    {
        if (langSwitch[0].activeSelf)
        {
            SwitchSetting();
            StringTable.ChangeLanguage(StringTable.Language.English);
            lang = StringTable.Language.English;
        }
        else
        {
            SwitchSetting();
            StringTable.ChangeLanguage(StringTable.Language.Korean);
            lang = StringTable.Language.Korean;
        }
        SaveLoadSystem.SaveData.Language = lang;
        SaveLoadSystem.AutoSave();
    }
}
