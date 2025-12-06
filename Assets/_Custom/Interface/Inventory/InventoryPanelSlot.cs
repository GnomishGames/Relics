using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Sprite emptyIcon;
    private Vector2 originalAnchoredPosition;

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
            GetComponent<Image>().sprite = emptyIcon;
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        // restore this slot's rect position (in case the drag was not dropped on another slot)
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
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
            // snap the dragged item's RectTransform to this slot's anchored position
            var draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
            if (draggedRect != null)
            {
                draggedRect.anchoredPosition = rectTransform.anchoredPosition;
            }
        }
        inventoryPanel.fromPanel = null;
        equipmentPanel.fromPanel = null;
        containerPanel.fromPanel = null;
    }
}