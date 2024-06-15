using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ItemSlot : MonoBehaviour
{
    public Item item;
    public int quantity;
    public TextMeshProUGUI itemCountText;
    public TextMeshProUGUI itemNameText;
    public GameObject slotSelectedImageGO;
    public bool slotIsSelected;

    public Image itemSlotImage;

    // Called whenever the item slot needs to be updated

    void Awake()
    {
        UpdateSlotUI();
    }

void Start()
{
    // Inventory.Instance.OnInventoryChanged += UpdateSlotUI;
}
    public void UpdateSlotUI()
    {
        // If item is null, set alpha to 0; otherwise, set it to 1
        itemSlotImage.color = new Color(itemSlotImage.color.r, itemSlotImage.color.g, itemSlotImage.color.b, item != null ? 1f : 0f);

        if(item != null)
        {
            itemSlotImage.sprite = item.icon;
            itemNameText.text = item.name;

        }

        if (item != null && quantity > 1)
        {
            itemCountText.text = quantity.ToString();
        }
        else
        {
            itemCountText.text = "";
        }
    }

    public void ToggleSelection(bool isSelected)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.gemSound, 0.016f, 0.05f);
        if (isSelected)
        {
            // Shows the border around the slot
            slotSelectedImageGO.SetActive(true);
            Inventory.Instance.SetSelectedItemSlot(this.gameObject.GetComponent<ItemSlot>());
            slotIsSelected = true;
            if (item != null)
            {
                itemNameText.text = item.name;
                //if slot is not empty, change the item actions text depending on itemType
                if (Inventory.Instance.GetSelectedItem().item.type == ItemType.Equipment)
                {

                    if (Inventory.Instance.isHoldingItem == false)
                    {
                        HUD.Instance.infoText.text = "F - EQUIP/ UNEQUIP \ng - grab";

                    }
                    else
                    {
                        HUD.Instance.infoText.text = "F - EQUIP/ UNEQUIP \ng - place";

                    }

                }
                else
                {
                    if (Inventory.Instance.isHoldingItem == false)
                    {
                        HUD.Instance.infoText.text = "g - grab";
                    }
                    else
                    {
                        HUD.Instance.infoText.text = "g - place";
                    }
                }
            }
        }
        else
        {
            Inventory.Instance.SetSelectedItemSlot(null);
            itemNameText.text = "empty slot";
            HUD.Instance.infoText.text = "";
            slotSelectedImageGO.SetActive(false);

            slotIsSelected = false;
        }
    }


    public void AddItem(Item newItem, int amount = 1)
    {

        if(newItem == null){
        Debug.Log("new item was null");
        return;
        }

        Debug.Log("Added new item: " + newItem.name);
        if (item == null)
        {
            item = newItem;
            itemSlotImage.sprite = newItem.icon;
            quantity = amount;
        }
        else if (item == newItem && item.isStackable)
        {
            quantity += amount;
        }

        UpdateSlotUI(); // Update the slot after adding an item
    }

    public void RemoveItem(int amount)
    {
        if (item != null && item.isStackable)
        {
            quantity -= amount;
            if (quantity <= 0)
            {
                item = null;
                quantity = 0;
            }
        }

        UpdateSlotUI(); // Update the slot after removing an item
    }

    public void SwapItemWithThis(ItemSlot slotToSwapWith)
    {

        // Swap items
        Item tempItem = item;
        item = slotToSwapWith.item;

        slotToSwapWith.item = tempItem;

        if(tempItem != null)
        {
        }

        // Swap quantities if the items are stackable
        if (item.isStackable && slotToSwapWith.item.isStackable)
        {
            int tempQuantity = quantity;
            quantity = slotToSwapWith.quantity;
            slotToSwapWith.quantity = tempQuantity;
        }

        // Update UI for both slots
        UpdateSlotUI();
        slotToSwapWith.UpdateSlotUI();
    }

    public void ClearSlot()
    {
        //item
        item = null;
        quantity = 0;

        UpdateSlotUI(); // Update the slot after clearing it
    }
}
