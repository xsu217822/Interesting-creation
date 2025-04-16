// ========================
// ShipCombatUtility.cs���°� - ֧�� Direction12 + ��չ���ӣ�
// ========================
using UnityEngine;

public static class ShipCombatUtility
{
    /// <summary>
    /// ��ȡĳ�����ϵ���������ϵ����directionalFireTable��
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
    /// ���㹥�������ʣ��������� + ����˥�� + �������� + ���� + debuff��
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

        // ? ��չ������Ӱ�����أ�Ԥ����������ϵ�����豸���ˡ�����Ч����
        float weatherModifier = 1f;  // TODO: �ⲿע������Ӱ�������ʣ��籩�� = 0.8f��
        float statusDebuffModifier = 1f; // TODO: ����ֻ�ܻ���״��𻵵�Ӱ��ɽ���Ϊ <1

        float finalChance = baseChance * decay * directional * defender.hitTakenRate * weatherModifier * statusDebuffModifier;
        return Mathf.Clamp01(finalChance);
    }

    /// <summary>
    /// ������ɵ��˺����������� + �����Ϳ��ԣ�
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
