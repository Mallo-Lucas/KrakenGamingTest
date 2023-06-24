using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.Events;

public enum UICommands
{
    CHANGE_SCORE,
    CHANGE_BONUS_SCORE,
    SET_PLAYERS_HEARTS,
    FADE_SCREEN_IN,
    FADE_SCREEN_OUT,
    GAME_OVER,
    ACTIVATE_ABILITY_UI,
    CHANGE_ABILITY_ICON,
    CHANEGE_ABILITY_AMMOUNT,
    OPEN_LEADERBOARDs
}

[System.Serializable]
public struct UIParameters
{
    public UICommands Command;
    public float Value;
    public bool State;
    public Sprite Sprite;
}

[System.Serializable]
public class UIUnityEvent : UnityEvent<UIParameters>
{

}

public class UIEventListener : MonoBehaviour
{
    [SerializeField] private UIEvent uiEvent;
    [SerializeField] private UIUnityEvent response;

    private void OnEnable()
    {
        uiEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        uiEvent.UnregisterListener(this);
    }

    public void OnEventRaised(UIParameters parameters)
    {
        response.Invoke(parameters);
    }
}





