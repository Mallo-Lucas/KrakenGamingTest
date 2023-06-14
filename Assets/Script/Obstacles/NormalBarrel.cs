using KrakenGamingTest.ScriptableObjects.Obstacles;
using UnityEngine;

namespace KrakenGamingTest.Obstacles
{
    public class NormalBarrel : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private BarrelData barrelData;

        private Vector3 _direction;
        private float _movementSpeed;
        private bool _canMove;
        private bool _canFallFromStairs;

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
                var fallFromStairsChance = Random.Range(0, 101);
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

        private void OnCollisionEnter(Collision collision)
        {
            if (!GameStaticFunctions.IsGoInLayerMask(collision.gameObject, barrelData.bounceLayerMask))
                return;

            _movementSpeed = barrelData.speedOnGround;
            var flipChance = Random.Range(0, 101);
            if (flipChance <= barrelData.flipChance)
                FlipDirection();
        }
    }
}
