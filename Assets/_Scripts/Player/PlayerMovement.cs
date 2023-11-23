using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerSettings;
    
    private bool canMove = false;


    public void Move(Vector2 inputDirection)
    {
        if (canMove)
        {
            Vector3 forwardMovement = transform.up * inputDirection.y * playerSettings.ForwardSpeed;
            transform.position += forwardMovement * Time.deltaTime;
        }

        float rotationChange = -inputDirection.x * playerSettings.RotationSpeed * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(0, 0, rotationChange);
    }

    public void EnableMovement() => canMove = true;

    public void DisableMovement() => canMove = false;
}
