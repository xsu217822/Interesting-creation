using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    public GameManager gameManager;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject quitConfirm;
    public GameObject graphicMenu;
    public GameObject audioMenu;
    public GameObject controlMenu;

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

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        quitConfirm.SetActive(false);
        graphicMenu.SetActive(false);
        audioMenu.SetActive(false);
        controlMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameManager.GameState.MainMenu)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        else if (gameManager.gameState == GameManager.GameState.Options)
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }


    }
}
