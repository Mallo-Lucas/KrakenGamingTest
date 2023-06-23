using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawnSystem : MonoBehaviour
{
    [SerializeField] private SpawnObstaclesData spawnObstaclesData;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlayerModel player;
    [SerializeField] private Animator animator;

    private List<Obstacle> _obstaclesCreated = new();

    private static readonly WaitForSeconds WaitForSpawnObstacle = new WaitForSeconds(0.8f);
    private static readonly string THROW_ANIMATION = "Throw";

    private void Awake()
    {
        StartSpawn();
    }

    private void Start()
    {
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(StartSpawn);
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(DestroyAllObstacles);
        LevelEventsHandler.Instance.PlayerWin += delegate { StopSpawn(0); };
        player.OnGetDamage += StopSpawn;       
    }

    private void DestroyAllObstacles()
    {
        for (int i = 0; i < _obstaclesCreated.Count; i++)
            _obstaclesCreated[i].PlayerFail();
        _obstaclesCreated.Clear();
    }

    public void RemoveObstacle(Obstacle obstacle)
    {
        _obstaclesCreated.Remove(obstacle); 
    }

    public void StopSpawn(int value)
    {
        StopAllCoroutines();
        foreach (var obstacle in _obstaclesCreated)
        {
            obstacle.SetObstacleCanMove(false);
        }
    }

    public void AddObstacle(Obstacle obstacle)
    {
        _obstaclesCreated.Add(obstacle);
    }

    private void StartSpawn()
    {
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        animator.CrossFade(THROW_ANIMATION, 0.2f);
        yield return WaitForSpawnObstacle;
        var newObstacle = Instantiate(spawnObstaclesData.GetRandomObstacleFromPool());
        var obstacleTransform = newObstacle.transform;
        obstacleTransform.position = spawnPoint.position;
        obstacleTransform.rotation = spawnPoint.rotation;
        newObstacle.Initialize(spawnPoint.forward, this);
        _obstaclesCreated.Add(newObstacle);
        while (true)
        {
            var r = Random.Range(spawnObstaclesData.minTimeToSpawnObstacle, spawnObstaclesData.maxTimeToSpawnObstacle);
            yield return new WaitForSeconds(r);
            animator.CrossFade(THROW_ANIMATION, 0.2f);
            yield return WaitForSpawnObstacle;
            newObstacle = Instantiate(spawnObstaclesData.GetRandomObstacleFromPool());
            obstacleTransform = newObstacle.transform;
            obstacleTransform.position = spawnPoint.position;
            obstacleTransform.rotation = spawnPoint.rotation;
            newObstacle.Initialize(spawnPoint.forward, this);
            _obstaclesCreated.Add(newObstacle);
        }
    }
}
