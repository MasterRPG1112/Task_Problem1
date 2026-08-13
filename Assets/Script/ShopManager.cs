using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TMPro.TMP_Text itemNameText;
    public TMPro.TMP_Text itemDescText;
    public Button yesButton;

    [Header("현재 선택된 아이템")]
    private Item selectedItem;
    private ItemSlot selectedSlot;

    void Start()
    {
        ClearSelection();
    }

    public void SelectItem(Item item, ItemSlot slot)
    {
        selectedItem = item;
        selectedSlot = slot;

        itemNameText.text = item.itemName;

        if (item.isConsumable)
        {
            itemDescText.text = $"{item.description}\n\n가격: {item.price} Gold\n<color=yellow>(소지 수량: {item.itemQuantity}개)</color>";
            if (yesButton != null) yesButton.interactable = true;
        }
        else if (item.isPurchased)
        {
            itemDescText.text = $"{item.description}\n\n<color=red>[이미 구매 완료된 상품입니다]</color>";
            if (yesButton != null) yesButton.interactable = false;
        }
        else
        {
            itemDescText.text = $"{item.description}\n\n가격: {item.price} Gold";
            if (yesButton != null) yesButton.interactable = true;
        }
    }

    public void OnClickYes()
    {
        if (selectedItem == null) return;

        if (selectedItem.isConsumable)
        {
            selectedItem.itemQuantity += 1;
            Debug.Log($"{selectedItem.itemName} 구매 완료! (현재 수량: {selectedItem.itemQuantity}개)");
        }
        else
        {
            if (selectedItem.isPurchased)
            {
                Debug.Log("이미 구매한 아이템입니다.");
                return;
            }

            selectedItem.isPurchased = true;
            Debug.Log($"{selectedItem.itemName} 1회성 구매 완료!");
        }

        if (selectedSlot != null)
        {
            selectedSlot.UpdateSlotUI();
        }

        SelectItem(selectedItem, selectedSlot);
    }

    public void OnClickNo()
    {
        ClearSelection();
    }

    private void ClearSelection()
    {
        selectedItem = null;
        selectedSlot = null;
        itemNameText.text = "아이템을 선택하세요";
        itemDescText.text = "";
        if (yesButton != null) yesButton.interactable = true;
    }
}