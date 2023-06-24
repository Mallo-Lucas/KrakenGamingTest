using KrakenGamingTest.ScriptableObjects.Player;
using System;
using System.Collections;
using UnityEngine;

namespace KrakenGamingTest.Player
{
    public class PlayerModel : MonoBehaviour, I_Pause
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private BoxCollider playerCollider;
        [SerializeField] private PlayerView playerView;
        [SerializeField] private GameObject sword;
        [SerializeField] private Transform swordAttackArea;

        private PlayerAbility _playerAbility;

        private Collider[] playerSwordAreaHit = new Collider[3];
        private Vector3 _movementDirection;
        private float _movementSpeed;
        private bool _canMove;
        private bool _onGround;
        private bool _canClimb;
        private bool _onCrouch;
        private bool _onUseAbility;
        private int _playerLifes;
        private int _playerAbilityStack;

        public PlayerData GetPlayerData() => playerData;
        public void SetPlayerCanMove(bool value) => _canMove = value;

        public event Action<Vector3> OnMove;
        public event Action<bool> OnClimb;
        public event Action<bool> OnGround;
        public event Action<bool> OnCrouch;
        public event Action<int> OnGetDamage;
        public event Action<float> OnUseAbility;
        public event Action OnJump;
        public event Action OnRespawn;
        public event Action UseSword;
        public event Action<Sprite,float, bool> GetAbility;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _canMove = false;
            _onGround = true;
            _onCrouch = false;
            _movementSpeed = playerData.playerSpeedOnGround;
            _playerLifes = playerData.playerLifes;
            LevelEventsHandler.Instance.SubscribeToFadeOutEvent(delegate { SetPlayerCanMove(true); });
            LevelEventsHandler.Instance.PlayerWin += delegate { SetPlayerCanMove(false); };
            playerView.GetSubscriptionsEvents(this);
            StartCoroutine(CheckIfPlayerIsOnGround());
            SubscribeToPause(this);
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
            if (!_onGround || _canClimb || !_canMove || _onCrouch)
                return;
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * playerData.playerJumpForce, ForceMode.Impulse);
            _onGround = false;
            _movementSpeed = playerData.playerSpeedOnAir;
            OnJump?.Invoke();
        }

        private void Crouch(bool state)
        {
            if (!_onGround || !_canMove)
                return;
            OnCrouch?.Invoke(state);
            _onCrouch = state;
            if (state)
            {
                playerCollider.size = playerData.sizeOnCrouch;
                playerCollider.center = playerData.centerOnCrouch;
                return;
            }
            playerCollider.size = playerData.sizeOnStand;
            playerCollider.center = playerData.centerOnStand;
        }

        private void UseAbility()
        {
            if (_playerAbilityStack <= 0 || _onUseAbility || _playerAbility == null)
                return;
            _playerAbilityStack--;
            _playerAbility.UseAbility(this);
            OnUseAbility?.Invoke(_playerAbilityStack);
            if (_playerAbilityStack == 0)
            {
                _playerAbility.AbilityRemove(this);
                GetAbility?.Invoke(_playerAbility.abilityIcon, _playerAbility.abilityStack, false);
            }
        }

        public void AddAbility(PlayerAbility playerAbility)
        {
            EnableSword(false);
            _playerAbility = playerAbility;
            _playerAbilityStack = (int)_playerAbility.abilityStack;
            _playerAbility.AbilityAdded(this);
            GetAbility?.Invoke(_playerAbility.abilityIcon, _playerAbility.abilityStack, true);
        }

        public void EnableSword(bool state)
        {
            sword.gameObject.SetActive(state);
        }

        public void AttacSword()
        {
            StartCoroutine(AttackSwordCoroutine());
        }

        private IEnumerator AttackSwordCoroutine()
        {
            _onUseAbility = true;
            _canMove = false;
            UseSword?.Invoke();
            var timer = 0.8f;
            while (timer>0)
            {
                timer -= Time.deltaTime;
                var obstaclesCount = Physics.OverlapBoxNonAlloc(swordAttackArea.position, swordAttackArea.localScale / 2, playerSwordAreaHit, swordAttackArea.rotation, playerData.obstaclesLayer);
                for (int i = 0; i < obstaclesCount; i++)
                {
                    playerSwordAreaHit[i].GetComponent<Obstacle>().DestroyObstacleWhitScore();
                }
                yield return null;
            }
            _onUseAbility = false;
            _canMove = true;
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
            controller.OnCrouch += Crouch;
            controller.OnAbility += UseAbility;
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
            if (_playerAbility != null)
            {
                _playerAbility.AbilityRemove(this);
                GetAbility?.Invoke(_playerAbility.abilityIcon, _playerAbility.abilityStack, false);
                _playerAbility = null;
                _playerAbilityStack = 0;
            }       
            SetCanClimb(false);
            EnableSword(false);
            _canMove = false;
            _onGround = true;
            _onCrouch = false;
            rb.useGravity = true;
            _movementSpeed = playerData.playerSpeedOnGround;
            OnRespawn?.Invoke();
        }

        public void Pause(bool state)
        {
            _canMove = !state;
        }

        public void SubscribeToPause(I_Pause ipause)
        {
            LevelEventsHandler.Instance.SubscribeToPauseMenu(ipause);
        }
    }
}
