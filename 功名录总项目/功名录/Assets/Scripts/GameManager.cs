using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���ڴ洢��ǰ��Ϸ״̬
    public enum GameState { MainMenu, GameEditor, Options, Game, InGameOptions }
    public GameState gameState = GameState.MainMenu;

    // ��֤ GameManager ��Ψһʵ��
    public static GameManager Instance;

    //ȷ�� GameManager �ڳ����л�ʱ��������
    private void Awake()
    {
        // ȷ�� GameManager �ڳ����л�ʱ��������
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

    // ���ڼ����µĳ���
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // �������˵�����
    public void LoadMainMenu()
    {
        LoadScene("MainMenuScene");
    }

    // ������Ϸ����
    public void LoadGame()
    {
        LoadScene("GameScene");
    }

    // �˳���Ϸ
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
                // �����ǰ�����������˵�����
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    LoadMainMenu();
                }
                break;
            case GameState.GameEditor:
                // �����ǰ����������Ϸ�༭������
                if (SceneManager.GetActiveScene().name != "GameEditorScene")
                {
                    LoadScene("GameEditorScene");
                }
                break;
            case GameState.Options:
                // �����ǰ��������ѡ���
                if (SceneManager.GetActiveScene().name != "OptionsScene")
                {
                    LoadScene("OptionsScene");
                }
                break;
            case GameState.Game:
                // �����ǰ����������Ϸ����
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    LoadGame();
                }
                break;
            case GameState.InGameOptions:
                // �����ǰ����������Ϸ����
                if (SceneManager.GetActiveScene().name != "InGameOptionsScene")
                {
                    LoadScene("InGameOptionsScene");
                }
                break;
        }
    }
}
