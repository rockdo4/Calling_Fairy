using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownAnimationConnector : MonoBehaviour
{
    private TownCharMove townCharMove;
    private void Awake()
    {
        townCharMove = GetComponentInParent<TownCharMove>();
    }
    public void EndAnimation()
    {
        townCharMove.EndAnimation();
    }
}
