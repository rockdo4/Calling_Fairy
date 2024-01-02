using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class Fever : MonoBehaviour
{
    //[SerializeField]
    //private float[] feverTime = new float[3] { 0.5f, 1f, 2f };
    [SerializeField]
    private SOFever feverInfo;
    public Image[] image = new Image[4];
    public Sprite[] feverSprite = new Sprite[4];
    [SerializeField]
    private GameObject feverImage;
    // Start is called before the first frame update
    public bool FeverChecker { 
        get { return feverChecker; }
        private set 
        {
            feverChecker = value;
            if (value)
            {
                // 피버 시작때
                OnFeverStart?.Invoke();
                feverImage.SetActive(true);
            }
            else
            {
                //피버 끝날 때 
                OnFeverEnd?.Invoke();
                feverImage.SetActive(false);
            }
        } }
    private bool feverChecker;

    public event Action OnFeverStart;
    public event Action OnFeverEnd;
    private int FeverCount { get; set; }
    private float feverTimer;
    private float addedTime;
    private float removedTime;
    public TextMeshProUGUI feverText;
    public TextMeshProUGUI feverOnObject;
    private float testTime;
    private StageManager stageManager;
    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        feverImage.SetActive(false);
        //emptyImageSprite = emptyImage.GetComponent<SpriteRenderer>().sprite;
        //for (int i = 0; i < image.Length; i++)
        //{
        //    image[i].sprite = emptyImageSprite;
        //}
    }

    //피버 게이지 채우는 함수
    void Update()
    {
        //if (TestManager.Instance.TestCodeEnable)
        //{
        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        GuageCheck();
        //    }
        //    //if(FeverChecker)
        //    if (Input.GetKeyDown(KeyCode.V))
        //    {
        //        UseFever();
        //    }
        //}

        if (FeverChecker)
        {
            //feverText.text = "FeverTime : " + (feverTimer - addedTime).ToString("N2");
            //feverText.gameObject.SetActive(true);
            addedTime += Time.deltaTime;

            if (addedTime >= feverTimer)
            {
                addedTime = 0;
                FeverChecker = false;
                //Debug.Log("피버시간 끝");
                //feverText.gameObject.SetActive(false);
            }
        }
        //}
        //removedTime += Time.deltaTime;
        //if (removedTime >= 1f)
        //{
        //    feverText.gameObject.SetActive(false);
        //    removedTime = 0f;
        //}
        TextTest();
    }

    private void TextTest()
    {
        testTime += Time.deltaTime;
        if (FeverCount < 2)
        {
            feverOnObject.gameObject.SetActive(false);
            return;
        }
        else
            feverOnObject.gameObject.SetActive(true);



        if (testTime >= 2f)
        {
            feverOnObject.alpha = 1f;  
            testTime = 0;
        }
        else if (testTime >= 1f)
        {
            feverOnObject.alpha = 0.7f;
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
        image[countThreeChain - 1].gameObject.SetActive(true);
    }

    //피버 사용하기
    public void UseFever()
    {
        if (FeverCount < 2 || stageManager.IsStageEnd)    
            return;     
        FeverChecker = true;
        switch (FeverCount)
        {
            case 2:
                //Debug.Log()
                feverTimer = feverInfo.feverTime[0];
                break;
            case 3:
                feverTimer = feverInfo.feverTime[1];
                break;
            case 4:
                feverTimer = feverInfo.feverTime[2];
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
            i.gameObject.SetActive(false);
        }
    }

    public int GetFeverCount()
    {
        return FeverCount;
    }
}
