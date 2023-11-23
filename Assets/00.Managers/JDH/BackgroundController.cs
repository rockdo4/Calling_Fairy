using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject frontBackground;
    public GameObject[] middleBackgrounds;
    public GameObject tailBackground;

    private float spriteHalfWidth;
    private int mbCounter;
    private GameObject tb;

    private void Awake()
    {
        mainCamera = Camera.main;

        spriteHalfWidth = middleBackgrounds[0].GetComponent<SpriteRenderer>().sprite.rect.width / 200;
    }

    private void Update()
    {
        var centerGap = middleBackgrounds[mbCounter].transform.position.x - mainCamera.transform.position.x;
        var rightSide = centerGap - spriteHalfWidth + (mainCamera.pixelWidth / 200);
        var leftSide = centerGap + spriteHalfWidth - (mainCamera.pixelWidth / 200);
        if (rightSide < 0.01)
        {
            var pos = middleBackgrounds[mbCounter].transform.position.x;
            mbCounter++;
            if (mbCounter >= middleBackgrounds.Length)
            {
                mbCounter = 0;
            }
            pos += spriteHalfWidth * 3;
            middleBackgrounds[mbCounter].transform.position = new Vector2(pos, 0);
        }
        if(leftSide < 0.01)
        {
            var pos = -middleBackgrounds[mbCounter].transform.position.x;
            mbCounter++;
            if (mbCounter >= middleBackgrounds.Length)
            {
                mbCounter = 0;
            }
            pos -= spriteHalfWidth * 3;
            middleBackgrounds[mbCounter].transform.position = new Vector2(pos, 0);
        }
    }

    public void SetTailBackground()
    {
        var pos = middleBackgrounds[mbCounter].transform.position.x;
        pos += tailBackground.GetComponent<SpriteRenderer>().sprite.rect.width / 200 * 3;
        tb = Instantiate(tailBackground, new Vector3(pos, 0), Quaternion.identity);
    }

    public void ActiveTailBackground()
    {
        tb.AddComponent<TailBackground>();
    }
}
