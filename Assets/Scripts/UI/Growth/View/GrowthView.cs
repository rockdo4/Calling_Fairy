using UnityEngine;

public abstract class GrowthView : MonoBehaviour
{
    [SerializeField]
    protected GrowthController controller;

    public abstract void UpdateUI();

    private void OnEnable()
    {
        UpdateUI();
    }
}
