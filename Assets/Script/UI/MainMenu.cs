using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Animator cutsceneAnimator;
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Image blackScreen;

    private static readonly string MENU_HIDE = "Hide";
    private static readonly string CUTSCENE_START_GAME = "MainMenuStartGame";

    private void Awake()
    {
        BlackScreenFadeOut();
        startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
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
            Debug.Log(blackScreen.color.a);
            yield return null;
        }
    }
}
