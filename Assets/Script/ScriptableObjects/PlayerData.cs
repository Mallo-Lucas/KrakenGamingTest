using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakenGamingTest.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Player Stats")]
        public int playerLifes;
        public float playerSpeedOnGround;
        public float playerSpeedOnAir;
        public float playerJumpForce;
        public float playerDeathForce;

        [Header("Layers")]
        public LayerMask goundLayers;
    }
}

