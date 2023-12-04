using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "Stage Info.Asset", menuName = "Status/Stage Info")]
public class SOStageInfo : ScriptableObject
{    
    public GameObject[] waveInfo1;
    public GameObject[] waveInfo2;
    public GameObject[] waveInfo3;
    public GameObject[] waveInfo4;


    public LinkedList<GameObject>[] GetData()
    {
        int temp = 0;
        if (waveInfo1.Length != 0)
            temp++;
        if (waveInfo2.Length != 0)
            temp++;
        if (waveInfo3.Length != 0)
            temp++;
        if (waveInfo4.Length != 0)
            temp++;
        var rtn = new LinkedList<GameObject>[temp];

        for(int i = 0; i < rtn.Length; i++)
        {
            rtn[i] = new();

            var waveinfo = i switch
            {
                0 => waveInfo1,
                1 => waveInfo2,
                2 => waveInfo3,
                3 => waveInfo4,
            };

            foreach(var elemnt in waveinfo)
            {
                rtn[i].AddFirst(elemnt);
            }
        }
        return rtn;
    }
}