using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBarPanelSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
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
    public SkillBar skillBar;

   //panel references
    public SkillBarPanel skillBarPanel;
    public SkillBookPanel skillBookPanel;
    //public ContainerPanel containerPanel;

    public TMP_Text cooldownText;
    
    public int slotNumber;  //manually set on the interface

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        //draglayer keeps icons on top when dragging
        dragLayer = GameObject.FindWithTag("DragLayer").transform;
    }

    private void Update()
    {
        UpdateSlotIcons();//move to timer or events
    }

    private void UpdateSlotIcons()
    {
        if (skillBar.skillSOs[slotNumber] != null)
        {
            GetComponent<Image>().sprite = skillBar.skillSOs[slotNumber].sprite;
            GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
        if (skillBar.skillSOs[slotNumber] == null)
        {
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }

        //update cooldown text
        float timer = skillBar.skillTimer[slotNumber];
        if (timer > 0)
        {
            cooldownText.text = Mathf.CeilToInt(timer).ToString();
        }
        else
        {
            cooldownText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //null check
        if (skillBar.skillSOs[slotNumber] == null)
        {
            Debug.LogWarning("No skill assigned to this slot.");
            return;
        }

        skillBar.DoSkill(slotNumber, skillBar.skillTimer[slotNumber]);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        skillBarPanel.fromSlot = slotNumber;
        skillBarPanel.fromPanel = "SkillBar";
        
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
        // remember the original anchored position so we can restore if the drag is cancelled
        originalAnchoredPosition = rectTransform.anchoredPosition;
        // remember original parent and sibling index so we can restore later
        originalParent = rectTransform.parent;
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        // choose drag layer
        if (dragLayer == null)
        {
            dragLayer = (canvas != null) ? canvas.transform : rectTransform.root;
        }

        // preserve world position and reparent to drag layer so it renders on top
        Vector3 worldPos = rectTransform.position;
        rectTransform.SetParent(dragLayer, false);
        rectTransform.position = worldPos;
        rectTransform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        // restore this slot's parent and rect position (in case the drag was not dropped on another slot)
        if (originalParent != null && rectTransform.parent != originalParent)
        {
            // preserve world pos while reparenting
            Vector3 worldPos = rectTransform.position;
            rectTransform.SetParent(originalParent, false);
            rectTransform.position = worldPos;
        }

        rectTransform.anchoredPosition = originalAnchoredPosition;
        rectTransform.SetSiblingIndex(originalSiblingIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move in world space while on drag layer so it tracks the pointer correctly
        rectTransform.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (skillBarPanel.fromPanel == "SkillBar")
            {
                skillBar.MoveSkill(skillBarPanel.fromSlot, slotNumber);
            }
            if (skillBookPanel.fromPanel == "SkillBook")
            {
                skillBar.UnEquipSkill(slotNumber, skillBookPanel.fromSlot);
            }
            
            // snap the dragged item's RectTransform to this slot's anchored position and reparent it back
            var draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            if (draggedRect != null)
            {
                // reparent to this slot's parent so anchoredPosition aligns correctly
                var targetParent = rectTransform.parent;
                Vector3 worldPos = draggedRect.position;
                draggedRect.SetParent(targetParent, false);
                // preserve world pos to avoid jump, then set anchored to slot
                draggedRect.position = worldPos;
                draggedRect.anchoredPosition = rectTransform.anchoredPosition;
                // restore sibling so it sits in the same slot place
                draggedRect.SetSiblingIndex(rectTransform.GetSiblingIndex());
            }
        }
        //containerPanel.fromPanel = null;
        skillBookPanel.fromPanel = null;
        skillBarPanel.fromPanel = null;
    }
}
