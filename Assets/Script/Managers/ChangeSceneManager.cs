using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField] private int currentSceneIndex;
    public static ChangeSceneManager Instance;

    private void Awake()
    {
        Instance = this; 
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadSceneAsync(currentSceneIndex+1, LoadSceneMode.Single);
    }
}
