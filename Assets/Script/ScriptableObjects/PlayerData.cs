using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakenGamingTest.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Player Stats")]
        public float playerSpeed;
        public float playerJumpForce;
    }
}

