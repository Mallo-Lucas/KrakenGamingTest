using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : ScriptableObject
{
    public float abilityStack;
    public Sprite abilityIcon;

    public virtual void UseAbility(PlayerModel player)
    {
        
    }

    public virtual void AbilityAdded(PlayerModel player)
    {
        player.AddAbility(this);
    }

    public virtual void AbilityRemove(PlayerModel player)
    {

    }
}
