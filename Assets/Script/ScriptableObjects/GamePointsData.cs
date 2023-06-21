using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GamePointsData", fileName = "GamePointsData", order = 0)]
public class GamePointsData : ScriptableObject
{
    public int bonusScore;
    public int pointToSubstractPerSecond;
    public int jumpBarrelPoint;
    public int evadeShurikenPoint;
    public float barrelsJumpedConsecutivelyTimer;
}
