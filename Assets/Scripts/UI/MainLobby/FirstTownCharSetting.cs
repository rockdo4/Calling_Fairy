using UnityEngine;
public class FirstTownCharSetting : MonoBehaviour
{
    //public AnimatorController animatorController;
    private SettingUI firstCharSet;
    [SerializeField]
    private void Awake()
    {
        firstCharSet = GameObject.FindWithTag(Tags.Canvas).GetComponentInChildren<SettingUI>(true);
        SettingCharSet();
    }
    public void SettingCharSet()
    {
        firstCharSet.FirstTownSetting();
    }
    

}
