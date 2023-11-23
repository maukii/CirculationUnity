using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] Transform[] healthPointVisuals = new Transform[3];


    private void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void OnDisable() => PlayerHealth.OnPlayerDamaged -= UpdateHealthUI;

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthPointVisuals.Length; i++)
            healthPointVisuals[i].gameObject.SetActive(i < playerData.CurrentHealth);
    }
}
