using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vc;
    public GameObject vcObject;
    protected Vector3 prevCamPos;

    public void SetTarget(GameObject target)
    {
        vc.Follow = target.transform;
        vc.LookAt = target.transform;
    }

    public void StopMoving()
    {
        vc.Follow = null;
        vc.LookAt = null;
    }
    
    public void MoveTo(Vector3 moveCam)
    {
        vc.transform.position += moveCam;
    }
}
