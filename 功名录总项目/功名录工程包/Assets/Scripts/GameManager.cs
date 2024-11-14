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

    // ���ڸ�����Ϸ״̬
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
                // �����ǰ�����������˵�����
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    SceneManager.LoadScene("MainMenuScene");
                }
                break;
            case GameState.GameEditor:
                // �����ǰ����������Ϸ�༭������
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    SceneManager.LoadScene("MainMenuScene");
                }
                break;
            case GameState.Options:
                // �����ǰ��������ѡ���
                if (SceneManager.GetActiveScene().name != "MainMenuScene")
                {
                    SceneManager.LoadScene("MainMenuScene");
                }
                break;
            case GameState.Game:
                // �����ǰ����������Ϸ����
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    SceneManager.LoadScene("GameScene");
                }
                break;
            case GameState.InGameOptions:
                // �����ǰ����������Ϸ����
                if (SceneManager.GetActiveScene().name != "GameScene")
                {
                    SceneManager.LoadScene("GameScene");
                }
                break;
        }
    }
}
