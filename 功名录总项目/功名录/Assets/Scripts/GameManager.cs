using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 用于存储当前游戏状态
    public enum GameState { MainMenu, GameEditor, Options, Game, InGameOptions }
    public GameState gameState = GameState.MainMenu;

    // 保证 GameManager 的唯一实例
    public static GameManager Instance;

    //确保 GameManager 在场景切换时不被销毁
    private void Awake()
    {
        // 确保 GameManager 在场景切换时不被销毁
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 用于加载新的场景
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 加载主菜单场景
    public void LoadMainMenu()
    {
        LoadScene("MainMenuScene");
    }

    // 加载游戏场景
    public void LoadGame()
    {
        LoadScene("GameScene");
    }

    // 退出游戏
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                // 如果当前场景不是主菜单场景
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    LoadMainMenu();
                }
                break;
            case GameState.GameEditor:
                // 如果当前场景不是游戏编辑器场景
                if (SceneManager.GetActiveScene().name != "GameEditorScene")
                {
                    LoadScene("GameEditorScene");
                }
                break;
            case GameState.Options:
                // 如果当前场景不是选项场景
                if (SceneManager.GetActiveScene().name != "OptionsScene")
                {
                    LoadScene("OptionsScene");
                }
                break;
            case GameState.Game:
                // 如果当前场景不是游戏场景
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    LoadGame();
                }
                break;
            case GameState.InGameOptions:
                // 如果当前场景不是游戏场景
                if (SceneManager.GetActiveScene().name != "InGameOptionsScene")
                {
                    LoadScene("InGameOptionsScene");
                }
                break;
        }
    }
}
