using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    public static event Action<Ball> OnBallGenerated;
    public static event Action<List<Ball>> OnBallsGenerated;

    [SerializeField] private BallSettings ballSettings;
    [SerializeField] private BoundsData worldBounds;
    [SerializeField] private BoundsData spawnBounds;


    public async Task GenerateBallsAsync()
    {
        List<Ball> balls = new List<Ball>();

        for (int i = 0; i < ballSettings.BallAmount; i++)
        {
            Vector2 position = GetRandomPositionInsideSpawnArea();

            float radius = Random.Range(ballSettings.BallMinRadius, ballSettings.BallMaxRadius);
            float randomVelocityX = Random.Range(ballSettings.BallMinSpeed, ballSettings.BallMaxSpeed) * (Random.value > 0.5f ? 1 : -1);
            float randomVelocityY = Random.Range(ballSettings.BallMinSpeed, ballSettings.BallMaxSpeed) * (Random.value > 0.5f ? 1 : -1);
            Vector2 velocity = new Vector2(randomVelocityX, randomVelocityY);

            float torque = Random.Range(ballSettings.BallMinTorque, ballSettings.BallMaxTorque) * (Random.value > 0.5f ? 1 : -1);

            Ball ball = Instantiate(ballSettings.ballPrefab, position, Quaternion.identity, transform)
                .SetRadius(radius)
                .SetMass(radius * ballSettings.BallMassMultiplier)
                .SetVelocity(velocity)
                .SetTorque(torque);

            balls.Add(ball);

            OnBallGenerated?.Invoke(ball);
            await Task.Delay(TimeSpan.FromSeconds(Random.Range(ballSettings.BallMinCreationTime, ballSettings.BallMaxCreationTime)));
        }

        await Task.Delay(TimeSpan.FromSeconds(ballSettings.BallMaxCreationTime));
        OnBallsGenerated?.Invoke(balls);
    }

    private Vector2 GetRandomPositionInsideSpawnArea()
    {
        float randomX = Random.Range(spawnBounds.bounds.min.x, spawnBounds.bounds.max.x);
        float randomY = Random.Range(spawnBounds.bounds.min.y, spawnBounds.bounds.max.y);
        return new Vector2(randomX, randomY);
    }
}
