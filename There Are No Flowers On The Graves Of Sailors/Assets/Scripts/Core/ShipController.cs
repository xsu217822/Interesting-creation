// ========================
// ShipController.cs���°� - ���� Direction12 �� CombatFoundation��
// ========================
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("�󶨵�����ģ��")]
    public ShipData data;

    [Header("����ʱ״̬")]
    public int currentHP;
    public bool hasMoved = false;
    public bool hasAttacked = false;
    public bool isDestroyed = false;

    public Direction12 facingDirection = Direction12.Front; // ����Ĭ��ǰ��
    public Faction Faction => data.faction;

    void Start()
    {
        currentHP = data.maxHP;
    }

    // ========== �ƶ����� ==========
    public void MoveTo(Vector3 target)
    {
        if (!hasMoved)
        {
            transform.position = target;
            hasMoved = true;
        }
    }

    // ========== �������� ==========
    public void Attack(ShipController target, AttackType attackType)
    {
        if (!hasAttacked && CanUseWeapon(attackType))
        {
            float distance = Vector3.Distance(this.transform.position, target.transform.position);
            Direction12 directionToTarget = CalculateRelativeDirectionTo(target);

            float chance = ShipCombatUtility.CalculateHitChance(
                data,
                target.data,
                distance,
                directionToTarget,
                attackType
            );

            float roll = Random.Range(0f, 1f);

            if (roll <= chance)
            {
                int damage = ShipCombatUtility.CalculateDamage(
                    data,
                    target.data,
                    directionToTarget,
                    attackType
                );
                target.TakeDamage(damage);
                Debug.Log($"{data.shipName} ���� {target.data.shipName}����� {damage} �˺���");
            }
            else
            {
                Debug.Log($"{data.shipName} ���� {target.data.shipName} δ���С�");
            }

            hasAttacked = true;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0 && !isDestroyed)
        {
            Sink();
        }
    }

    void Sink()
    {
        isDestroyed = true;
        Debug.Log($"{data.shipName} ��������");
        Destroy(gameObject);
    }

    // ========== �غϿ��� ==========
    public void ResetTurn()
    {
        hasMoved = false;
        hasAttacked = false;
    }

    public bool CanActThisTurn()
    {
        return ShipBehaviorJudge.CanAct(data.behaviorRules, hasMoved, hasAttacked);
    }

    public bool CanUseWeapon(AttackType type)
    {
        switch (type)
        {
            case AttackType.HE:
                return data.weaponCapabilities.HasFlag(ShipWeaponCapability.CanUseHE);
            case AttackType.AP:
                return data.weaponCapabilities.HasFlag(ShipWeaponCapability.CanUseAP);
            case AttackType.Torpedo:
                return data.weaponCapabilities.HasFlag(ShipWeaponCapability.CanUseTorpedo);
            case AttackType.Airstrike:
                return data.weaponCapabilities.HasFlag(ShipWeaponCapability.CanUseAirstrike);
        }
        return false;
    }

    // ========== �����ж���ռλ������ ==========
    public Direction12 CalculateRelativeDirectionTo(ShipController target)
    {
        // TODO��дһ�������ε�ͼ�����ж���������������ʵ��
        Vector3 dir = (target.transform.position - this.transform.position).normalized;
        return facingDirection; // ��ʱĬ���������򣨺���Ӧ����λ�ù�ϵ������ʵ����
    }
}
