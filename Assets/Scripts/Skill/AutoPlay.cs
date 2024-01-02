
using UnityEngine;
using UnityEngine.UI;
public class AutoPlay : MonoBehaviour
{

    private bool isAutoPlay = false;
    private SkillSpawn skillSpawn;
    [SerializeField]
    private float autoTimer = 0.5f;
    private float addTime = 0;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private Image autoImage;
    [SerializeField]
    private Image TargetImage;
    private void Awake()
    {
        skillSpawn = GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<SkillSpawn>();
        color = Random.ColorHSV();
    }
    public void SetAutoPlay(bool isAuto)
    {
        isAutoPlay = isAuto;
    }
    private void Update()
    {
        if(skillSpawn.stageCreatureInfo.IsStageEnd)
        {
            return;
        }
        addTime += Time.deltaTime;

        AutoImageTurn();
        if (isAutoPlay)
        {
            if (skillSpawn.skillWaitList.Count <= 0 || skillSpawn == null)
                return;
            if (autoTimer < addTime && Mathf.Approximately(skillSpawn.skillWaitList[0].SkillObject.transform.position.x, skillSpawn.GetSkillPos(0).x))
            {
                if (skillSpawn.skillWaitList.Count > 0)
                {
                    skillSpawn.TouchSkill(skillSpawn.skillWaitList[0].SkillObject);
                    addTime = 0f;
                }
            }

        }


    }

    private void AutoImageTurn()
    {
        if (isAutoPlay)
        {
            autoImage.transform.Rotate(0, 0, -200 * speed * Time.deltaTime);
            ChangeColor();
        }
        else
        {
            autoImage.transform.rotation = Quaternion.identity;
            TargetImage.color = Color.white;
        }
    }
    float colorChangeSpeed = 2.0f; // 색상 변경 속도
    float colorChangeInterval = 1.0f; // 색상 변경 간격
    float timeSinceColorChange = 0.0f; // 마지막 색상 변경 이후의 시간
    Color color;
    private void ChangeColor()
    {
        autoImage.color = Color.Lerp(autoImage.color, color, colorChangeSpeed * Time.deltaTime);
        TargetImage.color = Color.Lerp(TargetImage.color, color, colorChangeSpeed * Time.deltaTime);
        timeSinceColorChange += Time.deltaTime;
        if (timeSinceColorChange >= colorChangeInterval)
        {
            color = Random.ColorHSV();
            timeSinceColorChange = 0.0f;
        }
    }
}
