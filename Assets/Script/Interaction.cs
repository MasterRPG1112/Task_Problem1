using UnityEngine;

public class Interaction : MonoBehaviour
{
    bool interaction;

    public Merchant targetMerchant;

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
            if (targetMerchant != null && targetMerchant.isPlayerNear == true)
            {
                targetMerchant.OpenUI();
                Debug.Log("상인과 대화 중...");
            }
        }
    }
}
