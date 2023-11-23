using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Audio")]
    [field: SerializeField] public string playerDamagedAudioId = "PlayerDamaged";
    [field: SerializeField] public string playerDiedAudioId = "PlayerDied";

    [Header("Scale")]
    [field: SerializeField] public float PlayerScale = 0.35f;

    [Header("Health")]
    [field: SerializeField] public int MaxHealth = 3;
    [field: SerializeField] public int CurrentHealth = -1;

    [Header("Movement")]
    [field: SerializeField] public float ForwardSpeed = 5.0f;
    [field: SerializeField] public float RotationSpeed = 100.0f;
}
