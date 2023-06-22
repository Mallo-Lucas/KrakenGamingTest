using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Obstacles/ShurikenData", fileName = "ShurikenData", order = 0)]
public class ShurikenData : ScriptableObject
{
    public float speed;
    public float rotationSpeed;
    public float goDownSpeed;
    public float timeOnFlip;
    [Range(0, 100)] public float chancesToGoDown;
    public LayerMask playerLayerMask;
    public LayerMask collisionLayerMask;
}
