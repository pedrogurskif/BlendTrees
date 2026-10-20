using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerCC : MonoBehaviour
{
    private CharacterController cc;
    [SerializeField] private Camera mcam;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    private InputAction moveAction, runAction, jumpAction, lookAction;

    private float currentMoveSpeed, verticalSpeed;
    public float rotationSpeed;

    public Animator animator;
    public float animationPlayTransition = 0.15f; 
    public float animationSmoothTime = 0.1f;

    public bool isPlayerGrounded;

    Vector2 currentAnimationBlendVector;
    Vector2 animationSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentMoveSpeed = walkSpeed;
        cc = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, moveVector, ref animationSpeed, animationSmoothTime);
        currentMoveSpeed = runAction.IsPressed() ? runSpeed : walkSpeed;
        HandleMovement(currentAnimationBlendVector);
        Quaternion targetRotation = Quaternion.Euler(0f, mcam.transform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("SpeedSides", currentAnimationBlendVector.x*currentMoveSpeed);
        animator.SetFloat("SpeedFront", currentAnimationBlendVector.y*currentMoveSpeed);

        if(cc.isGrounded && !isPlayerGrounded)
        {
            animator.CrossFade("Local Motion", 0.15f);
        }
        isPlayerGrounded = cc.isGrounded;
    }

    private void HandleMovement(Vector2 vect)
    {
        Vector3 moveDirection = (transform.forward * vect.y + transform.right * vect.x) * currentMoveSpeed;

        if(cc.isGrounded)
        {
            if(jumpAction.WasPressedThisFrame())
            {
                animator.CrossFade("Jump", animationPlayTransition);
                verticalSpeed = jumpForce;
            }
        }
        else
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalSpeed;
        cc.Move(moveDirection * Time.deltaTime);
    }
}
