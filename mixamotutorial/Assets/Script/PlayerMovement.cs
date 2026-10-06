using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    public float runSpeed = 9;
    public InputActionAsset inputActions;
    private InputAction runAction;
    private InputAction moveAction;

    Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Sprint");
    }

    void FixedUpdate()
    {
        float targetMovingSpeed = runAction.IsPressed() ? runSpeed : speed;

        Vector2 targetVelocity = moveAction.ReadValue<Vector2>() * targetMovingSpeed;
        rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);
    }
}