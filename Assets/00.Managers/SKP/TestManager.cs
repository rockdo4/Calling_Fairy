using TMPro;
using UnityEngine;
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
            text.color = Color.green;
            text.text = "TestMode : On";
            explainText.color = Color.green;
            explainText.text = "F2 = TestMode On/Off\nD = ĳ���� �� ���� ����\nF = ĳ���� ���� ����\nC = �ǹ� ������ �� ĭ ����\nV = �ǹ������� ���";
        }
        else
        {
            text.color = Color.red;
            text.text = "TestMode : Off";
            explainText.color = Color.red;
            explainText.text = "F2 = TestMode On/Off";
        }

        
    }
    
}
