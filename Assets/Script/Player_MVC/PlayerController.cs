using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KrakenGamingTest.Player
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<Vector3> OnMove;
        public event Action<bool> OnCrouch;
        public event Action OnJump;
        public event Action OnAbility;

        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private PlayerInput playerInput;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _crouchAction;
        private InputAction _abilityAction;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            playerModel.GetSubscriptionEvents(this);
            PlayerInputGetActions();
        }

        private void PlayerInputGetActions()
        {
            var inputActions = playerInput.actions;
            _moveAction = inputActions["Move"];
            _jumpAction = inputActions["Jump"];
            _crouchAction = inputActions["Crouch"];
            _abilityAction = inputActions["Ability"];

            SubscribeActions();
        }

        private void SubscribeActions()
        {
            _moveAction.performed += MoveAction;
            _jumpAction.performed += JumpAction;
            _crouchAction.performed += CrouchAction;
            _abilityAction.performed += AbilityAction;
        }

        private void MoveAction(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(context.ReadValue<Vector2>());
        }

        private void JumpAction(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }

        private void CrouchAction(InputAction.CallbackContext context)
        {
            OnCrouch?.Invoke(context.ReadValue<float>() != 0);
        }
        private void AbilityAction(InputAction.CallbackContext context)
        {
            OnAbility?.Invoke();
        }
    }
}
