using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private AutoPlay autoPlay;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        Debug.Log("OnToggleValueChanged ½ÇÇà");
        autoPlay.SetAutoPlay(isOn);
    }
}
