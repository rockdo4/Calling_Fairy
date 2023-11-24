using UnityEngine;
using UnityEngine.EventSystems;
public class SkillIcon : MonoBehaviour
{
    public void SetReposition()
    {
        //sp = EventSystem.current.currentSelectedGameObject.GetComponentInParent<SkillSpawn>();
        //Debug.Log(gameObject.name);
        SkillSpawn.Instance.TouchSkill(gameObject);
    }
}