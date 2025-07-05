using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
                return FindObjectOfType<CutSceneManager>();
        }
    }
    private static CutSceneManager _instance;

    [SerializeField] private Image _image;
    private Coroutine cutSceneCoroutine;
    private float cutSceneTime = 2f;
    public event Action<int> EventRiseCutSceneCount;


    public void RiseCutSceneCount(int num)
    {
        EventRiseCutSceneCount?.Invoke(num);
    }

    public void CutSceneEffect(bool cutScene)
    {
        if (cutSceneCoroutine != null)
        {
            StopCoroutine(cutSceneCoroutine);
            cutSceneCoroutine = null;
        }
        _image.gameObject.SetActive(true);
        cutSceneCoroutine = StartCoroutine(CommonFunction.FadeImage(_image, cutSceneTime, false, () =>
        {
            _image.gameObject.SetActive(false);
        }));
    }
}
