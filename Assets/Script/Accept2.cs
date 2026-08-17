using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accept2 : MonoBehaviour
{
    int Quest1_kill;
    public GameObject Quest1_Clear_Panel;
    public static int Quest_progress;
    public void OnButtonClicked()
    {
        Quest1_Clear_Panel.SetActive(false);
        Quest_progress = 2;
    }
}