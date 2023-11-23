using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement), typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    public static event Action OnPlayerReady;

    [SerializeField] private PlayerData playerData;

    public PlayerInput PlayerInput { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }

    private Vector2 normalizedInputDirection;
    private AnimateOpen creationAnimation;
    private CircleCollider2D playerCollider;


    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerHealth = GetComponent<PlayerHealth>();
        creationAnimation = GetComponent<AnimateOpen>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        creationAnimation.SetTargetScale(Vector3.one * playerData.PlayerScale);
        creationAnimation.SetCallback(OnPlayerReady);
        creationAnimation.Animate();
    }

    private void Update() => normalizedInputDirection = PlayerInput.GetInput();

    private void FixedUpdate() => PlayerMovement.Move(normalizedInputDirection);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            PlayerHealth.TakeDamage();

            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
                ball.CollidedWithPlayer();
        }
    }

    public void DisableCollisions() => playerCollider.enabled = false;

    public void EnableCollisions() => playerCollider.enabled = true;
}
