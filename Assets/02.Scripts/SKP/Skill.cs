using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : PoolAble
{
    GameObject skillId;

    public void Rset()
    {
        ReleaseObject();
    }
}
