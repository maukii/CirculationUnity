using UnityEngine;

public class PlayerColliderStateHandler : MonoBehaviour
{
    private CircleCollider2D playerCollider;


    private void Awake() => playerCollider = GetComponent<CircleCollider2D>();

    private void Start() => playerCollider.enabled = ShouldCollide();

    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;

    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameManager.GameState state) => playerCollider.enabled = ShouldCollide();

    private bool ShouldCollide() => GameManager.Instance.GetGameState() == GameManager.GameState.Playing;
}
