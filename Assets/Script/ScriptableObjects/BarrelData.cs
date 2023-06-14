using UnityEngine;

namespace KrakenGamingTest.ScriptableObjects.Obstacles
{
    [CreateAssetMenu(menuName = "ScriptableObject/Obstacles", fileName = "BarrelData", order = 0)]
    public class BarrelData : ScriptableObject
    {
        public float speedOnGround;
        public float speedOnAir;
        [Range(0,100)]public float flipChance;
        [Range(0,100)]public float fallFromStairsChance;
        public LayerMask bounceLayerMask;
    }
}
