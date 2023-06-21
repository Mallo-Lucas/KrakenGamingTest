using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private List<I_Pause> _pausables = new ();
    private InputAction _pauseAction;
    private bool _pause;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerInputGetActions();
    }

    private void PlayerInputGetActions()
    {
        var inputActions = playerInput.actions;
        _pauseAction = inputActions["Pause"];
        SubscribeActions();
    }

    private void SubscribeActions()
    {
        _pauseAction.performed += Pause;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        _pause = !_pause;
        foreach (var item in _pausables)
            item.Pause(_pause);
        if (_pause)
        {
            Time.timeScale = 0;
            return;
        }
        Time.timeScale = 1;
    }

    public void AddI_PauseObject(I_Pause iPause)
    {
        _pausables.Add(iPause); 
    }
}
