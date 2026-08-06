using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public Item itemData;
    public ShopManager shopManager;

    public void OnClickSlot()
    {
        if (shopManager != null && itemData != null)
        {
            shopManager.SelectItem(itemData);
        }
    }
}