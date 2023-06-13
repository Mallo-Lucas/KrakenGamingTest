using KrakenGamingTest.ScriptableObjects.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakenGamingTest.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Rigidbody rb;

        private float _movementDirection;
        private bool _canJump;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            rb.MovePosition(transform.position + transform.forward * _movementDirection * playerData.playerSpeed * Time.deltaTime);
        }

        private void Jump()
        {
            if (_canJump)
                return;
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * playerData.playerJumpForce, ForceMode.Impulse);
            _canJump = true;
        }

        private void SetPlayerMovementDirection(float direction)
        {
            _movementDirection = direction;
        }

        public void GetSubscriptionEvents(PlayerController controller)
        {
            controller.OnMove += SetPlayerMovementDirection;
            controller.OnJump += Jump;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _canJump = false;
        }
    }
}
