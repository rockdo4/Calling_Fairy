using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    public UnityAction OnOpenModal;
    public UnityAction OnCloseModal;

    private Button button;
    private Transform popupTrsf;
    private int originOrder;
    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(CloseModal);
    }

    public void OpenModal(Transform transform)
    {
        gameObject.SetActive(true);
        popupTrsf = transform;
        originOrder = transform.GetSiblingIndex();
        var panelOrder = this.transform.GetSiblingIndex();
        if (originOrder < panelOrder)
        {
            transform.SetSiblingIndex(panelOrder + 1);
        }

        if (OnOpenModal != null)
        {
            OnOpenModal.Invoke();
        }
    }

    public void CloseModal()
    {
        if (originOrder != popupTrsf.GetSiblingIndex())
        {
            popupTrsf.SetSiblingIndex(originOrder);
        }
        
        gameObject.SetActive(false);

        if (OnCloseModal != null)
        {
            OnCloseModal.Invoke();
        }
        OnOpenModal = null;
        OnCloseModal = null;
    }

}
