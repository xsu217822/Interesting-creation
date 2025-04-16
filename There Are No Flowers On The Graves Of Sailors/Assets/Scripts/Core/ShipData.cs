// ========================
// ShipData.cs����������ģ�� - ʹ�ýṹ���ʾʮ���򹥻����� + �Զ���䣩
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
    [Header("��������")]
    public string shipName;
    public ShipType shipType;
    public Faction faction;

    public int maxHP;
    public int moveRange;
    public int attackRange;          // �ڻ��������루HE/AP��
    public int torpedoRange;         // ���׹�������

    [Header("����������")]
    [Range(0f, 1f)] public float baseHitChance = 0.8f;         // �ڻ�������
    [Range(0f, 1f)] public float torpedoHitChance = 0.6f;      // ����������
    [Range(0f, 1f)] public float airstrikeHitChance = 0.5f;    // ��Ϯ������
    [Range(0.1f, 2f)] public float hitTakenRate = 1f;          // ��������

    [Header("����������")]
    public int heShellDamage;              // �߱�
    public float apShellPercent;           // ���װٷֱ�
    public int torpedoDamage;              // ����
    public int airstrikeDamage;            // ��Ϯ

    [Header("��������")]
    public int armor;                      // ��HE/��Ϯ��Ч
    [Range(0f, 1f)] public float apResistance = 1f;        // ���׿���
    [Range(0f, 1f)] public float torpedoResistance = 0f;   // ���׿���

    [Header("ս����Ϊ����")]
    public ShipBehaviorRule behaviorRules;

    [Header("������װ����")]
    public ShipWeaponCapability weaponCapabilities;

    [Header("ģ�Ͱ�")]
    public GameObject shipPrefab;

    [Header("ʮ���򹥻�������ʹ�� Direction12 ö�٣�")]
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