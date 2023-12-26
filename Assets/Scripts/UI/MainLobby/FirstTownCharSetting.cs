using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTownCharSetting : MonoBehaviour
{
    private void Awake()
    {
        var firstCharSet = GameObject.FindWithTag(Tags.Canvas).GetComponentInChildren<SettingUI>(true);
        firstCharSet.FirstTownSetting();

    }


}
