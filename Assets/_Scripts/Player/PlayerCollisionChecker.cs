using UnityEngine;

public class PlayerCollisionChecker : MonoBehaviour
{
    private PlayerHealth health;


    private void Awake() => health = GetComponent<PlayerHealth>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            health.TakeDamage();

            Ball ball = collision.gameObject.GetComponent<Ball>();
            if(ball != null) 
                ball.CollidedWithPlayer();
        }
    }
}
