using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Obstacles/OnDirectionObstacleData", fileName = "OnDirectionObstacleData", order = 0)]
public class OnDirectionObstacleData : ScriptableObject
{
    public float speed;
    public LayerMask collisionLayerMask;
    public LayerMask playerLayerMask;
}
