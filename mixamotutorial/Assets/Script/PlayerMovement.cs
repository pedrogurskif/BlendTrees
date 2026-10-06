using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    public float runSpeed = 9;
    public float animationSmoothTime = 0.1f;
    public InputActionAsset inputActions;
    private InputAction runAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    public Animator animator;

    Vector2 currentAnimationBlendVector;
    Vector2 animationSpeed;

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
        Vector2 input = moveAction.ReadValue<Vector2>() * targetMovingSpeed;
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationSpeed, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0f, currentAnimationBlendVector.y);
        rigidbody.linearVelocity = transform.rotation * move;

        animator.SetFloat("SpeedSides", move.x);
        animator.SetFloat("SpeedFront", move.z);
    }
}