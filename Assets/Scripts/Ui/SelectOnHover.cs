using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectOnHover : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Selectable selectable;
    [SerializeField] private UiView parentView;
    private void Awake()
    {
        if (selectable == null) selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
        parentView?.OnChildSelectableSelected(selectable);
    }
}
