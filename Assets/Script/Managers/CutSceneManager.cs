using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Animator cutSceneAnimator;
    [SerializeField] private List<Animator> animators; 
    [SerializeField] private List<string> animationsToActivate;
    [SerializeField] private List<float> animationsTransitionDuration;
    [SerializeField] private bool lastLevel;
    [SerializeField] private UIEvent uiEvent;

    private static readonly string LEVEL_END = "LevelEnd";
    private static readonly string LAST_LEVEL_END = "LastLevelEnd";

    private void Start()
    {
        if (LevelEventsHandler.Instance != null)
            LevelEventsHandler.Instance.PlayerWin += EndLevelAnimation;
    }

    public void ExecuteAnimation(int index)
    {
        animators[index].CrossFade(animationsToActivate[index], animationsTransitionDuration[index]);
    }

    public void EndLevelAnimation()
    {
        if (!lastLevel)
        {
            cutSceneAnimator.CrossFade(LEVEL_END,0.2f);
            return;
        }
        cutSceneAnimator.CrossFade(LAST_LEVEL_END, 0.2f);
    }

    public void CutSceneEned()
    {
        uiEvent.Raise(new UIParameters()
        {
            Command = UICommands.FADE_SCREEN_IN,
            Value = 2
        });
        if(!lastLevel)
            LevelEventsHandler.Instance.SubscribeToFadeInEvent(()=> ChangeSceneManager.Instance.GoToNextScene());
    }

}
