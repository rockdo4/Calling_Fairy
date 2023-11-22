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

    private void Awake()
    {
        mainCamera = Camera.main;

        spriteHalfWidth = middleBackgrounds[0].GetComponent<SpriteRenderer>().sprite.rect.width / 200;
    }

    private void Update()
    {
        var centerGap = middleBackgrounds[mbCounter].transform.position.x - mainCamera.transform.position.x;
        var rightSide = -centerGap + spriteHalfWidth - (mainCamera.rect.width / 2);
        var leftSide = centerGap + spriteHalfWidth - (mainCamera.rect.width / 2);
        if (rightSide < 0.5)
        {
            var pos = middleBackgrounds[mbCounter].transform.position.x;
            mbCounter++;
            if (mbCounter >= middleBackgrounds.Length)
            {
                mbCounter = 0;
            }
            pos += spriteHalfWidth * 2;
            middleBackgrounds[mbCounter].transform.position = new Vector2(pos, 0);
        }
        if(leftSide < 0.5)
        {
            var pos = -middleBackgrounds[mbCounter].transform.position.x;
            mbCounter++;
            if (mbCounter >= middleBackgrounds.Length)
            {
                mbCounter = 0;
            }
            pos -= spriteHalfWidth * 2;
            middleBackgrounds[mbCounter].transform.position = new Vector2(pos, 0);
        }
    }

    public void SetTailBackground()
    {

    }
}
