using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ExplosiveJump.Asset", menuName = "ExplosiveJumpInfo")]
public class SOExplosiveJump : ScriptableObject
{
    public float minForce = 20f;
    public float maxForce = 30f;
    [Range(0, 90)]
    public float minAngle = 20f;
    [Range(0, 90)]
    public float maxAngle = 70f;
    public float rollingRate = 300f;
}
