// ========================
// GameManager.cs
// 游戏管理器：启动时初始化所有核心系统
// ========================
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("管理器引用")]
    public WeatherManager weatherManager;
    // public TurnManager turnManager;
    // public UIManager uiManager;
    // public ShipManager shipManager;
    // 其他需要的Manager依赖...

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
        Debug.Log("[GameManager] 初始化所有管理系统...");

        if (weatherManager == null)
        {
            weatherManager = FindObjectOfType<WeatherManager>();
            if (weatherManager == null)
                Debug.LogWarning("[GameManager] WeatherManager 未找到！");
        }

        // TODO：其他管理器依次添加...
    }

    void Start()
    {
        Debug.Log("[GameManager] 游戏初始化完成，开始流程...");

        // 例如设置初始天气和时间
        weatherManager?.SetWeather(WeatherType.Clear);
        weatherManager?.SetTime(6, 30);
    }
}
