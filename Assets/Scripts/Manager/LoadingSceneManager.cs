using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private static int nextSceneIndex;

    [SerializeField]
    private Slider loadingBar;
    [SerializeField]
    private Image background;

    void Start()
    {
        var sprites = Resources.LoadAll<Sprite>("Sprites/Loading");

        background.sprite = sprites[Random.Range(0, sprites.Length - 1)];

        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        SceneManager.LoadScene("LoadingScene");
    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        asyncOperation.allowSceneActivation = false;

        float timer = 0.0f;

        while(!asyncOperation.isDone)
        {
            yield return null;

            if (asyncOperation.progress < 0.9f)
            {
                loadingBar.value = asyncOperation.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loadingBar.value = Mathf.Lerp(0.9f, 1.0f, timer);

                if(loadingBar.value >= 1.0f)
                {
                    asyncOperation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
