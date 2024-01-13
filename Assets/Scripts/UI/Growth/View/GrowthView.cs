using UnityEngine;

public abstract class GrowthView : MonoBehaviour
{
    public FairyCard SelectFairy { get; set; }

    public abstract void UpdateUI();
}
