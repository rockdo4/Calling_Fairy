using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class CommonFunction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="time"></param>
    /// <param name="fadeIn"></param>
    public static IEnumerator FadeImage(Image image, float time, bool fadeIn, Action onSuccess= null)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float a = fadeIn ? (elapsedTime / time) : 1 - (elapsedTime / time);
            image.color = new Color(1, 1, 1, a);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onSuccess?.Invoke();
    }


}
