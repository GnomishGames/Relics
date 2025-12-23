using UnityEngine;
using UnityEngine.EventSystems;

public class AutoAttack : MonoBehaviour, IPointerClickHandler
{
    SkillBar skillBar;

    private void Awake()
    {
        skillBar = GetComponentInParent<SkillBar>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        skillBar.autoattackOn = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillBar.autoattackOn = !skillBar.autoattackOn;
    }
}
