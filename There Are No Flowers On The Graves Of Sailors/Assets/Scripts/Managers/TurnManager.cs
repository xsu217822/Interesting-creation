// ========================
// TurnManager.cs����̬�ȹ��� - ÿ��Ͷ������
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

    [Header("AI����")]
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
        Debug.Log("[TurnManager] �� " + roundNumber + " �ֿ�ʼ");

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

        Debug.Log("[TurnManager] ��ǰ�ж���λ: " + currentShip.data.shipName);

        if (IsAIControlled(currentShip))
        {
            StartCoroutine(ExecuteAIAction(currentShip));
        }
        else
        {
            // ��ҿ��Ƶ�λ �� �ȴ�����
            // �ɴ���UI��ʾ
        }
    }

    IEnumerator ExecuteAIAction(ShipController aiShip)
    {
        yield return new WaitForSeconds(aiDelay);

        // TODO������AI��ʵ�����߼�
        Debug.Log($"[AI] {aiShip.data.shipName} ������Ϊ������");

        AdvanceToNextTurn();
    }

    int RollInitiativeForShip(ShipController ship)
    {
        int baseRoll = Random.Range(1, 21); // 1d20

        // ����Ӱ��
        float weatherMod = WeatherManager.Instance?.GetWeatherAccuracyModifier() ?? 1f;
        int weatherPenalty = Mathf.RoundToInt((1f - weatherMod) * 10f);

        // ״̬Ӱ�죨��ʱ���ӣ�
        int statusPenalty = 0;

        return baseRoll - weatherPenalty - statusPenalty;
    }

    bool IsAIControlled(ShipController ship)
    {
        // TODO����ʽʵ��ʱ�� faction �� tag �ж��Ƿ�Ϊ AI
        return ship.data.faction == Faction.UK || ship.data.faction == Faction.USA;
    }
}