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

    // 用于更改游戏状态
    public void ChangeGameState(string newStateName)
    {
        GameState newState = (GameState)System.Enum.Parse(typeof(GameState), newStateName);
        gameState = newState;
        Debug.Log("Game State Changed to: " + newState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                    SceneManager.LoadScene("MainMenuScene");
                }
                break;
            case GameState.GameEditor:
                // 如果当前场景不是游戏编辑器场景
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    SceneManager.LoadScene("MainMenuScene");
                }
                break;
            case GameState.Options:
                // 如果当前场景不是选项场景
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    SceneManager.LoadScene("MainMenuScene");
                }
                break;
            case GameState.Game:
                // 如果当前场景不是游戏场景
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    SceneManager.LoadScene("GameScene");
                }
                break;
            case GameState.InGameOptions:
                // 如果当前场景不是游戏场景
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    SceneManager.LoadScene("GameScene");
                }
                break;
        }
    }
}
