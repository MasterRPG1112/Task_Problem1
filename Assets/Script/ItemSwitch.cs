using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    public ShopManager shopManager;

    public ItemSlot[] itemSlots = new ItemSlot[8];

    public int currentIndex = -1;
    public Item currentEquippedItem;

    void Update()
    {
        if (Input.GetButtonDown("ItemSwitch"))
        {
            SwitchToNextItem();
        }
    }

    public void SwitchToNextItem()
    {
        if (shopManager == null || itemSlots == null || itemSlots.Length == 0) return;

        int totalCount = itemSlots.Length;
        int checkCount = 0;
        int nextIndex = currentIndex;

        while (checkCount < totalCount)
        {
            nextIndex = (nextIndex + 1) % totalCount;
            checkCount++;

            if (itemSlots[nextIndex] != null && itemSlots[nextIndex].itemData != null)
            {
                Item targetItem = itemSlots[nextIndex].itemData;

                if (shopManager.HasItem(targetItem))
                {
                    currentIndex = nextIndex;
                    currentEquippedItem = targetItem;
                    Debug.Log($"[스위치 성공] {(currentIndex + 1)}번째: {currentEquippedItem.itemName}");
                    return;
                }
            }
        }

        Debug.Log("소지 중인 아이템이 없습니다.");
    }
}