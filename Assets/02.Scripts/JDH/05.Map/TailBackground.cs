using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBackground : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Fairy>() != null)
        {
            GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>().StopMoving();
        }
    }
}
