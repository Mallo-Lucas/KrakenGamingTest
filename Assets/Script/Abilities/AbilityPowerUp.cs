using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPowerUp : MonoBehaviour
{
    [SerializeField] private PlayerAbility powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerModel player))
        {
            player.AddAbility(powerUp);
            Destroy(gameObject);
        }
    }
}
