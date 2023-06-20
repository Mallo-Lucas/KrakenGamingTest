using KrakenGamingTest.Player;
using KrakenGamingTest.ScriptableObjects.Obstacles;
using System;
using System.Collections;
using UnityEngine;

namespace KrakenGamingTest.Obstacles
{
    public class NormalBarrel : Obstacle
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private BarrelData barrelData;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private Transform playerJumpArea;

        private Collider[] playerJumpAreaHit = new Collider[1];
        private Vector3 _direction;
        private float _movementSpeed;
        private bool _canMove;
        private bool _canFallFromStairs;

        private Action OnPlayerJumpOver;

        public void SetBarrelCanMove(bool canMove) => _canMove = canMove;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Initialize()
        {
            _direction = transform.right;
            _movementSpeed = barrelData.speedOnGround;
            _canMove = true;
            OnPlayerJumpOver += scoreManager.BarrelJumped;
            StartCoroutine(CastPlayerJumpArea());
        }

        private void Move()
        {
            if (!_canMove)
                return;
            if (_canFallFromStairs)
            {
                rb.MovePosition(transform.position + Vector3.down * _movementSpeed * Time.deltaTime);
                return;
            }

            rb.MovePosition(transform.position + _direction * _movementSpeed * Time.deltaTime);
        }


        public void FlipDirection()
        {
            _direction.x *= -1;
        }

        public void SetBarrelCanFallFromStairs(bool canFallFromStairs)
        {
            if (_canFallFromStairs == canFallFromStairs)
                return;
                           
            if (canFallFromStairs)
            {             
                var fallFromStairsChance = UnityEngine.Random.Range(0, 101);
                if (fallFromStairsChance > barrelData.fallFromStairsChance)
                    return;
                rb.velocity = Vector3.zero;              
                rb.useGravity = false;
                _movementSpeed = barrelData.speedOnAir;
                _canFallFromStairs = canFallFromStairs;
                return;
            }
            rb.velocity = Vector3.zero;
            rb.useGravity = true;            
            _movementSpeed = barrelData.speedOnGround;
            _canFallFromStairs = canFallFromStairs;
            FlipDirection();
        }

        public void SetAirSpeed()
        {
            _movementSpeed = barrelData.speedOnAir;
        }

        private IEnumerator CastPlayerJumpArea()
        {
            while(true)
            {
                var playerCount = Physics.OverlapBoxNonAlloc(playerJumpArea.transform.position, playerJumpArea.localScale / 2, playerJumpAreaHit, playerJumpArea.rotation, barrelData.playerLayerMask);
                if (playerCount > 0)
                {
                    OnPlayerJumpOver?.Invoke();
                    break;
                }
                yield return null;
            }
        }

        public override void HurtPlayer(PlayerModel player)
        {
            base.HurtPlayer(player);
            player.PlayerGetDamage();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!GameStaticFunctions.IsGoInLayerMask(collision.gameObject, barrelData.collisionLayerMask))
                return;

            if(collision.gameObject.TryGetComponent(out PlayerModel player))
            {
                _canMove = false;
                HurtPlayer(player);
                return;
            }

            _movementSpeed = barrelData.speedOnGround;
            var flipChance = UnityEngine.Random.Range(0, 101);
            if (flipChance <= barrelData.flipChance)
                FlipDirection();
        }
    }
}
