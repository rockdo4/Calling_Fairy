using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Fever.Asset", menuName = "FeverInfo")]
public class SOFever : ScriptableObject
{
    public int[] feverTime = new int[3];
}
