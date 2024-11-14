using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{
    // ��֤ GraphicManager ��Ψһʵ��
    public static GraphicManager Instance;
    public Dropdown resolutionDropdown;
    private List<string> Resolutions = new List<string>();
    private List<int> RefreshRates = new List<int>();

    // ��֤ GraphicManager ��Ψһʵ��
    private void Awake()
    {
        // ȷ�� GraphicManager �ڳ����л�ʱ��������
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

    // ���ڸ�����Ϸ�ֱ���
    public void OnResolutionChange(int resolutionIndex)
    {
        
        // ��ȡ�ֱ�������
        int indexOfResolution = resolutionIndex / 3;
        string[] dimensions = Resolutions[indexOfResolution].Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());
        // ��֤������������Χ
        int indexOfScreenSize = resolutionIndex % 3;
        // ���÷ֱ���
        if (indexOfScreenSize == 0)
        {
            Screen.SetResolution(width, height, FullScreenMode.ExclusiveFullScreen);
            resolutionDropdown.GetComponentInChildren<Text>().text = width + "x" + height + " (ȫ��)";
            Debug.Log("Screen resolution set to: " + width + "x" + height + " (Exclusive Full Screen)");
        }
        else if (indexOfScreenSize == 1)
        {
            Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            resolutionDropdown.GetComponentInChildren<Text>().text = width + "x" + height + " (ȫ������)";
            Debug.Log("Screen resolution set to: " + width + "x" + height + " (Full Screen Window)");
        }
        else if (indexOfScreenSize == 2)
        {
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
            resolutionDropdown.GetComponentInChildren<Text>().text = width + "x" + height + " (�߿򴰿�)";
            Debug.Log("Screen resolution set to: " + width + "x" + height + " (Windowed)");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions();

        // ��ȡ����֧�ֵ���Ļ�ֱ���
        Resolution[] resolutions = Screen.resolutions;
        // ��������֧�ֵ���Ļ�ֱ���
        foreach (Resolution res in resolutions)
        {
            // �����ֱ����ַ��������磺"1920 x 1080"��
            string resolutionString = res.width + " x " + res.height;

            // ����÷ֱ��ʲ��������б��У������
            if (!Resolutions.Contains(resolutionString))
            {
                Resolutions.Add(resolutionString);
            }

            // ���ˢ���ʲ��������б��У������
            if (!RefreshRates.Contains(res.refreshRate))
            {
                RefreshRates.Add(res.refreshRate);
            }
        }

        // ��ӡ����֧�ֵ���Ļ�ֱ���
        foreach (string res in Resolutions)
        {
            // ��ռȫ��ģʽѡ��
            Dropdown.OptionData newOptionExclusiveFullScreen = new Dropdown.OptionData();
            newOptionExclusiveFullScreen.text = res + " (ȫ��)";
            resolutionDropdown.options.Add(newOptionExclusiveFullScreen);

            // ȫ������ģʽѡ��
            Dropdown.OptionData newOptionFullScreenWindow = new Dropdown.OptionData();
            newOptionFullScreenWindow.text = res + " (ȫ������)";
            resolutionDropdown.options.Add(newOptionFullScreenWindow);

            // ���ڻ�ģʽѡ��
            Dropdown.OptionData newOptionWindowed = new Dropdown.OptionData();
            newOptionWindowed.text = res + " (�߿򴰿�)";
            resolutionDropdown.options.Add(newOptionWindowed);

            Debug.Log("Available Resolution: " + res);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
