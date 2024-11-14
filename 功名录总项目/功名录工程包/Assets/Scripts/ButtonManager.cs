using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class ButtonManager : MonoBehaviour
{
    // 保证 ButtonManager 的唯一实例
    public static ButtonManager Instance;
    public GameManager gameManager;
    public CanvasManager canvasManager;



    private void Awake()
    {
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

    public void OnStartButtonClick()
    {
        gameManager.ChangeGameState("GameEditor");
        Debug.Log("Start Button Clicked");
    }

    public void OnOptionsButtonClick()
    {
        gameManager.ChangeGameState("Options");
        canvasManager.graphicMenu.SetActive(true);
        Debug.Log("Options Button Clicked");
    }

    public void OnBackButtonClick()
    {
        gameManager.ChangeGameState("MainMenu");
        Debug.Log("Back Button Clicked");
    }

    public void OnExitButtonClick()
    {
        canvasManager.quitConfirm.SetActive(true);
        Debug.Log("Exit Button Clicked");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("Exit Button Clicked");
    }

    public void OnCancelButtonClick()
    {
        canvasManager.quitConfirm.SetActive(false);
        Debug.Log("Cancel Button Clicked");
    }

    public void OnGraphicButtonClick()
    {
        canvasManager.graphicMenu.SetActive(true);
        canvasManager.audioMenu.SetActive(false);
        canvasManager.controlMenu.SetActive(false);
        Debug.Log("Graphic Button Clicked");
    }

    public void OnAudioButtonClick()
    {
        canvasManager.graphicMenu.SetActive(false);
        canvasManager.audioMenu.SetActive(true);
        canvasManager.controlMenu.SetActive(false);
        Debug.Log("Audio Button Clicked");
    }

    public void OnControlButtonClick()
    {
        canvasManager.graphicMenu.SetActive(false);
        canvasManager.audioMenu.SetActive(false);
        canvasManager.controlMenu.SetActive(true);
        Debug.Log("Control Button Clicked");
    }
}
