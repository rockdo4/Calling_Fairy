using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 prevCamPos;
    //public GameObject frontBackground;
    public GameObject tailBackground;

    public GameObject[] farBackgrounds;
    [Tooltip("원경 따라오는 속도")]
    public float fBfollowSpeed;
    public GameObject[] middleBackgrounds;
    [Tooltip("중경 따라오는 속도(원경보다 느려야함)")]
    public float mBfollowSpeed;
    public GameObject[] nearBackgrounds;

    private float moveTo;

    private int fCounter = 0;
    private int mCounter = 0;
    private int nCounter = 0;
    private GameObject tb;
    private CameraManager cm;

    private void Awake()
    {
        mainCamera = Camera.main;
        prevCamPos = mainCamera.transform.position;
    }

    private void Update()
    {
        /*
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
        if (leftSide < 0.01)
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
        if (tb != null)
        {
            centerGap = tb.transform.position.x - mainCamera.transform.position.x;
            rightSide = centerGap - tailBackgroundHalfSize + (mainCamera.orthographicSize);
            if (rightSide < 0.01)
            {
                cm.StopMoving();
            }
        }
        */
        moveTo = prevCamPos.x - mainCamera.transform.position.x;
        prevCamPos = mainCamera.transform.position;
        foreach (var item in farBackgrounds)
        {
            item.transform.position += new Vector3(moveTo * fBfollowSpeed, 0);
        }
        foreach (var item in middleBackgrounds)
        {
            item.transform.position += new Vector3(moveTo * mBfollowSpeed, 0);
        }
        CheckSide(farBackgrounds, fCounter);
        CheckSide(middleBackgrounds, mCounter);
        CheckSide(nearBackgrounds, nCounter);
    }

    public void SetTailBackground()
    {
        var pos = nearBackgrounds[nCounter].transform.position.x;
        pos += tailBackground.GetComponent<SpriteRenderer>().sprite.rect.width / 200 * 3;
        tb = Instantiate(tailBackground, new Vector3(pos, 0), Quaternion.identity);
    }

    public void ActiveTailBackground()
    {
        tb.AddComponent<TailBackground>();
    }

    private void CheckSide(GameObject[] backgrounds, int counter)
    {
        var centerGap = backgrounds[counter].transform.position.x - mainCamera.transform.position.x;
        var sideSize = mainCamera.orthographicSize * mainCamera.aspect;
        if (centerGap > sideSize)
        {
            //backgrounds[counter].transform.position -= new Vector3( * sideSize * 2, 0);
        }
    }
}
