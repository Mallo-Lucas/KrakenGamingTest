using KrakenGamingTest.Player;
using KrakenGamingTest.ScriptableObjects.Obstacles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionObstacle : Obstacle, I_Pause
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private OnDirectionObstacleData data;
    [SerializeField] private Transform evadeArea;

    private Collider[] _playerEvadeAreaHit = new Collider[1];
    private float _movementSpeed;

    private Action PlayerEvade;

    private void FixedUpdate()
    {
        Move();
    }

    public override void Initialize(Vector3 dir, ObstaclesSpawnSystem obstaclesSpawnSystem)
    {
        base.Initialize(dir, obstaclesSpawnSystem);
        _movementSpeed = data.speed;
        _canMove = true;
        PlayerEvade += LevelEventsHandler.Instance.GetScoreManagerObstacleEvadeEvent();
        StartCoroutine(CastPlayerEvadeArea());
        SubscribeToPause(this);
    }

    private void Move()
    {
        if (!_canMove)
            return;
        rb.MovePosition(transform.position + transform.forward * _movementSpeed * Time.deltaTime);
    }

    public override IEnumerator CastPlayerEvadeArea()
    {
        while (true)
        {
            var playerCount = Physics.OverlapBoxNonAlloc(evadeArea.position, evadeArea.localScale / 2, _playerEvadeAreaHit, evadeArea.rotation, data.playerLayerMask);
            if (playerCount > 0)
            {
                PlayerEvade?.Invoke();
                break;
            }
            yield return null;
        }
    }

    public override void DestroyObstacle()
    {
        _obstaclesSpawnSystem.RemoveObstacle(this);
        base.DestroyObstacle();
        PlayerEvade = null;
    }

    public override void DestroyObstacleWhitScore()
    {
        for (int i = 0; i < 2; i++)
            PlayerEvade?.Invoke();
        DestroyObstacle();
    }

    public void Pause(bool state)
    {
        _canMove = !state;
    }

    public override void HurtPlayer(PlayerModel player)
    {
        base.HurtPlayer(player);
        player.PlayerGetDamage();
        PlayerEvade = null;
        rb.isKinematic = true;
    }

    public void SubscribeToPause(I_Pause ipause)
    {
        LevelEventsHandler.Instance.SubscribeToPauseMenu(ipause);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameStaticFunctions.IsGoInLayerMask(collision.gameObject, data.collisionLayerMask))
            return;

        if (collision.gameObject.TryGetComponent(out PlayerModel player))
        {
            _canMove = false;
            HurtPlayer(player);
            return;
        }

        DestroyObstacle();
    }
}
