using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
