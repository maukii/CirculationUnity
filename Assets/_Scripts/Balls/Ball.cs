using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private BallSettings ballSettings;
    [SerializeField] private BoundsData worldBounds;

    public Rigidbody2D RB { get; private set; }
    public float Radius { get; private set; }
    public float Mass { get; private set; }

    private AnimateOpen creationAnimation;
    private float targetRadius = 0f;


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        creationAnimation = GetComponent<AnimateOpen>();
    }

    private void Start()
    {
        creationAnimation.SetDuration(Random.Range(ballSettings.BallMinCreationTime, ballSettings.BallMaxCreationTime));
        creationAnimation.SetTargetScale(Vector3.one * targetRadius * 2f);
        creationAnimation.SetUpdateCallback(UpdateRadius);
        creationAnimation.SetCallback(() => AudioManager.Instance.PlayAudio(ballSettings.BallCreatedAudioId));
        creationAnimation.Animate();
    }

    private void Update() => CheckBounds();

    public Ball SetVelocity(Vector2 velocity)
    {
        RB.velocity = velocity;
        return this;
    }

    public Ball SetTorque(float torque)
    {
        RB.AddTorque(torque);
        return this;
    }

    public Ball SetRadius(float radius)
    {
        //transform.localScale = Vector3.one * Radius * 2f;
        targetRadius = radius;
        return this;
    }

    public void UpdateRadius(float radius)
    {
        Radius = targetRadius * radius;
    }

    public Ball SetMass(float mass)
    {
        this.Mass = mass;
        RB.mass = mass;
        return this;
    }

    public void CollidedWithPlayer() => BallCollisionHandler.Instance.RemoveBallFromCollisionHandler(this);

    private void CheckBounds()
    {
        Vector2 position = transform.position;
        Vector2 velocity = RB.velocity;

        if (position.x - Radius < worldBounds.bounds.min.x)
        {
            position.x = worldBounds.bounds.min.x + Radius;
            velocity.x = -velocity.x;
        }

        if (position.x + Radius >= worldBounds.bounds.max.x)
        {
            position.x = worldBounds.bounds.max.x - Radius;
            velocity.x = -velocity.x;
        }

        if (position.y - Radius < worldBounds.bounds.min.y)
        {
            position.y = worldBounds.bounds.min.y + Radius;
            velocity.y = -velocity.y;
        }

        if (position.y + Radius >= worldBounds.bounds.max.y)
        {
            position.y = worldBounds.bounds.max.y - Radius;
            velocity.y = -velocity.y;
        }

        transform.position = position;
        SetVelocity(velocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}