using UnityEngine;

public class Interaction : MonoBehaviour
{
    bool interaction;

    public GameManager Quest_progress;
    public Merchant targetMerchant;
    public Quest_1 targetQuest1;
    public Quest_1_Clear targetQuest1_Clear;

    void Update()
    {
        GetInput();
        InteractionRun();
    }

    void GetInput()
    {
        interaction = Input.GetButtonDown("Interaction");
    }

    void InteractionRun()
    {
        if (interaction == true)
        {
            if (targetQuest1 != null && targetQuest1.isPlayerNear == true)
            {
                if (GameManager.Quest_progress == 0)
                {
                    targetQuest1.OpenUI();
                    Debug.Log("연구원과 대화중");
                }
                if (GameManager.Quest_progress == 1)
                {
                    targetQuest1_Clear.OpenUI();
                    Debug.Log("퀘스트를 클리어함");
                }
            }
            if (targetMerchant != null && targetMerchant.isPlayerNear == true)
            {
                targetMerchant.OpenUI();
                Debug.Log("상인과 대화 중...");
            }
        }
    }
}
