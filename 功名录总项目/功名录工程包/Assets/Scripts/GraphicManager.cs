using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{
    // 保证 GraphicManager 的唯一实例
    public static GraphicManager Instance;
    public Dropdown resolutionDropdown;
    private List<string> Resolutions = new List<string>();
    private List<int> RefreshRates = new List<int>();

    // 保证 GraphicManager 的唯一实例
    private void Awake()
    {
        // 确保 GraphicManager 在场景切换时不被销毁
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

    // 用于更改游戏分辨率
    public void OnResolutionChange(int resolutionIndex)
    {
        
        // 获取分辨率索引
        int indexOfResolution = resolutionIndex / 3;
        string[] dimensions = Resolutions[indexOfResolution].Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());
        // 保证索引不超出范围
        int indexOfScreenSize = resolutionIndex % 3;
        // 设置分辨率
        if (indexOfScreenSize == 0)
        {
            Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
            resolutionDropdown.GetComponentInChildren<Text>().text = width + "x" + height + " (全屏)";
            Debug.Log("Screen resolution set to: " + width + "x" + height + " (Exclusive Full Screen)");
        }
        else if (indexOfScreenSize == 1)
        {
            Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            resolutionDropdown.GetComponentInChildren<Text>().text = width + "x" + height + " (全屏窗口)";
            Debug.Log("Screen resolution set to: " + width + "x" + height + " (Full Screen Window)");
        }
        else if (indexOfScreenSize == 2)
        {
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
            resolutionDropdown.GetComponentInChildren<Text>().text = width + "x" + height + " (边框窗口)";
            Debug.Log("Screen resolution set to: " + width + "x" + height + " (Windowed)");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions();

        // 获取所有支持的屏幕分辨率
        Resolution[] resolutions = Screen.resolutions;
        // 遍历所有支持的屏幕分辨率
        foreach (Resolution res in resolutions)
        {
            // 构建分辨率字符串（例如："1920 x 1080"）
            string resolutionString = res.width + " x " + res.height;

            // 如果该分辨率不存在于列表中，则添加
            if (!Resolutions.Contains(resolutionString))
            {
                Resolutions.Add(resolutionString);
            }

            // 如果刷新率不存在于列表中，则添加
            if (!RefreshRates.Contains(res.refreshRate))
            {
                RefreshRates.Add(res.refreshRate);
            }
        }

        // 打印所有支持的屏幕分辨率
        foreach (string res in Resolutions)
        {
            // 独占全屏模式选项
            Dropdown.OptionData newOptionExclusiveFullScreen = new Dropdown.OptionData();
            newOptionExclusiveFullScreen.text = res + " (全屏)";
            resolutionDropdown.options.Add(newOptionExclusiveFullScreen);

            // 全屏窗口模式选项
            Dropdown.OptionData newOptionFullScreenWindow = new Dropdown.OptionData();
            newOptionFullScreenWindow.text = res + " (全屏窗口)";
            resolutionDropdown.options.Add(newOptionFullScreenWindow);

            // 窗口化模式选项
            Dropdown.OptionData newOptionWindowed = new Dropdown.OptionData();
            newOptionWindowed.text = res + " (边框窗口)";
            resolutionDropdown.options.Add(newOptionWindowed);

            Debug.Log("Available Resolution: " + res);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
