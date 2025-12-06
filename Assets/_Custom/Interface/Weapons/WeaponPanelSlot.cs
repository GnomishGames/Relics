using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponsPanelSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Sprite emptyIcon;

    //player reference
    public Transform player;

    //panels
    public InventoryPanel inventoryPanel;
    public EquipmentPanel equipmentPanel;

    //class references
    Inventory inventory;
    Equipment equipment;

    public int slotNumber; //manually set on the interface
    public SlotType slotType;

    private void Awake()
    {
        //set arrays
        inventory = player.GetComponent<Inventory>();
        equipment = player.GetComponent<Equipment>();

        //set ui elements
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        UpdateSlotIcons();
    }

    private void UpdateSlotIcons()
    {
        if (equipment.weaponSOs[slotNumber] != null)
        {
            GetComponent<Image>().sprite = equipment.weaponSOs[slotNumber].sprite;
            GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
        if (equipment.weaponSOs[slotNumber] == null)
        {
            GetComponent<Image>().sprite = emptyIcon;
            GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
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
                equipment.EquipWeapon(inventoryPanel.fromSlot, slotNumber, slotType);
            }

            if (equipmentPanel.fromPanel == "Weapon")
            {
                equipment.MoveWeapon(equipmentPanel.fromSlot, slotNumber, slotType);
            }
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        inventoryPanel.fromPanel = null;
        equipmentPanel.fromPanel = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        equipmentPanel.fromSlot = slotNumber;
        equipmentPanel.fromPanel = "Weapon";
    }

    
}