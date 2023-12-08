using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject frontBackground;
    public GameObject[] middleBackgrounds;
    private float spriteHalfWidth;
    public GameObject tailBackground;
    private float tailBackgroundHalfSize;

    public GameObject[] farBackgrounds;
    [Tooltip("원경 따라오는 속도")]
    public float fBfollowSpeed;
    private float farBackgroundsHalfWidth;
    public GameObject[] nearBackgrounds;
    [Tooltip("근경 따라오는 속도")]
    public float nBfollowSpeed;
    private float nearBackgroundsHalfWidth;

    private int mbCounter;
    private GameObject tb;
    private CameraManager cm;

    private void Awake()
    {
        mainCamera = Camera.main;
        cm = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
        var sprite = middleBackgrounds[0].GetComponent<SpriteRenderer>().sprite;
        spriteHalfWidth = sprite.rect.width / sprite.pixelsPerUnit / 2f;
        sprite = tailBackground.GetComponent<SpriteRenderer>().sprite;
        tailBackgroundHalfSize = sprite.rect.width / sprite.pixelsPerUnit;
        //sprite = farBackgrounds[0].GetComponent<SpriteRenderer>().sprite;
        farBackgroundsHalfWidth = sprite.rect.width / sprite.pixelsPerUnit;
        //sprite = nearBackgrounds[0].GetComponent<SpriteRenderer>().sprite;
        nearBackgroundsHalfWidth = sprite.rect.width / sprite.pixelsPerUnit;
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
                cm.StopMoving();
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
