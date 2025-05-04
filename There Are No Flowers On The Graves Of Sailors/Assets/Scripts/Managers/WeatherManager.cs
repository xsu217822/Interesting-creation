// ========================
// WeatherManager.cs
// ���Ƶ�ǰ������ʱ�䣬���ṩ��������ϵ��
// ========================
using UnityEngine;

public enum WeatherType
{
    Clear,      // ����
    Rain,       // С��
    Storm,      // ������
    Fog         // ����
}

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    [Header("ʱ������")]
    public int hour = 12;
    public int minute = 0;
    public float timeSpeed = 10f; // ÿ10���ƽ�1����

    [Header("����״̬")]
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

        // TODO����չ�˴��߼��Ը���ʱ������ı��������ݲ����ã�
        // if (Random.value < 0.05f) currentWeather = (WeatherType)Random.Range(0, 4);
    }

    /// <summary>
    /// ��ȡ��ǰ����Ӱ�������ʵ�����ֵ��0~1��
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