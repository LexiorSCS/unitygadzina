using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLayer : MonoBehaviour
{
    private Vector2 MoveInput;
    [SerializeField] private float Speed = 10f;

    [SerializeField] CharacterController CharacterController;

    private float VerticalVelocity = 0f;

    #region Input Handling Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

    }

    public void onAttack(InputAction.CallbackContext context)
    {
        
    }

    #endregion

    #region Unity Callbacks

    private void Update()
    {
        Vector3 moveInput3D = new Vector3 (MoveInput.x, 0f, MoveInput.y);

        Vector3 motion = moveInput3D * Speed * Time.deltaTime;

        VerticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (CharacterController.isGrounded)
        {
            VerticalVelocity = -3f;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        motion.y = VerticalVelocity * Time.deltaTime;
        CharacterController.Move(motion);
    }

    #endregion
    
}
