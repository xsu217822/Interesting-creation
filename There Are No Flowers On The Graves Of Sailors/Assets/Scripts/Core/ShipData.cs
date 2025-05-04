// ========================
// ShipData.cs（舰船数据模板 - 使用结构体表示十二向攻击修正 + 自动填充）
// ========================
using UnityEngine;

public enum ShipType { Destroyer, Cruiser, Battleship, Carrier }
public enum Faction { USA, UK, France, Germany, Japan, Italy, USSR, China }

[System.Flags]
public enum ShipBehaviorRule
{
    None = 0,
    CanMoveAndAttack = 1 << 0,
    CanMoveThenAttack = 1 << 1,
    CanAttackThenMove = 1 << 2,
    CanOnlyMoveOrAttack = 1 << 3,
    NeedsWindToLaunchPlanes = 1 << 4
}

[System.Flags]
public enum ShipWeaponCapability
{
    None = 0,
    CanUseHE = 1 << 0,
    CanUseAP = 1 << 1,
    CanUseTorpedo = 1 << 2,
    CanUseAirstrike = 1 << 3
}

public enum Direction12
{
    Front = 0,
    FrontRight = 1,
    FrontLeft = 2,
    BackRight = 3,
    BackLeft = 4,
    Back = 5,
    FrontLeftDiagonal = 6,
    FrontRightDiagonal = 7,
    Right = 8,
    Left = 9,
    BackRightDiagonal = 10,
    BackLeftDiagonal = 11
}

[System.Serializable]
public struct DirectionalFireEntry
{
    public Direction12 direction;
    [Range(0f, 1f)] public float ratio;
}

[CreateAssetMenu(menuName = "Ship/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("基本属性")]
    public string shipName;
    public ShipType shipType;
    public Faction faction;

    public int maxHP;
    public int moveRange;
    public int attackRange;          // 炮击攻击距离（HE/AP）
    public int torpedoRange;         // 鱼雷攻击距离

    [Header("命中率设置")]
    [Range(0f, 1f)] public float baseHitChance = 0.8f;         // 炮击命中率
    [Range(0f, 1f)] public float torpedoHitChance = 0.6f;      // 鱼雷命中率
    [Range(0f, 1f)] public float airstrikeHitChance = 0.5f;    // 空袭命中率
    [Range(0.1f, 2f)] public float hitTakenRate = 1f;          // 被命中率

    [Header("攻击力设置")]
    public int heShellDamage;              // 高爆
    public float apShellPercent;           // 穿甲百分比
    public int torpedoDamage;              // 鱼雷
    public int airstrikeDamage;            // 空袭

    [Header("防御属性")]
    public int armor;                      // 对HE/空袭有效
    [Range(0f, 1f)] public float apResistance = 1f;        // 穿甲抗性
    [Range(0f, 1f)] public float torpedoResistance = 0f;   // 鱼雷抗性

    [Header("战术行为规则")]
    public ShipBehaviorRule behaviorRules;

    [Header("可用武装类型")]
    public ShipWeaponCapability weaponCapabilities;

    [Header("模型绑定")]
    public GameObject shipPrefab;

    [Header("十二向攻击修正（使用 Direction12 枚举）")]
    public DirectionalFireEntry[] directionalFireTable = new DirectionalFireEntry[12];

    private void OnValidate()
    {
        if (directionalFireTable == null || directionalFireTable.Length != 12)
        {
            directionalFireTable = new DirectionalFireEntry[12];
        }

        for (int i = 0; i < 12; i++)
        {
            directionalFireTable[i].direction = (Direction12)i;
        }
    }
}