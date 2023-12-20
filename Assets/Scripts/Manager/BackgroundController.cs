using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 prevCamPos;
    //public GameObject frontBackground;
    public GameObject tailBackground;
    private StageManager sm;

    public GameObject[] farBackgrounds1;
    public float fBfollowSpeed1;
    public GameObject[] farBackgrounds2;
    public float fBfollowSpeed2;
    public GameObject[] farBackgrounds3;
    public float fBfollowSpeed3;
    public GameObject[] nearBackgrounds;

    private float moveTo;

    private int fCounter1 = 0;
    private int fCounter2 = 0;
    private int fCounter3 = 0;
    private int nCounter = 0;
    private float sideSize = 0;
    private GameObject tb;
    private CameraManager cm;

    private void Awake()
    {
        mainCamera = Camera.main;
        prevCamPos = mainCamera.transform.position;
        sm = GetComponent<StageManager>();
        SetStageImages();
        var sr = nearBackgrounds[0].GetComponentInChildren<SpriteRenderer>();
        sideSize = sr.sprite.rect.width / sr.sprite.pixelsPerUnit / 2;
    }

    private void Update()
    {
        moveTo = prevCamPos.x - mainCamera.transform.position.x;
        prevCamPos = mainCamera.transform.position;
        foreach (var item in farBackgrounds1)
        {
            item.transform.position += new Vector3(moveTo * fBfollowSpeed1 * Time.deltaTime, 0);
        }
        foreach (var item in farBackgrounds2)
        {
            item.transform.position += new Vector3(moveTo * fBfollowSpeed2 * Time.deltaTime, 0);
        }
        foreach (var item in farBackgrounds3)
        {
            item.transform.position += new Vector3(moveTo * fBfollowSpeed3 * Time.deltaTime, 0);
        }
        CheckSide(farBackgrounds1, ref fCounter1);
        CheckSide(farBackgrounds2, ref fCounter2);
        CheckSide(farBackgrounds3, ref fCounter3);
        CheckSide(nearBackgrounds, ref nCounter);
    }

    public void SetTailBackground()
    {
        var pos = nearBackgrounds[nCounter + 1].transform.position.x + mainCamera.orthographicSize * mainCamera.aspect * 2;
        pos += tailBackground.GetComponent<SpriteRenderer>().sprite.rect.width / 200 * 3;
        tb = Instantiate(tailBackground, new Vector3(pos, 0), Quaternion.identity);
    }

    private void CheckSide(GameObject[] backgrounds, ref int counter)
    {
        var rightCounter = (counter + 1) % backgrounds.Length;
        var rightgap = backgrounds[rightCounter].transform.position.x - mainCamera.transform.position.x;
        var leftCounter = (counter + 2) % backgrounds.Length;
        var leftgap = backgrounds[leftCounter].transform.position.x - mainCamera.transform.position.x;
        //var sideSize = mainCamera.orthographicSize * mainCamera.aspect;
        if (rightgap < 0)
        {
            backgrounds[leftCounter].transform.position += new Vector3(sideSize * 2, 0);
            counter = (counter + 1) % backgrounds.Length;
        }
        else if (leftgap > 0)
        {
            backgrounds[rightCounter].transform.position -= new Vector3(sideSize * 2, 0);
            counter--;
            if (counter < 0)
            {
                counter += backgrounds.Length;
            }
        }
    }

    private void SetStageImages()
    {
        return;
        var stageId = GameManager.Instance.StageId;
        var table = sm.thisIsStageData.dic[stageId];
        // 테이블 수정 이후 채워 넣기
    }
}
