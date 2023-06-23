using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShieldOnPlayer : MonoBehaviour
{
    [SerializeField] private Animator shieldsAnimator;
    private bool _almostDone;

    private static readonly string ALMOST_DONE = "ShieldAlmostOver";

    public void Initialize(float value)
    {
        StartCoroutine(DestroyPowerUp(value));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Obstacle obstacle))
            obstacle.DestroyObstacleWhitScore();
    }

    private IEnumerator DestroyPowerUp(float value)
    {
        var timer = value;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer<=2 && !_almostDone)
            {
                shieldsAnimator.CrossFade(ALMOST_DONE, 0.2f);
                _almostDone = true;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
