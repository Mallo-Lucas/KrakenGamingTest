using KrakenGamingTest.ScriptableObjects.Player;
using UnityEngine;

namespace KrakenGamingTest.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Rigidbody rb;

        private Vector3 _movementDirection;
        private float _movementSpeed;
        private bool _canMove;
        private bool _canJump;
        private bool _canClimb;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _canMove = true;
            _canJump = true;
            _movementSpeed = playerData.playerSpeedOnGround;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!_canMove)
                return;

            if(!_canClimb)
                _movementDirection.y = 0;

            rb.MovePosition(transform.position + _movementDirection * _movementSpeed * Time.deltaTime);
        }

        private void Jump()
        {
            if (!_canJump || _canClimb)
                return;
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * playerData.playerJumpForce, ForceMode.Impulse);
            _canJump = false;
            _movementSpeed = playerData.playerSpeedOnAir;
        }

        private void SetPlayerMovementDirection(Vector3 direction)
        {
            _movementDirection = direction;
        }

        public void GetSubscriptionEvents(PlayerController controller)
        {
            controller.OnMove += SetPlayerMovementDirection;
            controller.OnJump += Jump;
        }

        public void SetPhysicsToClimb(bool state)
        {
            _canClimb = state;
            rb.velocity = Vector3.zero;
            if (state)
            {
                rb.useGravity = false;
                _movementSpeed = playerData.playerSpeedOnGround;
                return;
            }
            rb.useGravity = true;
            _movementSpeed = playerData.playerSpeedOnAir;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _canJump = true;
            _movementSpeed = playerData.playerSpeedOnGround;
        }
    }
}
