using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected bool _canHurt;

    public virtual void DestroyObstacle()
    {
        Destroy(gameObject);
    }

    public virtual void HurtPlayer(PlayerModel player)
    {
        if (_canHurt)
            return;
        _canHurt = true;
    }
}
