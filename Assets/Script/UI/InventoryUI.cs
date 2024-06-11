using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;
    
    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectItem;
    int selectItemIndex = 0;

    int equipIndex;

    private bool isEquip = false;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
    }

    public void Toggle()
    {
        if(IsOpen())
        {
            inventoryWindow.SetActive(false);
        }

        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    private void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmpty();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        DropItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    private void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null )
            {
                slots[i].Set();
            }

            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmpty()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    private void DropItem(ItemData data)
    {
        if(isEquip == false)
        {
            Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        }
    }

    public void OnDropButton()
    {
        DropItem(selectItem);
        RemoveSelectItem();
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectItem = slots[index].item;
        selectItemIndex = index;

        selectedItemName.text = selectItem.itemName;
        selectedItemDescription.text = selectItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i = 0; i < selectItem.consumable.Length; i++)
        {
            selectedStatName.text += selectItem.consumable[i].type.ToString() + "\n";
            selectedStatValue.text += selectItem.consumable[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectItem.type == ItemType.Consum);
        equipButton.SetActive(selectItem.type == ItemType.Equip && !slots[index].equipped);
        unequipButton.SetActive(selectItem.type == ItemType.Equip && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if(selectItem.type == ItemType.Consum)
        {
            for(int i = 0; i < selectItem.consumable.Length; i++)
            {
                switch(selectItem.consumable[i].type)
                {
                    case ConsumType.Health:
                        condition.Heal(selectItem.consumable[i].value); 
                        break;
                    case ConsumType.Hunger:
                        condition.Eat(selectItem.consumable[i].value); 
                        break;
                }
            }

            RemoveSelectItem();
        }
    }

    private void RemoveSelectItem()
    {
        slots[selectItemIndex].quantity--;

        if (slots[selectItemIndex].quantity <= 0)
        {
            selectItem = null;
            slots[selectItemIndex].item = null;
            selectItemIndex = -1;
            ClearSelectItem();
        }

        UpdateUI();
    }

    private void ClearSelectItem()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnEquipButton()
    {
        if (slots[equipIndex].enabled)
        {
            UnEquip(equipIndex);
        }

        slots[selectItemIndex].equipped = true;
        equipIndex = selectItemIndex;
        CharacterManager.Instance.Player.equip.NewEquip(selectItem);
        UpdateUI();

        isEquip = true;
        SelectItem(selectItemIndex);
    }

    public void UnEquipButton()
    {
        isEquip = false;
        UnEquip(selectItemIndex);
    }

    private void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if(selectItemIndex == index)
        {
            SelectItem(selectItemIndex);
        }
    }
}
