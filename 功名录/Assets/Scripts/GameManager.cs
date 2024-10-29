using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
