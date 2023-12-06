using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Slot : MonoBehaviour
{
    public SlotItem SelectedSlotItem { get; set; }

    //해제할 때는 null을 넘기는 방식으로.
    public virtual void SetSlot(SlotItem item)
    {
        SelectedSlotItem = item;
    }
}
