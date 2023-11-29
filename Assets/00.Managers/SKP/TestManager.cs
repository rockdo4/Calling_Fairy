using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI explainText;
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
            text.color = Color.green;
            text.text = "TestMode : On";
            explainText.gameObject.SetActive(true);
        }
        else
        {
            text.color = Color.red;
            text.text = "TestMode : Off";
            explainText.gameObject.SetActive(false);
        }
    }
    
}
