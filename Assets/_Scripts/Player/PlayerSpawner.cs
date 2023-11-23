using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Logger logger;
    [SerializeField] private Player playerPrefab;

    private Player player;


    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;

    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
        {
            player.PlayerMovement.EnableMovement();
            logger.LogInfo("Enable player movement");

            player.EnableCollisions();
            logger.LogInfo("Enable player collisions");
        }
    }

    public async Task GeneratePlayerAsync()
    {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
        logger.LogInfo("Player created");

        player.PlayerMovement.DisableMovement();
        logger.LogInfo("Disable player movement");

        player.DisableCollisions();
        logger.LogInfo("Disable player collisions");

        float creationDelay = player.GetComponent<ICreationDelay>().GetCreationDelay();
        await Task.Delay(TimeSpan.FromSeconds(creationDelay));
    }
}
