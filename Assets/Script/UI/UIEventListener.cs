using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.Events;

public enum UICommands
{
    CHANGE_SCORE,
    CHANGE_BONUS_SCORE
}

[System.Serializable]
public struct UIParameters
{
    public UICommands Command;
    public float Value;
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





