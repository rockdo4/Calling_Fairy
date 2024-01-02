using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AutoFontSize : MonoBehaviour
{
    private Text uiText;
    public int minFontSize = 10;
    public int maxFontSize = 40;

    void Start()
    {
        uiText = GetComponent<Text>();
        AdjustFontSize();
    }

    void AdjustFontSize()
    {
        for (int i = maxFontSize; i >= minFontSize; i--)
        {
            uiText.fontSize = i;
            if (uiText.preferredWidth <= uiText.rectTransform.rect.width &&
                uiText.preferredHeight <= uiText.rectTransform.rect.height)
            {
                break;
            }
        }
    }
}
