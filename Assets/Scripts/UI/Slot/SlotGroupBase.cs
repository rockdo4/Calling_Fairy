using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SlotGroupBase : MonoBehaviour
{
    [Tooltip("슬롯이 비워져 있을 때 발생하는 이벤트")]
    public UnityEvent OnSlotEmpty;
    [Tooltip("슬롯이 채워져 있을 때 발생하는 이벤트")]
    public UnityEvent OnSlotFilled;
}
