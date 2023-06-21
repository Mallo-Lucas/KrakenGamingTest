using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected bool _canHurt;
    protected bool _canMove;
    protected ObstaclesSpawnSystem _obstaclesSpawnSystem;

    public void SetObstacleCanMove(bool canMove) => _canMove = canMove;
    public virtual void DestroyObstacle()
    {
        Destroy(gameObject);
    }

    public virtual void DestroyObstacleWhitScore()
    {
        
    }

    public virtual void HurtPlayer(PlayerModel player)
    {
        if (_canHurt)
            return;
        _canHurt = true;
    }

    public virtual void Initialize(Vector3 dir, ObstaclesSpawnSystem obstaclesSpawnSystem)
    {
        _obstaclesSpawnSystem = obstaclesSpawnSystem;
    }

    public virtual void FlipDirection()
    {

    }

    public virtual IEnumerator CastPlayerEvadeArea()
    {
        yield return null;
    }
}
