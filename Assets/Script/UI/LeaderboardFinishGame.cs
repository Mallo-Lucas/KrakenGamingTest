using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardFinishGame : MonoBehaviour
{
    [SerializeField] private TMP_InputField saveNameInputField;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private Button saveScoreButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button exitGamesButton;
    private void Awake()
    {
        finalScore.text = LevelEventsHandler.Instance.GetFinalScore().ToString();
        saveScoreButton.onClick.AddListener(SaveName);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        exitGamesButton.onClick.AddListener(ExitGame);
    }

    private void SaveName()
    {
        if (saveNameInputField.text == string.Empty || saveNameInputField.text == null)
            return;

        var dataManager = LevelEventsHandler.Instance.GetDataManager();
        var data = dataManager.GetData();
        data.playerNames.Add(saveNameInputField.text);
        data.maxScores.Add(LevelEventsHandler.Instance.GetFinalScore());
        data.currentGameScore = 0;
        dataManager.Save();
        BackToMainMenu();
    }

    private void BackToMainMenu()
    {
        var dataManager = LevelEventsHandler.Instance.GetDataManager();
        dataManager.GetData().currentGameScore = 0;
        dataManager.Save();
        ChangeSceneManager.Instance.GoToMainMenu();
    }

    private void ExitGame()
    {
        var dataManager = LevelEventsHandler.Instance.GetDataManager();
        dataManager.GetData().currentGameScore = 0;
        dataManager.Save();
        Application.Quit();
    }
}
