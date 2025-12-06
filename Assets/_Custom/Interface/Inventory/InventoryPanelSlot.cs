using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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
    public Inventory inventory;

    //panel references
    public InventoryPanel inventoryPanel;
    public EquipmentPanel equipmentPanel;
    public ContainerPanel containerPanel;
    
    public int slotNumber;  //manually set on the interface

    private void Awake()
    {
        inventory = player.GetComponent<Inventory>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        //draglayer keeps icons on top when dragging
        dragLayer = GameObject.FindWithTag("DragLayer").transform;
    }

    private void Update()
    {
        UpdateSlotIcons();//move to timer
    }

    private void UpdateSlotIcons()
    {
        if (inventory.inventoryItem[slotNumber] != null)
        {
            GetComponent<Image>().sprite = inventory.inventoryItem[slotNumber].sprite;
            GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
        if (inventory.inventoryItem[slotNumber] == null)
        {
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        inventoryPanel.fromSlot = slotNumber;
        inventoryPanel.fromPanel = "Inventory";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
            if (inventoryPanel.fromPanel == "Inventory")
            {
                inventory.MoveItem(inventoryPanel.fromSlot, slotNumber);
            }
            if (equipmentPanel.fromPanel == "Armor")
            {
                inventory.UnEquipArmor(slotNumber, equipmentPanel.fromSlot);
            }
            if (equipmentPanel.fromPanel == "Weapon")
            {
                inventory.UnEquipWeapon(slotNumber, equipmentPanel.fromSlot);
            }
            if(containerPanel.fromPanel == "Container")
            {
                inventory.LootItem(slotNumber, containerPanel.fromSlot);
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
        inventoryPanel.fromPanel = null;
        equipmentPanel.fromPanel = null;
        containerPanel.fromPanel = null;
    }
}