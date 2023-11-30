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

    //피버 게이지 채우는 함수
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
                Debug.Log("피버시간 끝");
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
        //피버 게이지 채우는 중
        if (FeverCount <= image.Length)
            ChangeFeverSquare(FeverCount);
    }

    //피버가 쌓이면 이미지 바꾸기
    private void ChangeFeverSquare(int countThreeChain)
    {
        image[countThreeChain - 1].sprite = feverSprite[countThreeChain - 1];
    }

    //피버 사용하기
    public void UseFever()
    {
        if (FeverCount < 2)
        {
            feverText.text = "피버게이지가\n모자랍니다.";
            feverText.gameObject.SetActive(true);
            removedTime = 0;

            Debug.Log("피버게이지가 모자랍니다");
            return;
        }
        Debug.Log("피버시작");
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

    //피버 소모시 이미지 변경
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
