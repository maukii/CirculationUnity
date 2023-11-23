using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action OnFirstInputReceived;
    
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    
    private Vector2 inputDirection = Vector2.zero;
    private bool firstInputReceived = false;


    private void Update()
    {       
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        inputDirection.Set(HorizontalInput, VerticalInput);
        Vector2.ClampMagnitude(inputDirection, 1.0f);

        if (!firstInputReceived)
            CheckFirstInput();
    }

    public Vector2 GetInput() => inputDirection;

    private void CheckFirstInput()
    {
        if (GetInput() != Vector2.zero)
        {
            firstInputReceived = true;
            OnFirstInputReceived?.Invoke();
        }
    }
}