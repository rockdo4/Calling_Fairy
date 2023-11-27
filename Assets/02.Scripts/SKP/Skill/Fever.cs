using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour
{
    public static Fever Instance;
    public Image[] image = new Image[4];
    public Sprite[] feverSprite = new Sprite[4];
    public GameObject emptyImage;
    private Sprite emptyImageSprite;
    // Start is called before the first frame update
    bool FeverChecker { get; set; }
    private int FeverCount { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        //ChangeFeverSquare();
        //���� �Ųٷ� �����°� ����ٰ� �Ѵٸ�
        //{
        //    int endNum = image.Length;
        //    image[endNum - 1].transform.Rotate(0, 0, 180);
        //}
        emptyImageSprite = emptyImage.GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < image.Length; i++)
        {
            image[i].sprite = emptyImageSprite;
        }
    }

    //�ǹ� ������ ä��� �Լ�
    void Update()
    {
        //if(FeverChecker)

        if(TestManager.Instance.TestCodeEnable)
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GuageCheck();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseFever();
        }
    }

    public void GuageCheck()
    {
        if (FeverCount >= image.Length)
            FeverCount = image.Length;
        FeverCount++;
        //�ǹ� ������ ä��� ��
        if (FeverCount <= image.Length)
            ChangeFeverSquare(FeverCount);
    }
    
    //�ǹ��� ���̸� �̹��� �ٲٱ�
    private void ChangeFeverSquare(int countThreeChain)
    {
        image[countThreeChain-1].sprite = feverSprite[countThreeChain-1];
    }

    //�ǹ� ����ϱ�
    public void UseFever()
    {
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
