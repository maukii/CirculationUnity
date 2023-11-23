using System.Collections.Generic;
using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    public static BallCollisionHandler Instance { get; private set; }

    [SerializeField] private BallSettings ballSettings;

    private List<Ball> balls = new List<Ball>();


    private void Awake() => Instance = this;

    private void OnEnable()
    {
        BallSpawner.OnBallGenerated += OnBallSpawned;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        BallSpawner.OnBallGenerated -= OnBallSpawned;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (balls == null)
            return;

        HandleBallCollisions();
    }

    public void RemoveBallFromCollisionHandler(Ball ballToRemove)
    {
        if (balls.Contains(ballToRemove))
            balls.Remove(ballToRemove);

        Destroy(ballToRemove.gameObject);
    }

    private void OnBallSpawned(Ball ball) => balls.Add(ball);

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
            ClearBalls();
    }

    private void HandleBallCollisions()
    {
        List<KeyValuePair<Ball, Ball>> collidingPairs = new List<KeyValuePair<Ball, Ball>>();

        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                Ball ball = balls[i];
                Ball target = balls[j];

                // Dont check collision with self
                if (ball == target)
                    continue;

                if (CirclesOverlap(ball.transform.position, ball.Radius, target.transform.position, target.Radius))
                {
                    collidingPairs.Add(new KeyValuePair<Ball, Ball>(ball, target));

                    float distance = Mathf.Sqrt((ball.transform.position.x - target.transform.position.x) * (ball.transform.position.x - target.transform.position.x) + (ball.transform.position.y - target.transform.position.y) * (ball.transform.position.y - target.transform.position.y));

                    float overlapAmount = 0.5f * (distance - ball.Radius - target.Radius);

                    Vector3 ballPosition = ball.transform.position;
                    Vector3 targetPosition = target.transform.position;
                    ballPosition.x -= overlapAmount * (ballPosition.x - targetPosition.x) / distance;
                    ballPosition.y -= overlapAmount * (ballPosition.y - targetPosition.y) / distance;

                    targetPosition.x += overlapAmount * (ballPosition.x - targetPosition.x) / distance;
                    targetPosition.y += overlapAmount * (ballPosition.y - targetPosition.y) / distance;

                    ball.transform.position = ballPosition;
                    target.transform.position = targetPosition;
                }
            }
        }

        foreach (var c in collidingPairs)
        {
            Ball b1 = c.Key;
            Ball b2 = c.Value;

            float distance = Mathf.Sqrt((b1.transform.position.x - b2.transform.position.x) * (b1.transform.position.x - b2.transform.position.x) + (b1.transform.position.y - b2.transform.position.y) * (b1.transform.position.y - b2.transform.position.y));

            float nx = (b2.transform.position.x - b1.transform.position.x) / distance;
            float ny = (b2.transform.position.y - b1.transform.position.y) / distance;

            float tx = -ny;
            float ty = nx;

            float dpTan1 = b1.RB.velocity.x * tx + b1.RB.velocity.y * ty;
            float dpTan2 = b2.RB.velocity.x * tx + b2.RB.velocity.y * ty;

            float dpNorm1 = b1.RB.velocity.x * nx + b1.RB.velocity.y * ny;
            float dpNorm2 = b2.RB.velocity.x * nx + b2.RB.velocity.y * ny;

            float m1 = (dpNorm1 * (b1.Mass - b2.Mass) + 2.0f * b2.Mass * dpNorm2) / (b1.Mass + b2.Mass);
            float m2 = (dpNorm2 * (b2.Mass - b1.Mass) + 2.0f * b1.Mass * dpNorm1) / (b1.Mass + b2.Mass);

            Vector2 b1Velocity = b1.RB.velocity;
            Vector2 b2Velocity = b2.RB.velocity;

            b1Velocity.x = tx * dpTan1 + nx * m1;
            b1Velocity.y = ty * dpTan1 + ny * m1;
            b2Velocity.x = tx * dpTan2 + nx * m2;
            b2Velocity.y = ty * dpTan2 + ny * m2;

            b1.SetVelocity(b1Velocity);
            b2.SetVelocity(b2Velocity);

            float collisionAngle = CalculateCollisionAngle(b1, b2);

            b1.transform.rotation = CalculateRotationFromCollision(b1, b2, collisionAngle);
            b2.transform.rotation = CalculateRotationFromCollision(b2, b1, collisionAngle);

            // Calculate torque change based on collision (adjust this calculation as needed)
            float torqueChange = CalculateTorqueFromCollision(b1, b2);

            b1.SetTorque(torqueChange);
            b2.SetTorque(torqueChange);

            AudioManager.Instance.PlayAudio(ballSettings.BallBounceAudioId);
        }
    }

    private bool CirclesOverlap(Vector2 center1, float radius1, Vector2 center2, float radius2)
    {
        float distance = (center1 - center2).sqrMagnitude;
        float combinedRadii = (radius1 + radius2) * (radius1 + radius2);

        return distance <= combinedRadii;
    }

    private float CalculateCollisionAngle(Ball b1, Ball b2)
    {
        Vector2 collisionDirection = b2.transform.position - b1.transform.position;

        // Calculate the collision angle (between 0 and 2π) using Mathf.Atan2
        float collisionAngle = Mathf.Atan2(collisionDirection.y, collisionDirection.x);

        // Convert the angle to a positive value in radians (between 0 and 2π)
        if (collisionAngle < 0)
            collisionAngle = 2 * Mathf.PI + collisionAngle;

        return collisionAngle;
    }

    private Quaternion CalculateRotationFromCollision(Ball b1, Ball b2, float collisionAngle)
    {
        // Convert collision angle to degrees
        float rotationChange = collisionAngle * Mathf.Rad2Deg;

        // Update the rotation of the ball based on collision
        Quaternion newRotation = b1.transform.rotation * Quaternion.Euler(0f, 0f, rotationChange);

        return newRotation;
    }

    private float CalculateTorqueFromCollision(Ball b1, Ball b2)
    {
        // Calculate the vector from the center of the ball to the collision point
        Vector2 collisionVector = b2.transform.position - b1.transform.position;

        // Calculate the collision angle relative to the direction of the ball's movement
        float collisionAngle = Mathf.Atan2(collisionVector.y, collisionVector.x);

        // Calculate the perpendicular distance from the center to the collision point
        float distanceToCollision = collisionVector.magnitude;
        float perpendicularDistance = distanceToCollision * Mathf.Sin(collisionAngle);

        // Calculate the force exerted during the collision (you may need additional data or equations here)
        // For example, using collision forces, masses, velocities, or other relevant data
        float collisionForce = CalculateCollisionForce(b1, b2);

        // Calculate the torque change based on collision force and perpendicular distance
        float torqueChange = collisionForce * perpendicularDistance;

        return torqueChange;
    }

    private float CalculateCollisionForce(Ball b1, Ball b2)
    {
        Vector2 relativeVelocity = b2.RB.velocity - b1.RB.velocity;
        float relativeSpeed = relativeVelocity.magnitude;
        float totalMass = b1.Mass + b2.Mass;

        // Calculate the collision force using a simplified formula (adjust as needed)
        float collisionForce = 0.5f * totalMass * relativeSpeed;

        return collisionForce;
    }

    private void ClearBalls()
    {
        foreach (Ball ball in balls)
            Destroy(ball.gameObject);

        balls.Clear();
    }
}
