using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirectionObstacleSpawner : MonoBehaviour
{
    [SerializeField] private SpawnObstaclesData spawnObstaclesData;
    [SerializeField] private ObstaclesSpawnSystem obstaclesSpawnSystem;
    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        var newObstacle = Instantiate(spawnObstaclesData.GetRandomObstacleFromPool());
        var obstacleTransform = newObstacle.transform;
        obstacleTransform.position = transform.position;
        obstacleTransform.rotation = transform.rotation;
        newObstacle.Initialize(Vector3.zero, obstaclesSpawnSystem);
        obstaclesSpawnSystem.AddObstacle(newObstacle);

        while (true)
        {
            var r = Random.Range(spawnObstaclesData.minTimeToSpawnObstacle, spawnObstaclesData.maxTimeToSpawnObstacle);
            yield return new WaitForSeconds(r);
            newObstacle = Instantiate(spawnObstaclesData.GetRandomObstacleFromPool());
            obstacleTransform = newObstacle.transform;
            obstacleTransform.position = transform.position;
            obstacleTransform.rotation = transform.rotation;
            newObstacle.Initialize(Vector3.zero, obstaclesSpawnSystem);
            obstaclesSpawnSystem.AddObstacle(newObstacle);
        }
    }
}
