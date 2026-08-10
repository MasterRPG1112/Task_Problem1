using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TMPro.TMP_Text itemNameText;  // 오른쪽 아이템 이름 텍스트
    public TMPro.TMP_Text itemDescText;  // 오른쪽 아이템 설명/가격 텍스트

    [Header("현재 선택된 아이템")]
    private Item selectedItem;

    void Start()
    {
        ClearSelection();
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;

        itemNameText.text = item.itemName;
        itemDescText.text = $"{item.description}\n\n가격: {item.price} Gold";
    }

    public void OnClickYes()
    {
        if (selectedItem == null)
        {
            Debug.Log("선택된 아이템이 없습니다!");
            return;
        }

        Debug.Log($"{selectedItem.itemName} 구매 완료!");

        ClearSelection();
    }

    public void OnClickNo()
    {
        Debug.Log("아이템 선택 취소");
        ClearSelection();
    }

    private void ClearSelection()
    {
        selectedItem = null;
        itemNameText.text = "아이템을 선택하세요";
        itemDescText.text = "";
    }
}
