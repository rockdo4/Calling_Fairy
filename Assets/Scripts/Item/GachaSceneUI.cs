using TMPro;
using UnityEngine;

public class GachaSceneUI : MonoBehaviour
{
    [SerializeField]
    private GameObject gachaSkipIcon;
    [SerializeField]
    private Sprite gachaSprite;
    [SerializeField]
    private TextMeshProUGUI gachaName;
    [SerializeField]
    private TextMeshProUGUI gachaDescription;
    GachaLogic gL;
    private void Awake()
    {
        gL = GetComponentInParent<GachaLogic>();
    }

    private void GachaDirect(int ID)
    {
        SkipIconSet();
        //CharData.


    }

    private void SkipIconSet()
    {
        if (gL.tenTimes)
        {
            gachaSkipIcon.SetActive(true);
        }
        else
        {
            gachaSkipIcon.SetActive(false);
        }
    }
}
