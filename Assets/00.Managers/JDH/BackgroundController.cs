using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject frontBackground;
    public GameObject[] middleBackgrounds;
    public GameObject tailBackground;

    private float spriteHalfWidth;
    private float tailBackgroundHalfSize;
    private int mbCounter;
    private GameObject tb;
    private CameraManager cm;

    private void Awake()
    {
        mainCamera = Camera.main;
        cm = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
        spriteHalfWidth = middleBackgrounds[0].GetComponent<SpriteRenderer>().sprite.rect.width / 200;
        tailBackgroundHalfSize = middleBackgrounds[0].GetComponent<SpriteRenderer>().sprite.rect.width / 200;
    }

    private void Update()
    {
        var centerGap = middleBackgrounds[mbCounter].transform.position.x - mainCamera.transform.position.x;
        var rightSide = centerGap - spriteHalfWidth + (mainCamera.orthographicSize);
        var leftSide = centerGap + spriteHalfWidth - (mainCamera.orthographicSize);
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
        if(tb != null)
        {
            centerGap = tb.transform.position.x - mainCamera.transform.position.x;
            rightSide = centerGap - tailBackgroundHalfSize + (mainCamera.orthographicSize);
            if(rightSide < 0.01)
            {
                cm.ToggleVC();
            }
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
