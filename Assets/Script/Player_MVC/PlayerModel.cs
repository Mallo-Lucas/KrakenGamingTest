using KrakenGamingTest.ScriptableObjects.Player;
using System;
using System.Collections;
using UnityEngine;

namespace KrakenGamingTest.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider playerCollider;
        [SerializeField] private PlayerView playerView;
        [SerializeField] private InGameUIManager inGameUIManager;

        private Vector3 _movementDirection;
        private float _movementSpeed;
        private bool _canMove;
        private bool _onGround;
        private bool _canClimb;
        private int _playerLifes;

        public PlayerData GetPlayerData() => playerData;
        public void SetPlayerCanMove(bool value) => _canMove = value;

        public event Action<Vector3> OnMove;
        public event Action<bool> OnClimb;
        public event Action<bool> OnGround;
        public event Action<int> OnGetDamage;
        public event Action OnJump;
        public event Action OnRespawn;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _canMove = false;
            _onGround = true;
            _movementSpeed = playerData.playerSpeedOnGround;
            _playerLifes = playerData.playerLifes;
            inGameUIManager.FadeOutScreenEnd += delegate { SetPlayerCanMove(true); };
            playerView.GetSubscriptionsEvents(this);
            StartCoroutine(CheckIfPlayerIsOnGround());
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!_canMove)
                return;

            if (!_canClimb)
                _movementDirection.y = 0;

            rb.MovePosition(transform.position + _movementDirection * _movementSpeed * Time.deltaTime);
            OnMove?.Invoke(_movementDirection);       
        }

        private void Jump()
        {
            if (!_onGround || _canClimb || !_canMove)
                return;
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * playerData.playerJumpForce, ForceMode.Impulse);
            _onGround = false;
            _movementSpeed = playerData.playerSpeedOnAir;
            OnJump?.Invoke();
        }

        private void SetPlayerMovementDirection(Vector3 direction)
        {
            _movementDirection = direction;
            if (_movementDirection.y != 0 && _canClimb)
                SetPhysicsToClimb();
        }

        public void GetSubscriptionEvents(PlayerController controller)
        {
            controller.OnMove += SetPlayerMovementDirection;
            controller.OnJump += Jump;
        }

        public void SetCanClimb(bool state)
        {
            _canClimb = state;
            if (!_canClimb)
                SetPhysicsToClimb();
        }

        private void SetPhysicsToClimb()
        {
            OnClimb?.Invoke(_canClimb);
            rb.velocity = Vector3.zero;
            if (_canClimb)
            {
                rb.useGravity = false;
                return;
            }
            rb.useGravity = true;
        }

        private IEnumerator CheckIfPlayerIsOnGround()
        {
            while (true)
            {
                OnGround?.Invoke(_onGround);
                if (Physics.Raycast(transform.position - transform.up, Vector3.down, 0.2f, playerData.goundLayers))
                {
                    _onGround = true;
                    _movementSpeed = playerData.playerSpeedOnGround;
                    yield return null;
                    continue;
                }

                _onGround = false;
                _movementSpeed = playerData.playerSpeedOnAir;
                yield return null;
            }
        }

        public void PlayerGetDamage()
        {
            _playerLifes--;         
            OnGetDamage?.Invoke(_playerLifes);
            playerCollider.enabled = false;           
        }

        public void RespawnPlayer(Transform spawnPoint)
        {
            playerCollider.enabled = true;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            _canMove = false;
            OnRespawn?.Invoke();
        }
    }
}
