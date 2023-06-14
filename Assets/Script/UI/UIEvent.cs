using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI_Event", menuName = "Events/UI", order = 0)]
public class UIEvent : ScriptableObject
{
    private List<UIEventListener> listeners = new();

    public void Raise(UIParameters p)
    {
        for (var i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(p);
        }
    }

    public void RegisterListener(UIEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(UIEventListener listener)
    {
        listeners.Remove(listener);
    }
}

