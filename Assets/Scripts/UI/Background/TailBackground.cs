using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBackground : MonoBehaviour
{
    private StageManager stageManager;
    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Fairy>() != null)
        {
            GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>().StopMoving();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(stageManager.IsStageEnd)
        {
            return;
        }
        if (collision.gameObject.GetComponent<Fairy>() != null)
        {
            GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>().SetTarget(collision.gameObject);
        }
    }
}
