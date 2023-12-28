using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSetting : MonoBehaviour
{
    [SerializeField]
    private GameObject[] chapters;
    [SerializeField]
    private int stagesNum = 6;
    private void OnEnable()
    {
        var stageNum = GameManager.Instance.MyBestStageID - 9000;
        stageNum /= stagesNum;
        for (int i = 0; i < chapters.Length; i++)
        {
            if (i <= stageNum)
            {
                chapters[i].SetActive(true);
            }
            else
            {
                chapters[i].SetActive(false);
            }
        }
    }
}
