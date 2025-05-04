// ========================
// TurnManager.cs（动态先攻制 - 每轮投骰排序）
// ========================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    private Queue<ShipController> turnQueue = new();
    private Dictionary<ShipController, int> initiativeRoll = new();

    public List<ShipController> allShips = new();
    public ShipController currentShip;

    [Header("AI设置")]
    public float aiDelay = 1f;

    private int roundNumber = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        roundNumber++;
        Debug.Log("[TurnManager] 第 " + roundNumber + " 轮开始");

        initiativeRoll.Clear();

        foreach (var ship in allShips.Where(s => !s.isDestroyed))
        {
            int roll = RollInitiativeForShip(ship);
            initiativeRoll[ship] = roll;
            ship.ResetTurn();
        }

        var sorted = initiativeRoll.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).ToList();
        turnQueue = new Queue<ShipController>(sorted);

        AdvanceToNextTurn();
    }

    public void AdvanceToNextTurn()
    {
        if (turnQueue.Count == 0)
        {
            StartNewRound();
            return;
        }

        currentShip = turnQueue.Dequeue();

        if (currentShip.isDestroyed)
        {
            AdvanceToNextTurn();
            return;
        }

        Debug.Log("[TurnManager] 当前行动单位: " + currentShip.data.shipName);

        if (IsAIControlled(currentShip))
        {
            StartCoroutine(ExecuteAIAction(currentShip));
        }
        else
        {
            // 玩家控制单位 → 等待输入
            // 可触发UI提示
        }
    }

    IEnumerator ExecuteAIAction(ShipController aiShip)
    {
        yield return new WaitForSeconds(aiDelay);

        // TODO：加入AI真实决策逻辑
        Debug.Log($"[AI] {aiShip.data.shipName} 暂无行为，跳过");

        AdvanceToNextTurn();
    }

    int RollInitiativeForShip(ShipController ship)
    {
        int baseRoll = Random.Range(1, 21); // 1d20

        // 天气影响
        float weatherMod = WeatherManager.Instance?.GetWeatherAccuracyModifier() ?? 1f;
        int weatherPenalty = Mathf.RoundToInt((1f - weatherMod) * 10f);

        // 状态影响（暂时不加）
        int statusPenalty = 0;

        return baseRoll - weatherPenalty - statusPenalty;
    }

    bool IsAIControlled(ShipController ship)
    {
        // TODO：正式实现时以 faction 或 tag 判断是否为 AI
        return ship.data.faction == Faction.UK || ship.data.faction == Faction.USA;
    }
}