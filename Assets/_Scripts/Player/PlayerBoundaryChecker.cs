using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerBoundaryChecker : MonoBehaviour
{
    [SerializeField] private BoundsData worldBounds;

    PlayerHealth playerHealth;
    CircleCollider2D playerCollider;


    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    private void Update() => CheckPlayerBoundary();

    private void CheckPlayerBoundary()
    {
        if (!worldBounds.bounds.Contains(playerCollider.bounds.center))
            playerHealth.Die();
    }
}
