using UnityEngine;

public class StringChange : MonoBehaviour
{
    private StringTable.Language lang;
    private StringTable.Language prevLang;
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
        //SaveLoadSystem.AutoSave();
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
        prevLang = lang;
    }
    public void SwitchSetting()
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
            //SwitchSetting();
            StringTable.ChangeLanguage(StringTable.Language.Korean);
            lang = StringTable.Language.Korean;
        }
        else
        {
            StringTable.ChangeLanguage(StringTable.Language.English);
            lang = StringTable.Language.English;
            //SwitchSetting();
            
        }
        SaveLangSetting();
    }
    public void ReturnLangSetting()
    {
        lang = prevLang;
        SaveLangSetting();
    }
    public void SaveLangSetting()
    {
        UIManager.Instance.OnMainSceneUpdateUI.Invoke();
        SaveLoadSystem.SaveData.Language = lang;
        SaveLoadSystem.AutoSave();
    }
}
