using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringChange : MonoBehaviour
{
    private StringTable.Language lang;

    [SerializeField]
    private GameObject[] langSwitch = new GameObject[2];
    private bool firstSet = false;
    private void OnEnable()
    {
        if (!firstSet)
        {
            lang = GameManager.Instance.language;
            firstSet = true;
        }
        
        SaveLoadSystem.AutoSave();
        if (lang == StringTable.Language.Korean)
        {
            langSwitch[0].SetActive(true);
            langSwitch[1].SetActive(false);
        }
        else
        {
            langSwitch[0].SetActive(false);
            langSwitch[1].SetActive(true);
        }
    }
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
