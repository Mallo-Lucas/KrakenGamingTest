using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Slider sliderGeneralVolume;
    [SerializeField] private Slider sliderMusicVolume;
    [SerializeField] private Slider sliderEffectsVolume;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private AudioMixer masterMixer;

    private List<I_Pause> _pausables = new ();
    private InputAction _pauseAction;
    private bool _pause;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerInputGetActions();
        resumeButton.onClick.AddListener(Pause);
        quitButton.onClick.AddListener(QuitGame);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);

        sliderGeneralVolume.onValueChanged.AddListener(ChangeGeneralVolume);
        sliderMusicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        sliderEffectsVolume.onValueChanged.AddListener(ChangeEffectsVolume);

        sliderGeneralVolume.value = dataManager.GetData().GeneralVolume;
        sliderMusicVolume.value = dataManager.GetData().MusicVolume;
        sliderEffectsVolume.value = dataManager.GetData().EffectsVolume;
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

    public void Pause()
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
        optionsPanel.SetActive(false);
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

    public void ChangeGeneralVolume(float newValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderGeneralVolume.value) * 20);
        dataManager.GetData().GeneralVolume = newValue;
    }

    public void ChangeMusicVolume(float newValue)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(sliderMusicVolume.value) * 20);
        dataManager.GetData().MusicVolume = newValue;
    }

    public void ChangeEffectsVolume(float newValue)
    {
        masterMixer.SetFloat("EffectsVolume", Mathf.Log10(sliderEffectsVolume.value) * 20);
        dataManager.GetData().EffectsVolume = newValue;
    }
}
