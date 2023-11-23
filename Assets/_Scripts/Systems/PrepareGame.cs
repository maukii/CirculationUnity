using System.Threading.Tasks;
using UnityEngine;

public class PrepareGame : MonoBehaviour
{
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private SpawnGuardSpawner spawnGuardSpawner;
    [SerializeField] private PlayerSpawner playerSpawner;


    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;

    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private async void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PreparingGame)
            await SpawnEntitiesSequentially();
    }

    private async Task SpawnEntitiesSequentially()
    {
        await ballSpawner.GenerateBallsAsync();
        await spawnGuardSpawner.GenerateSpawnGuardAsync();
        await playerSpawner.GeneratePlayerAsync();
        await Task.Delay(1000);

        GameManager.Instance.SetGameState(GameManager.GameState.Playing);
    }
}
