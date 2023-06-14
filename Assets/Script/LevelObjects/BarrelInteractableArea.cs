using KrakenGamingTest.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelInteractableArea : MonoBehaviour
{
    private enum BarrelActions {BarrellFallFromFloor, BarrelFallFromStairs }

    [SerializeField] private BarrelActions barrelAction;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out NormalBarrel barrel))
        {
            switch(barrelAction)
            {
                case BarrelActions.BarrellFallFromFloor:
                    barrel.SetAirSpeed();
                    break;
                case BarrelActions.BarrelFallFromStairs:
                    barrel.SetBarrelCanFallFromStairs(true);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NormalBarrel barrel))
        {
            switch (barrelAction)
            {
                case BarrelActions.BarrelFallFromStairs:
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
