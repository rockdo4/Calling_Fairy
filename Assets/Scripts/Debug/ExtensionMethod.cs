using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
#region component
    /// <summary>
    /// 추가하려는 컴포넌트를 검사하는 코드
    /// </summary>
    /// <param name="comp">추가 할 컴포넌트</param>
    /// <param name="go">컴포넌트가 추가 될 게임 오브젝트</param>
    public static Component CheckComp<T>(this Component comp, GameObject go) where T : Component
    {
        if (!comp)
            return comp;

        Debug.Log(go.name + "의 오브젝트 중" + typeof(T) + "컴포넌트가 할당 되지 않았습니다.");
        return go.GetComponent<T>();
    }
#endregion
}