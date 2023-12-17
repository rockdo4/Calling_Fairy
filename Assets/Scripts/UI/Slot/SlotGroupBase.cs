using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SlotGroupBase : MonoBehaviour
{
    [Tooltip("������ ����� ���� �� �߻��ϴ� �̺�Ʈ")]
    public UnityEvent OnSlotEmpty;
    [Tooltip("������ ä���� ���� �� �߻��ϴ� �̺�Ʈ")]
    public UnityEvent OnSlotFilled;
}
