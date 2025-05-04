// ========================
// ShipCombatUtility.cs（新版 - 支持 Direction12 + 扩展钩子）
// ========================
using UnityEngine;

public static class ShipCombatUtility
{
    /// <summary>
    /// 获取某方向上的主炮修正系数（directionalFireTable）
    /// </summary>
    public static float GetDirectionalModifier(ShipData shipData, Direction12 direction)
    {
        foreach (var entry in shipData.directionalFireTable)
        {
            if (entry.direction == direction)
                return entry.ratio;
        }
        return 0f;
    }

    /// <summary>
    /// 计算攻击命中率（方向修正 + 距离衰减 + 被命中率 + 天气 + debuff）
    /// </summary>
    public static float CalculateHitChance(ShipData attacker, ShipData defender, float distance, Direction12 direction, AttackType attackType)
    {
        float baseChance = 0f;

        switch (attackType)
        {
            case AttackType.HE:
            case AttackType.AP:
                baseChance = attacker.baseHitChance;
                break;
            case AttackType.Torpedo:
                baseChance = attacker.torpedoHitChance;
                break;
            case AttackType.Airstrike:
                baseChance = attacker.airstrikeHitChance;
                break;
        }

        float decay = Mathf.Clamp01(1f - distance / (attacker.attackRange + 1f));
        float directional = GetDirectionalModifier(attacker, direction);

        // ? 扩展命中率影响因素（预留）：天气系数、设备损伤、技能效果等
        float weatherModifier = 1f;  // TODO: 外部注入天气影响命中率（如暴雨 = 0.8f）
        float statusDebuffModifier = 1f; // TODO: 若船只受火控雷达损坏等影响可降低为 <1

        float finalChance = baseChance * decay * directional * defender.hitTakenRate * weatherModifier * statusDebuffModifier;
        return Mathf.Clamp01(finalChance);
    }

    /// <summary>
    /// 计算造成的伤害（方向修正 + 各类型抗性）
    /// </summary>
    public static int CalculateDamage(ShipData attacker, ShipData defender, Direction12 direction, AttackType attackType)
    {
        float modifier = GetDirectionalModifier(attacker, direction);
        float damage = 0f;

        switch (attackType)
        {
            case AttackType.HE:
                damage = attacker.heShellDamage * modifier * (1f - defender.armor / 100f);
                break;

            case AttackType.AP:
                float baseAP = attacker.maxHP * attacker.apShellPercent;
                damage = baseAP * modifier * (1f - defender.apResistance);
                break;

            case AttackType.Torpedo:
                damage = attacker.torpedoDamage * modifier * (1f - defender.torpedoResistance);
                break;

            case AttackType.Airstrike:
                damage = attacker.airstrikeDamage * modifier * (1f - defender.armor / 100f);
                break;
        }

        return Mathf.Max(0, Mathf.RoundToInt(damage));
    }
}

public enum AttackType
{
    HE,
    AP,
    Torpedo,
    Airstrike
}
