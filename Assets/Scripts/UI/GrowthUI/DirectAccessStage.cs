using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DirectAccessStage : MonoBehaviour
{
    public TextMeshProUGUI stageName;
    public Button accessButton;

    public void Init()
    {
        stageName.text = "";
        accessButton.onClick.RemoveAllListeners();
    }
}
