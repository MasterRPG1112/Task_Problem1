using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    public TextMeshProUGUI bossNameText;
    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    private float maxHp;

    public void InitializeUI(string name, float maxHealth, float currentHealth)
    {
        maxHp = maxHealth;

        if (bossNameText != null)
        {
            bossNameText.text = name;
        }

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }

        UpdateHpText(currentHealth);
    }

    public void UpdateHealth(float currentHealth)
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHealth;
        }

        UpdateHpText(currentHealth);
    }

    private void UpdateHpText(float currentHealth)
    {
        if (hpText != null)
        {
            hpText.text = Mathf.CeilToInt(currentHealth) + " / " + Mathf.CeilToInt(maxHp);
        }
    }
}