using System;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public Sprite sprite;
    public float itemWeight;
    public int itemHealth;
    public SlotType slotType;

}
