// ========================
// WeatherManager.cs
// 控制当前天气与时间，并提供命中修正系数
// ========================
using UnityEngine;

public enum WeatherType
{
    Clear,      // 晴天
    Rain,       // 小雨
    Storm,      // 暴风雨
    Fog         // 大雾
}

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    [Header("时间设置")]
    public int hour = 12;
    public int minute = 0;
    public float timeSpeed = 10f; // 每10秒推进1分钟

    [Header("天气状态")]
    public WeatherType currentWeather = WeatherType.Clear;

    private float timeCounter;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= timeSpeed)
        {
            AdvanceMinute();
            timeCounter = 0f;
        }
    }

    void AdvanceMinute()
    {
        minute++;
        if (minute >= 60)
        {
            minute = 0;
            hour = (hour + 1) % 24;
        }

        // TODO：扩展此处逻辑以根据时间随机改变天气（暂不启用）
        // if (Random.value < 0.05f) currentWeather = (WeatherType)Random.Range(0, 4);
    }

    /// <summary>
    /// 获取当前天气影响命中率的修正值（0~1）
    /// </summary>
    public float GetWeatherAccuracyModifier()
    {
        switch (currentWeather)
        {
            case WeatherType.Clear: return 1.0f;
            case WeatherType.Rain: return 0.9f;
            case WeatherType.Fog: return 0.7f;
            case WeatherType.Storm: return 0.5f;
            default: return 1.0f;
        }
    }

    public string GetFormattedTime()
    {
        return $"{hour:D2}:{minute:D2}";
    }

    public void SetWeather(WeatherType newWeather)
    {
        currentWeather = newWeather;
    }

    public void SetTime(int h, int m)
    {
        hour = Mathf.Clamp(h, 0, 23);
        minute = Mathf.Clamp(m, 0, 59);
    }
}