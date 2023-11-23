using UnityEngine;

[CreateAssetMenu(fileName = "BallSettings", menuName = "BallSettings")]
public class BallSettings : ScriptableObject
{
    [Header("Audio")]
    [field: SerializeField] public string BallCreatedAudioId = "BallCreated";
    [field: SerializeField] public string BallBounceAudioId = "BallBounce";
    [field: SerializeField] public float BallBounceMinVolume = 1.0f;
    [field: SerializeField] public float BallBounceMaxVolume = 4.0f;

    [Header("Generation settings")]
    [field: SerializeField] public Ball ballPrefab;
    [field: SerializeField] public int BallAmount = 10;
    [field: SerializeField] public float BallMinRadius = 0.5f;
    [field: SerializeField] public float BallMaxRadius = 25.0f;
    [field: SerializeField] public float BallMinSpeed = 0.25f;
    [field: SerializeField] public float BallMaxSpeed = 1.0f;
    [field: SerializeField] public float BallMinTorque = -5.0f;
    [field: SerializeField] public float BallMaxTorque = 5.0f;
    [field: SerializeField] public float BallMassMultiplier = 5.0f;

    [Header("Animation")]
    [field: SerializeField] public float BallMinCreationTime = 0.25f;
    [field: SerializeField] public float BallMaxCreationTime = 0.5f;
}
