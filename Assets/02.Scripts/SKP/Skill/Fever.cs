using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour
{
    public Image[] image = new Image[4];
    public Sprite[] feverSprite = new Sprite[4];
    public GameObject emptyImage;
    private Sprite emptyImageSprite;
    // Start is called before the first frame update
    public bool FeverChecker { get; private set; }
    private int FeverCount { get; set; }
    private float feverTimer;
    private float addedTime;
    private float removedTime;
    public TextMeshProUGUI feverText;
    private void Awake()
    {
        emptyImageSprite = emptyImage.GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < image.Length; i++)
        {
            image[i].sprite = emptyImageSprite;
        }
    }

    //�ǹ� ������ ä��� �Լ�
    void Update()
    {
        if (TestManager.Instance.TestCodeEnable)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GuageCheck();
            }
            //if(FeverChecker)
            if (Input.GetKeyDown(KeyCode.V))
            {
                UseFever();
            }
        }
        
        if(FeverChecker)
        {
            feverText.text = "FeverTime : " + (feverTimer - addedTime).ToString("N2");
            feverText.gameObject.SetActive(true);
            addedTime += Time.deltaTime;
            if (addedTime >= feverTimer)
            {
                addedTime = 0;
                FeverChecker = false;
                Debug.Log("�ǹ��ð� ��");
                feverText.gameObject.SetActive(false);
            }
        }
        removedTime += Time.deltaTime;
        if (removedTime >= 1f)
        {
            feverText.gameObject.SetActive(false);
            removedTime = 0f;
        }
    }

    public void GuageCheck()
    {
        if (FeverChecker)
            return;
        FeverCount++;
        if (FeverCount >= image.Length)
            FeverCount = image.Length;
        //�ǹ� ������ ä��� ��
        if (FeverCount <= image.Length)
            ChangeFeverSquare(FeverCount);
    }

    //�ǹ��� ���̸� �̹��� �ٲٱ�
    private void ChangeFeverSquare(int countThreeChain)
    {
        image[countThreeChain - 1].sprite = feverSprite[countThreeChain - 1];
    }

    //�ǹ� ����ϱ�
    public void UseFever()
    {
        if (FeverCount < 2)
        {
            feverText.text = "�ǹ���������\n���ڶ��ϴ�.";
            feverText.gameObject.SetActive(true);
            removedTime = 0;

            Debug.Log("�ǹ��������� ���ڶ��ϴ�");
            return;
        }
        Debug.Log("�ǹ�����");
        FeverChecker = true;
        switch (FeverCount)
        {
            case 2:
                //Debug.Log()
                feverTimer = 0.5f;
                break;
            case 3:
                feverTimer = 1.5f;
                break;
            case 4:
                feverTimer = 2.5f;
                break;
        }
        FeverCount = 0;
        ReturnFeverImage();
    }

    //�ǹ� �Ҹ�� �̹��� ����
    private void ReturnFeverImage()
    {
        foreach (var i in image)
        {
            i.sprite = emptyImageSprite;
        }
    }

    public int GetFeverCount()
    {
        return FeverCount;
    }
}
