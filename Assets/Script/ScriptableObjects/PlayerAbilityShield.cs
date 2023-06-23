using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ability/PlayerAbilityShield", fileName = "PlayerAbilityShield", order = 0)]
public class PlayerAbilityShield : PlayerAbility
{
    [SerializeField] private PowerUpShieldOnPlayer abilityShield;
    [SerializeField] private float shieldUpTime;
    public override void UseAbility(PlayerModel player)
    {        
        var shield = Instantiate(abilityShield);
        shield.transform.position = player.transform.position;
        shield.transform.parent = player.transform;
        shield.Initialize(shieldUpTime);
    }

    public override void AbilityRemove(PlayerModel player)
    {

    }

    public override void AbilityAdded(PlayerModel player)
    {

    }
}
