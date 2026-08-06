using UnityEngine;

public class Merchant : MonoBehaviour
{
    public bool isPlayerNear;

    public GameObject merchantUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            if (merchantUI != null)
            {
                merchantUI.SetActive(false);
            }
        }
    }

    public void OpenUI()
    {
        if (merchantUI != null)
        {
            merchantUI.SetActive(true);
        }
    }
}
