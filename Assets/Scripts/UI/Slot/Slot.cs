using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Slot : MonoBehaviour
{
    public SlotItem SelectedSlotItem { get; set; }

    //������ ���� null�� �ѱ�� �������.
    public virtual void SetSlot(SlotItem item)
    {
        SelectedSlotItem = item;
    }
}
