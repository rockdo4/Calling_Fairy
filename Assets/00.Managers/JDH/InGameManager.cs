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

    public void ActiveFeverTime()
    {
        if (feverStack < 1)
            return;
        feverTime = Time.time + feverTimes[feverStack];
        isFeverTime = true;
        feverStack = 0;
    }

    public void StackFeverTime()
    {
        if (feverStack < feverTimes.Length)
            return;
        feverStack++;
    }
}
