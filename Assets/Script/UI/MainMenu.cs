using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Animator cutsceneAnimator;
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Image blackScreen;
    [SerializeField] private SFXAudioPoolPlayer audioPoolPlayer;
    [SerializeField] private Slider sliderGeneralVolume;
    [SerializeField] private Slider sliderMusicVolume;
    [SerializeField] private Slider sliderEffectsVolume;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private AudioMixer masterMixer;

    private static readonly string MENU_HIDE = "Hide";
    private static readonly string CUTSCENE_START_GAME = "MainMenuStartGame";
    private static readonly string BUTTON_PRESS = "ButtonPress";

    private void Awake()
    {
        dataManager.Load();     
        BlackScreenFadeOut();
        startGameButton.onClick.AddListener(StartGame);
        exitGameButton.onClick.AddListener(ExitGame);
    }

    private void Start()
    {
        sliderGeneralVolume.onValueChanged.AddListener(ChangeGeneralVolume);
        sliderMusicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        sliderEffectsVolume.onValueChanged.AddListener(ChangeEffectsVolume);

        sliderGeneralVolume.value = dataManager.GetData().GeneralVolume;
        sliderMusicVolume.value = dataManager.GetData().MusicVolume;
        sliderEffectsVolume.value = dataManager.GetData().EffectsVolume;
    }

    private void StartGame()
    {
        audioPoolPlayer.PlayAuidioById(BUTTON_PRESS);
        StartCoroutine(StartGameCoroutine());
    }

    private void ExitGame()
    {
        audioPoolPlayer.PlayAuidioById(BUTTON_PRESS);
        Application.Quit();
    }

    IEnumerator StartGameCoroutine()
    {
        menuAnimator.CrossFade(MENU_HIDE, 0.2f);
        yield return new WaitForSeconds(1.5f);
        cutsceneAnimator.CrossFade(CUTSCENE_START_GAME, 0.2f);
        yield return new WaitForSeconds(7.5f);
        BlackScreenFadeIn();
        yield return new WaitForSeconds(3);
        ChangeSceneManager.Instance.GoToNextScene();
    }

    private void BlackScreenFadeIn()
    {
        var screenColor = blackScreen.color;
        screenColor.a = 0;
        blackScreen.color = screenColor;
        StartCoroutine(FadeScreen(2, 1));      
    }

    private void BlackScreenFadeOut()
    {
        var screenColor = blackScreen.color;
        screenColor.a = 1;
        blackScreen.color = screenColor;
        StartCoroutine(FadeScreen(2, -1));
    }

    IEnumerator FadeScreen(float time, int multiplier)
    {
        var timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime / time;
            var screenColor = blackScreen.color;
            screenColor.a += Time.deltaTime / (time * multiplier);
            blackScreen.color = screenColor;
            yield return null;
        }
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
