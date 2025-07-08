using System.Collections.Generic;
using UnityEngine;

public class UIView : MonoBehaviour
{
    private List<DataBinder> binders = new List<DataBinder>();

    private void Awake()
    {
        // 시작할 때 자신의 자식들 중에서 모든 DataBinder를 찾아 리스트에 등록
        GetComponentsInChildren<DataBinder>(true, binders);
    }

    public void Bind(FairyCard fairy)
    {
        foreach (var binder in binders)
        {
            binder.targetModel = fairy; // DataBinder의 targetModel 설정
            binder.UpdateUI();
        }
    }

    public void Bind(Equipment equipment)
    {
        foreach (var binder in binders)
        {
            binder.targetModel = equipment; // DataBinder의 targetModel 설정
            binder.UpdateUI();
        }
    }

    public void UpdateAll()
    {
        foreach (var binder in binders)
        {
            binder.UpdateUI();
        }
    }
}
