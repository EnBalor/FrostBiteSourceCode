using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, Interactable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.itemName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        AudioManager.instance.PlaySFX("ItemPickUp");
        Destroy(gameObject);
    }
}