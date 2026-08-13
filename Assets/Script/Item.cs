using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemID;
    public string itemName;
    public int price;
    [TextArea]
    public string description;

    [Header("아이템 유형")]
    public bool isConsumable = false;

    [Header("소지 정보")]
    public bool isPurchased = false;
    public int itemQuantity = 0;
}