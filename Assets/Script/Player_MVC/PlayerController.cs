using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KrakenGamingTest.Player
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<Vector3> OnMove;
        public event Action OnJump;

        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private PlayerInput playerInput;

        private InputAction _moveAction;
        private InputAction _jumpAction;

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

            SubscribeActions();
        }

        private void SubscribeActions()
        {
            _moveAction.performed += MoveAction;
            _jumpAction.performed += JumpAction;
        }

        private void MoveAction(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(context.ReadValue<Vector2>());
        }

        private void JumpAction(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }
    }
}
