using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public Item itemData;
    public ShopManager shopManager;

    [Header("UI 연결")]
    public TMPro.TMP_Text buttonText;

    private void Start()
    {
        UpdateSlotUI();
    }

    public void OnClickSlot()
    {
        if (shopManager != null && itemData != null)
        {
            shopManager.SelectItem(itemData, this);
        }
    }

    public void UpdateSlotUI()
    {
        if (itemData == null || buttonText == null) return;

        if (itemData.isConsumable)
        {
            if (itemData.itemQuantity > 0)
                buttonText.text = $"{itemData.itemName} ({itemData.itemQuantity})";
            else
                buttonText.text = itemData.itemName;
        }
        else
        {
            if (itemData.isPurchased)
                buttonText.text = $"{itemData.itemName} (구매완료)";
            else
                buttonText.text = itemData.itemName;
        }
    }
}