using UnityEngine;
using UnityEngine.EventSystems;

public class AutoAttack : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    SkillBar skillBar;

    private void Awake()
    {
        skillBar = GetComponentInParent<SkillBar>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleAutoAttack();
    }

    public void ToggleAutoAttack()
    {
        skillBar.autoattackOn = !skillBar.autoattackOn;
    }
}
