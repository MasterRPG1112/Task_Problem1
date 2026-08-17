using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accept : MonoBehaviour
{
    int Quest1_kill;
    public GameObject Quest1_Panel;
    public GameObject Quest_progress;
    public void OnButtonClicked()
    {
        Quest1_Panel.SetActive(false);
        if (Quest1_kill >= 100)
        {
            GameManager.Quest_progress = 1;
        }
    }
}