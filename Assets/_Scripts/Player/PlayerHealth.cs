using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDied;

    [SerializeField] private Logger logger;
    [SerializeField] private PlayerData playerData;


    private void Awake() => playerData.CurrentHealth = playerData.MaxHealth;

    public bool IsAlive() => playerData.CurrentHealth > 0;

    public int GetCurrentHealth() => playerData.CurrentHealth;

    public int GetMaxHealth() => playerData.MaxHealth;

    public void TakeDamage()
    {
        playerData.CurrentHealth -= 1;
        logger.LogInfo($"Player take damage - current health: <color=red>{GetCurrentHealth()}</color>");

        if (playerData.CurrentHealth <= 0)
        {
            Die();
            return;
        }

        AudioManager.Instance.PlayAudio(playerData.playerDamagedAudioId);
        OnPlayerDamaged?.Invoke();
    }

    public void Die()
    {
        logger.LogInfo("Player died");
        playerData.CurrentHealth = -1;
        AudioManager.Instance.PlayAudio(playerData.playerDiedAudioId);
        OnPlayerDied?.Invoke();
        Destroy(gameObject);
    }
}