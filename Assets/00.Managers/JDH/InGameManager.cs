using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public int feverStack;
    public float[] feverTimes = new float[4];
    private float feverTime;

    public bool isFeverTime = false;

    private void Update()
    {
        if (!isFeverTime)
            return;
        if (feverTime < Time.time)
            isFeverTime = false;
    }

    public void AvtiveFeverTIme()
    {
        if (feverStack < 1)
            return;
        feverTime = Time.time + feverTimes[feverStack];
        isFeverTime = true;
        feverStack = 0;
    }

    public void StackFeverTime()
    {
        feverStack++;
        if (feverStack < feverTimes.Length)
            return;
    }
}
