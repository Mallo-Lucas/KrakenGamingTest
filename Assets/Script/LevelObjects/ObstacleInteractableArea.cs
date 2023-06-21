using KrakenGamingTest.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInteractableArea : MonoBehaviour
{
    private enum ObstacleActions {FallFromFloor, FallFromStairs }

    [SerializeField] private ObstacleActions obstacleAction;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out NormalBarrel barrel))
        {
            switch(obstacleAction)
            {
                case ObstacleActions.FallFromFloor:
                    barrel.SetAirSpeed();
                    break;
                case ObstacleActions.FallFromStairs:
                    barrel.SetBarrelCanFallFromStairs(true);
                    break;
            }
        }


        if (other.TryGetComponent(out NormalShuriken shuriken))
        {
            if(obstacleAction == ObstacleActions.FallFromFloor)
                shuriken.FlipDirection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NormalBarrel barrel))
        {
            switch (obstacleAction)
            {
                case ObstacleActions.FallFromStairs:
                    barrel.SetBarrelCanFallFromStairs(false);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
