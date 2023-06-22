using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;
    [SerializeField] private List<string> animationsToActivate;
    [SerializeField] private List<float> animationsTransitionDuration;

    public void ExecuteAnimation(int index)
    {
        animators[index].CrossFade(animationsToActivate[index], animationsTransitionDuration[index]);
    }
}
