using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;

    //player reference
    public Transform player;

    //array references
    Inventory inventory;

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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        inventoryPanel.fromPanel = null;
        equipmentPanel.fromPanel = null;
        containerPanel.fromPanel = null;
    }
}
