using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ability/PlayerAbilitySwordAttack", fileName = "PlayerAbilitySwordAttack", order = 0)]
public class PlayerAbilitySwordAttack : PlayerAbility
{
    public override void UseAbility(PlayerModel player)
    {
        player.AttacSword();
    }

    public override void AbilityRemove(PlayerModel player)
    {
        player.EnableSword(false);
    }

    public override void AbilityAdded(PlayerModel player)
    {
        player.EnableSword(true);
    }
}
