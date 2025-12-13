using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillPanelSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    // when clicked, this slot will use the assigned skill
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalAnchoredPosition;
    private Transform originalParent;
    private int originalSiblingIndex;
    [Tooltip("Optional transform (e.g. top-level canvas) to reparent the dragged item to while dragging. If empty, script uses the assigned Canvas.transform.")]
    private Transform dragLayer;

    //player reference
    public Transform player;

    //array references
    public SkillBook skillBook;

    //panel references
    public SkillPanel skillPanel;

    public int slotNumber;  //manually set on the interface

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        skillBook = player.GetComponent<SkillBook>();
        skillPanel = player.GetComponent<SkillPanel>();
        rectTransform = GetComponent<RectTransform>();


        //draglayer keeps icons on top when dragging
        dragLayer = GameObject.FindWithTag("DragLayer").transform;
    }

    private void Update()
    {
        UpdateSlotIcons();//move to events
    }

    private void UpdateSlotIcons()
    {
        if (skillPanel.skillSOs[slotNumber] != null)
        {
            GetComponent<Image>().sprite = skillPanel.skillSOs[slotNumber].sprite;
            GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
        if (skillPanel.skillSOs[slotNumber] == null)
        {
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        skillPanel.fromSlot = slotNumber;
        skillPanel.fromPanel = "skillPanel";

        //use skill
        if (skillPanel.skillSOs[slotNumber] != null)
        {
            Debug.Log("Using skill in slot " + slotNumber);

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
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
}
