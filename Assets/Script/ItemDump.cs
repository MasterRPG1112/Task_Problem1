using UnityEngine;

public class ItemDump : MonoBehaviour
{
    [Header("참조 연결")]
    public ShopManager shopManager;
    public ItemSwitch itemSwitch;

    void Update()
    {
        if (Input.GetButtonDown("ItemDump") || Input.GetKeyDown(KeyCode.Q))
        {
            DumpCurrentItem();
        }
    }
    public void DumpCurrentItem()
    {
        if (itemSwitch == null)
        {
            Debug.LogWarning("[ItemDump] ItemSwitch 참조가 설정되지 않았습니다.");
            return;
        }

        Item currentItem = itemSwitch.currentEquippedItem;

        if (currentItem == null)
        {
            Debug.Log("[ItemDump] 현재 손에 들고 있는 아이템이 없습니다.");
            return;
        }

        if (currentItem.isConsumable)
        {
            if (currentItem.itemQuantity > 0)
            {
                currentItem.itemQuantity -= 1;
                Debug.Log($"[ItemDump] {currentItem.itemName} 1개를 버렸습니다. (남은 수량: {currentItem.itemQuantity}개)");
            }
        }

        else
        {
            currentItem.isPurchased = false;
            Debug.Log($"[ItemDump] {currentItem.itemName}을(를) 버렸습니다.");
        }

        if (itemSwitch.currentIndex >= 0 && itemSwitch.currentIndex < itemSwitch.itemSlots.Length)
        {
            ItemSlot currentSlot = itemSwitch.itemSlots[itemSwitch.currentIndex];
            if (currentSlot != null)
            {
                currentSlot.UpdateSlotUI();
            }
        }

        if (shopManager != null && !shopManager.HasItem(currentItem))
        {
            Debug.Log($"[ItemDump] {currentItem.itemName}을(를) 더 이상 소지하지 않아 다음 아이템으로 스위칭합니다.");
            itemSwitch.SwitchToNextItem();
        }
    }
}
