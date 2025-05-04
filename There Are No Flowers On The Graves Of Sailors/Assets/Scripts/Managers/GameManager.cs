// ========================
// GameManager.cs
// ��Ϸ������������ʱ��ʼ�����к���ϵͳ
// ========================
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("����������")]
    public WeatherManager weatherManager;
    // public TurnManager turnManager;
    // public UIManager uiManager;
    // public ShipManager shipManager;
    // ������Ҫ��Manager����...

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeSystems();
    }

    void InitializeSystems()
    {
        Debug.Log("[GameManager] ��ʼ�����й���ϵͳ...");

        if (weatherManager == null)
        {
            weatherManager = FindObjectOfType<WeatherManager>();
            if (weatherManager == null)
                Debug.LogWarning("[GameManager] WeatherManager δ�ҵ���");
        }

        // TODO�������������������...
    }

    void Start()
    {
        Debug.Log("[GameManager] ��Ϸ��ʼ����ɣ���ʼ����...");

        // �������ó�ʼ������ʱ��
        weatherManager?.SetWeather(WeatherType.Clear);
        weatherManager?.SetTime(6, 30);
    }
}
