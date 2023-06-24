using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SpawnObstaclesData", fileName = "SpawnObstaclesData", order = 0)]
public class SpawnObstaclesData : ScriptableObject
{
    public float maxTimeToSpawnObstacle;
    public float minTimeToSpawnObstacle;
    public List<Obstacle> obstacles;
    public List<float> chances;
    private RouletteWheel<Obstacle> _obstaclesWheel;

    public Obstacle GetRandomObstacleFromPool()
    {
        _obstaclesWheel ??= new RouletteWheel<Obstacle>(obstacles, chances);

        return _obstaclesWheel.RunWithCached();
    }
}
