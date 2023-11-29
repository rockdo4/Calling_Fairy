using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI onText;
    [SerializeField]
    private TextMeshProUGUI offText;

    private void Awake()
    {
        Instance = this;
    }
    public static TestManager Instance;
    public bool TestCodeEnable { get; set; }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestCodeEnable = !TestCodeEnable;
        }

        if(TestCodeEnable)
        {
            onText.gameObject.SetActive(true);
            offText.gameObject.SetActive(false);
        }
        else
        {
            onText.gameObject.SetActive(false);
            offText.gameObject.SetActive(true);
        }
    }
    
}
