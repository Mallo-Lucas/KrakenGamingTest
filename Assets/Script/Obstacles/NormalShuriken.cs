using KrakenGamingTest.Player;
using KrakenGamingTest.ScriptableObjects.Obstacles;
using System;
using System.Collections;
using UnityEngine;

public class NormalShuriken : Obstacle, I_Pause
{
    [SerializeField] private ShurikenData shurikenData;
    [SerializeField] private Transform shurikenEvadeArea;
    [SerializeField] private Transform visual;

    private Collider[] playerEvadeAreaHit = new Collider[1];
    private Vector3 _direction;
    private float _movementSpeed;
    private float _rotationSpeed;
    private Action OnPlayerEvade;

    private void FixedUpdate()
    {
        Move();
    }

    public override void Initialize(Vector3 dir, ObstaclesSpawnSystem obstaclesSpawnSystem)
    {
        base.Initialize(dir, obstaclesSpawnSystem);
        _direction = dir;
        _movementSpeed = shurikenData.speed;
        _rotationSpeed = shurikenData.rotationSpeed;
        _canMove = true;
        OnPlayerEvade += LevelEventsHandler.Instance.GetScoreManagerShurikenEvent();
        StartCoroutine(CastPlayerEvadeArea());
        transform.position += Vector3.up*2.5f;
        SubscribeToPause(this);
    }

    private void Move()
    {
        if (!_canMove)
            return;
        transform.position += _direction * _movementSpeed * Time.deltaTime;
        visual.transform.Rotate(_rotationSpeed, 0, 0);
    }

    public override void HurtPlayer(PlayerModel player)
    {
        base.HurtPlayer(player);
        player.PlayerGetDamage();
        StopAllCoroutines();
        _canMove = false;
        OnPlayerEvade = null;
    }

    public override IEnumerator CastPlayerEvadeArea()
    {
        while (true)
        {
            var playerCount = Physics.OverlapBoxNonAlloc(shurikenEvadeArea.transform.position, shurikenEvadeArea.localScale / 2, playerEvadeAreaHit, shurikenEvadeArea.rotation, shurikenData.playerLayerMask);
            if (playerCount > 0)
            {
                OnPlayerEvade?.Invoke();
                break;
            }
            yield return null;
        }
    }

    public override void DestroyObstacle()
    {
        _obstaclesSpawnSystem.RemoveObstacle(this);
        base.DestroyObstacle();
        OnPlayerEvade = null;
    }

    public override void DestroyObstacleWhitScore()
    {
        for (int i = 0; i < 2; i++)
            OnPlayerEvade?.Invoke();
        DestroyObstacle();
    }

    public override void FlipDirection()
    {
        var r = UnityEngine.Random.Range(0, 101);
        if (r > shurikenData.chancesToGoDown)
            return;

        StartCoroutine(FlipShuriken());
    }

    private IEnumerator FlipShuriken()
    {
        var timer = shurikenData.timeOnFlip;
        float flipTimer = 0;
       
        while(timer > 0)
        {
            timer-= Time.deltaTime;
            flipTimer += Time.deltaTime;
            transform.position += Vector3.down * shurikenData.goDownSpeed * Time.deltaTime;
            yield return null;
        }

        _direction *= -1;
        _rotationSpeed *= -1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameStaticFunctions.IsGoInLayerMask(collision.gameObject, shurikenData.playerLayerMask))
            return;

        if (collision.gameObject.TryGetComponent(out PlayerModel player))
        {
            _canMove = false;
            HurtPlayer(player);
            return;
        }
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
