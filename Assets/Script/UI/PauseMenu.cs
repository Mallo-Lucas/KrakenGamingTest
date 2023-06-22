using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject pausePanel;

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
        resumeButton.onClick.AddListener(Pause);
        quitButton.onClick.AddListener(QuitGame);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void PlayerInputGetActions()
    {
        var inputActions = playerInput.actions;
        _pauseAction = inputActions["Pause"];
        SubscribeActions();
    }

    private void SubscribeActions()
    {
        _pauseAction.performed += PauseAction;
    }

    private void Pause()
    {
        _pause = !_pause;
        foreach (var item in _pausables)
            item.Pause(_pause);
        if (_pause)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            return;
        }
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void PauseAction(InputAction.CallbackContext context)
    {
       Pause();
    }

    private void BackToMainMenu()
    {
        ChangeSceneManager.Instance.GoToMainMenu();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void AddI_PauseObject(I_Pause iPause)
    {
        _pausables.Add(iPause); 
    }
}
