using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 MoveInput;
    private bool JumpInput;
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float JumpHeight = 1f;
    [SerializeField] private float GravityScale = 5f;
    [SerializeField] private float TurnSpeed = 120f;

    [Header("Component References")]
    [SerializeField] CharacterController CharacterController;
    [SerializeField] Animator Animator;

    private float VerticalVelocity = 0f;

    #region Input Handling Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
        }

    }

    public void onAttack(InputAction.CallbackContext context)
    {

    }

    #endregion

    #region Unity Callbacks

    private void Update()
    {
        UpdateMovement();

        UpdateAnimator();
    }

    #endregion

    #region Character Control Methods

    float GetAngleFromVector(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        return rotation.eulerAngles.y;
    }

    void UpdatePlayerRotation(Vector3 moveInput)
    {
        if (moveInput.sqrMagnitude < 0.01f)
            return; // Don't rotate if there's no significant movement)

        Vector3 playerRotation = transform.rotation.eulerAngles;

        playerRotation.y = GetAngleFromVector(moveInput);

        // transform.rotation = Quaternion.Euler(playerRotation);

        Quaternion targetRotation = Quaternion.Euler(playerRotation);

        float maxDegreesDelta = TurnSpeed * Time.deltaTime;

        // Apply the smoothed rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);
    }

    void UpdateMovement()
    {

        Vector3 moveInput3D = new Vector3(MoveInput.x, 0f, MoveInput.y);

        Vector3 motion = moveInput3D * Speed * Time.deltaTime;

        UpdatePlayerRotation(moveInput3D);

        if (CharacterController.isGrounded)
        {
            // Small downward force to keep controller grounded
            VerticalVelocity = -3f;
        }
        else
        {
            // Apply gravity scaled when in air
            VerticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
        }

        // Jumping
        if (JumpInput && CharacterController.isGrounded)
        {
            VerticalVelocity = Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(Physics.gravity.y * GravityScale));
            JumpInput = false; // Reset jump input after processing
        }

        motion.y = VerticalVelocity * Time.deltaTime;
        CharacterController.Move(motion);
    }



    #endregion

    #region Other Methods 

    void UpdateAnimator()
    {

        // Calculate horizontal speed from CharacterController velocity
        Vector3 horizontalVelocity = CharacterController.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;

        // Determine jump/fall states
        bool jump = false;
        bool fall = false;

        if (CharacterController.isGrounded)
        {
            jump = false;
            fall = false;
        }
        else
        {
            if (VerticalVelocity >= 0f)
            {
                jump = true;
                fall = false;
            }
            else
            {
                jump = false;
                fall = true;
            }
        }

        // Update animator every frame so movement blendspace works when grounded
        if (Animator != null)
        {
            Animator.SetFloat("Speed", speed);
            Animator.SetBool("Jump", jump);
            Animator.SetBool("Fall", fall);
        }

    #endregion

        


        
    }
}
