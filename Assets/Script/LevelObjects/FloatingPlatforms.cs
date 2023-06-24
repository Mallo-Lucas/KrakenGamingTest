using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatforms : MonoBehaviour
{
    [SerializeField] private Transform platformToMove;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float loopTime;
    [SerializeField] private bool goUp;

    private void Awake()
    {
        StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        float timer = 0;
        while (true)
        {
            switch (goUp) 
            {
                case true:
                    timer += Time.deltaTime;
                    platformToMove.position = Vector3.Lerp(startPos.position, endPos.position, timer / loopTime);
                    if (timer > loopTime)
                    {
                        timer = 0;
                        goUp = false;
                    }
                    break;
                case false:
                    timer += Time.deltaTime;
                    platformToMove.position = Vector3.Lerp(endPos.position, startPos.position, timer / loopTime);
                    if (timer > loopTime)
                    {
                        timer = 0;
                        goUp = true;
                    }
                    break;
            }
            yield return null;
        }
        
    }

}
