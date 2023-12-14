using TMPro;
using UnityEngine;

public class DamageDataEffects : Effects
{
    public enum InfoType
    {
        Physical,
        Magical,
        Heal,
        CriticalPhysical,
        CriticalMagical,
        Avoid,
    }
    [HideInInspector]
    public TextMeshPro text;
    [SerializeField]
    private float timer = 0f;
    [SerializeField]
    private float speed = 0.6f;
    [SerializeField]
    private float timeToDestroy = 1f;
    [SerializeField]
    private float stayTime = 0.5f;

    protected new void Awake()
    {
        base.Awake();
        text = GetComponent<TextMeshPro>();
        effectType = EffectType.String;        
    }
    public void SetPositionAndRotation(Vector3 position)
    {        
        base.SetPositionAndRotation(position);        
    }
    public void SetDamage(InfoType info, string str = "Miss")
    {
        transform.localScale = Vector3.one;
        Color color;
        switch (info)
        {
            case InfoType.Physical:
                ColorUtility.TryParseHtmlString("#FF7F50", out color);
                text.color = color;
                text.fontSize = 70f;
                break;
            case InfoType.Magical:
                ColorUtility.TryParseHtmlString("#87CEFA", out color);
                text.color = color;
                text.fontSize = 70f;
                break;
            case InfoType.Heal:
                ColorUtility.TryParseHtmlString("#7FFF00", out color);
                text.color = color;
                text.fontSize = 70f;
                break;
            case InfoType.CriticalPhysical:
                ColorUtility.TryParseHtmlString("#FF0000", out color);
                text.color = color;
                text.fontSize = 80f;
                break;
            case InfoType.CriticalMagical:
                ColorUtility.TryParseHtmlString("#0000FF", out color);
                text.color = color;
                text.fontSize = 80f;
                break;
            case InfoType.Avoid:
                ColorUtility.TryParseHtmlString("#D3D3D3", out color);
                text.color = color;
                text.fontSize = 70f;
                break;
        }
        text.text = str;
    }
    private void OnEnable()
    {
        timer = 0f;
    }

    protected void Update()
    {
        timer += Time.deltaTime;
        transform.LookAt(Camera.main.transform);
        if(timer > stayTime)
        {
            var pos = transform.position;
            pos.y += speed * Time.deltaTime;
            transform.position = pos;
        }
        if(timer > timeToDestroy)
        {
            gameObject.SetActive(false);
        }
    }
}
