using TMPro;
using UnityEngine;
public class TestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI explainText;
    public static TestManager Instance;
    PanelDebug panelDebug;

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
        panelDebug = GameObject.FindWithTag(Tags.DebugMgr).GetComponent<PanelDebug>();
    }
    public bool TestCodeEnable { get; set; }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestCodeEnable = !TestCodeEnable;
        }
        if(TestCodeEnable)
        {
            panelDebug.gameObject.SetActive(true);
            text.color = Color.green;
            text.text = "TestMode : On";
            explainText.color = Color.green;
            explainText.text = "F2 = TestMode On/Off\nD = ĳ���� �� ���� ����\nF = ĳ���� ���� ����\nC = �ǹ� ������ �� ĭ ����\nV = �ǹ������� ���";
        }
        else
        {
            panelDebug.gameObject.SetActive(false);
            text.color = Color.red;
            text.text = "TestMode : Off";
            explainText.color = Color.red;
            explainText.text = "F2 = TestMode On/Off";
        }

        
    }
    
}
