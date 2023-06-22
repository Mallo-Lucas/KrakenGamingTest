using KrakenGamingTest.Player;
using KrakenGamingTest.ScriptableObjects.Obstacles;
using System;
using System.Collections;
using UnityEngine;

namespace KrakenGamingTest.Obstacles
{
    public class NormalBarrel : Obstacle, I_Pause
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private BarrelData barrelData;
        [SerializeField] private Transform playerJumpArea;
        [SerializeField] private Transform visual;
      
        private Collider[] playerJumpAreaHit = new Collider[1];
        private Vector3 _direction;
        private float _movementSpeed;
        private float _rotationSpeed;
        private bool _canFallFromStairs;
        private bool _onAir;

        private Action OnPlayerJumpOver;

        private void FixedUpdate()
        {
            Move();
        }

        public override void Initialize(Vector3 dir, ObstaclesSpawnSystem obstaclesSpawnSystem)
        {
            base.Initialize(dir, obstaclesSpawnSystem);
            _direction = dir;
            _movementSpeed = barrelData.speedOnGround;
            _canMove = true;
            _rotationSpeed = barrelData.rotationSpeed;
            OnPlayerJumpOver += LevelEventsHandler.Instance.GetScoreManagerBarrelEvent();
            StartCoroutine(CastPlayerEvadeArea());
            SubscribeToPause(this);
        }

        private void Move()
        {
            if (!_canMove)
                return;
            visual.transform.Rotate(new Vector3(0, _rotationSpeed, 0));
            if (_canFallFromStairs)
            {
                rb.MovePosition(transform.position + Vector3.down * _movementSpeed * Time.deltaTime);
                return;
            }

            rb.MovePosition(transform.position + _direction * _movementSpeed * Time.deltaTime);
        }


        public override void FlipDirection()
        {
            _direction.x *= -1;
            _rotationSpeed *= -1;
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
            _onAir = true;
            _movementSpeed = barrelData.speedOnAir;
        }

        public override void DestroyObstacle()
        {
            _obstaclesSpawnSystem.RemoveObstacle(this);
            base.DestroyObstacle();
            OnPlayerJumpOver = null;
        }

        public override void DestroyObstacleWhitScore()
        {
            for (int i = 0; i < 2; i++)
                OnPlayerJumpOver?.Invoke();
            DestroyObstacle();
        }

        public override IEnumerator CastPlayerEvadeArea()
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
            OnPlayerJumpOver = null;
            rb.isKinematic = true;
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

            if (!_onAir)
                return;
            _onAir = false;
            _movementSpeed = barrelData.speedOnGround;
            var flipChance = UnityEngine.Random.Range(0, 101);
            if (flipChance <= barrelData.flipChance)
                FlipDirection();
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
