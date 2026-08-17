using UnityEngine;

public class Quest_1 : MonoBehaviour
{
    public bool isPlayerNear;

    public GameObject Quest1_Panel;

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

            if (Quest1_Panel != null)
            {
                Quest1_Panel.SetActive(false);
            }
        }
    }

    public void OpenUI()
    {
        if (Quest1_Panel != null)
        {
            Quest1_Panel.SetActive(true);
        }
    }
}
