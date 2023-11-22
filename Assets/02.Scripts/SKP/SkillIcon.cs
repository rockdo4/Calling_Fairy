using UnityEngine;
using UnityEngine.EventSystems;
public class SkillIcon : MonoBehaviour
{
    private SkillSpawn sp;
    public SkillInfo skillInfo;
    public void SetReposition()
    {
        sp = EventSystem.current.currentSelectedGameObject.GetComponentInParent<SkillSpawn>();
        Debug.Log(gameObject.name);
        SkillSpawn.Instance.TouchSkill(gameObject);


       
        //Debug.Log(gameObject);
        //skillInfo = EventSystem.current.currentSelectedGameObject.GetComponentInParent<SkillInfo>();
        //sp.skillWaitList[]
        //Debug.Log(this.skillInfo.Stage);
        for (int i = 0; i < sp.skillWaitList.Count; i++)
        {
            
            //Debug.Log(sp.skillWaitList[i].Stage);
            //if(sp.skillWaitList[i].Stage == skillInfo.Stage)
            //{
            //    Debug.Log(i);
           // }
        }
        //Debug.Log(sp.skillWaitList[].Stage);
        //Debug.Log(sp);

        //Debug.Log(skillInfo.SkillObject.gameObject);
        //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInParent<SkillInfo>().TargetPos);
    }
}