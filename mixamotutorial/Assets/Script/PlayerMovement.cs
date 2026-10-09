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
    public Transform cam;
    public bool grounded;
    public float jumpHeight;
    public Animator animator;
    public float animationPlayTransition = 0.15f; 
    public float rotationSpeed;

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
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        float targetMovingSpeed = runAction.IsPressed() ? runSpeed : speed;
        Vector2 input = moveAction.ReadValue<Vector2>() * targetMovingSpeed;
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationSpeed, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0f, currentAnimationBlendVector.y);
        Vector3 velocity = transform.rotation * move;

        velocity.y = rigidbody.linearVelocity.y; 
        rigidbody.linearVelocity = velocity;

        if(jumpAction.WasPressedThisFrame() && grounded) 
        { 
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y),
            rigidbody.linearVelocity.z); 
            grounded = false;
            animator.CrossFade("Jump", animationPlayTransition);
        }

        animator.SetFloat("SpeedSides", move.x);
        animator.SetFloat("SpeedFront", move.z);
        Quaternion targetRotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}