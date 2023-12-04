using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vc;
    public GameObject vcObject;

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

}
