using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    #region AudioClip
    /// <summary>
    /// 사운드 이펙트 실행
    /// </summary>
    /// <param name="clip">실행 할 사운트 클립</param>
    public static void PlaySe(this AudioClip clip)
    {
        AudioManager.Instance.PlaySE(clip);
    }
    #endregion

    #region string

    /// <summary>
    /// 어드레서블을 활용한 동기적 게임오브젝트 불러오기
    /// </summary>
    /// <param name="key">게임오브젝트를 불러울 키의 이름</param>
    /// <param name="isTemp">씬이 바뀐 뒤 삭제할지 여부</param>
    /// <returns>불러온 게임오브젝트</returns>
    public static GameObject GetGo(this string key, bool isTemp = true)
    {
        return AddressableController.Instance.GetGameObject(key, isTemp).Result;;
    }

    /// <summary>
    /// 어드레서블을 활용한 비동기적 게임오브젝트 불러오기
    /// </summary>
    /// <param name="key">게임오브젝트를 불러울 키의 이름</param>
    /// <param name="isTemp">씬이 바뀐 뒤 삭제할지 여부</param>
    /// <returns>불러온 게임오브젝트</returns>
    public static Task<GameObject> GetGoAsync(this string key, bool isTemp = true)
    {
        return AddressableController.Instance.GetGameObject(key, isTemp);
    }

    #endregion
}