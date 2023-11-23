using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Logger logger;

    public enum GameState
    {
        MainMenu,
        PreparingGame,
        GameReady,
        Playing,
        GameOver,
    }

    public static event Action<GameState> OnGameStateChanged;

    private GameState currentGameState = GameState.MainMenu;


    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += OnPlayerDied;

        BallSpawner.OnBallsGenerated += OnBallsGenerated;
        SpawnGuard.OnSpawnGuardReady += OnSpawnGuardReady;
        Player.OnPlayerReady += OnPlayerReady;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= OnPlayerDied;

        BallSpawner.OnBallsGenerated -= OnBallsGenerated;
        SpawnGuard.OnSpawnGuardReady -= OnSpawnGuardReady;
        Player.OnPlayerReady -= OnPlayerReady;
    }

    private void Start() => SetGameState(GameState.MainMenu);

    private void OnBallsGenerated(System.Collections.Generic.List<Ball> balls) => logger.LogInfo($"{nameof(OnBallsGenerated)}");

    private void OnSpawnGuardReady() => logger.LogInfo($"{nameof(OnSpawnGuardReady)}");

    private void OnPlayerReady() => logger.LogInfo($"{nameof(OnPlayerReady)}");

    private void OnPlayerDied() => SetGameState(GameState.GameOver);

    public void RestartGame() => SetGameState(GameState.PreparingGame);

    public void SetGameState(int newState)
    {
        currentGameState = (GameState)newState;
        OnGameStateChanged?.Invoke(currentGameState);
    }

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
        OnGameStateChanged?.Invoke(currentGameState);
        logger.LogInfo($"Game state changed to: {currentGameState}");
    }

    public GameState GetGameState() => currentGameState;
}