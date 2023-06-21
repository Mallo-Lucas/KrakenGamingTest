using UnityEngine;

namespace KrakenGamingTest.ScriptableObjects.Obstacles
{
    [CreateAssetMenu(menuName = "ScriptableObject/Obstacles/BarrelData", fileName = "BarrelData", order = 0)]
    public class BarrelData : ScriptableObject
    {
        public float speedOnGround;
        public float speedOnAir;
        public float rotationSpeed;
        [Range(0,100)]public float flipChance;
        [Range(0,100)]public float fallFromStairsChance;
        public LayerMask collisionLayerMask;
        public LayerMask playerLayerMask;
    }
}
