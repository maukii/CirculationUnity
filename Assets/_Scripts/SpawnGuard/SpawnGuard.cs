using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SpawnGuard : MonoBehaviour
{
    public static event Action OnSpawnGuardReady;

    [SerializeField] private float targetScale = 2.5f;

    private CircleCollider2D guardArea;
    private AnimateOpen creationAnimation;


    private void Awake()
    {
        guardArea = GetComponent<CircleCollider2D>();
        creationAnimation = GetComponent<AnimateOpen>();
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        creationAnimation.SetTargetScale(Vector3.one * targetScale);
        creationAnimation.SetCallback(OnSpawnGuardCreated);
        creationAnimation.Animate();
    }

    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;

    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnSpawnGuardCreated() => OnSpawnGuardReady?.Invoke();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                float distance = Vector2.Distance(transform.position, ball.transform.position);
                distance -= (transform.localScale.x * guardArea.radius) + ball.Radius;

                if (distance <= 0)
                {
                    Vector2 directionToBall = ball.transform.position - transform.position;
                    Vector2 newPosition = (Vector2)transform.position + directionToBall.normalized * ((transform.localScale.x * guardArea.radius) + ball.Radius);
                    ball.transform.position = newPosition;
                    Vector2 reflectionDirection = Vector2.Reflect(ball.RB.velocity.normalized, directionToBall.normalized);
                    ball.SetVelocity(reflectionDirection * ball.RB.velocity.magnitude);
                }
            }
        }
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
            Destroy(gameObject);
    }
}