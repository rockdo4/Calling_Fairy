using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI onText;
    [SerializeField]
    private TextMeshProUGUI offText;
    public static TestManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("TestManager is Singleton!");
            Destroy(gameObject);
        }
    }
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
