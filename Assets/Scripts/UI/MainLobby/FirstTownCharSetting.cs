using UnityEditor.Animations;
using UnityEngine;
public class FirstTownCharSetting : MonoBehaviour
{
    public AnimatorController animatorController;

    private void Awake()
    {
        var firstCharSet = GameObject.FindWithTag(Tags.Canvas).GetComponentInChildren<SettingUI>(true);
        firstCharSet.FirstTownSetting();

    }


}
