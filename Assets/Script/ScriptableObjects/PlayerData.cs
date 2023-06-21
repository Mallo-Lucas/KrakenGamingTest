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
        public LayerMask obstaclesLayer;

        [Header("Collider Changes")]
        public Vector3 sizeOnCrouch;
        public Vector3 centerOnCrouch;
        public Vector3 sizeOnStand;
        public Vector3 centerOnStand;
    }
}

