// ========================
// ShipController.cs（新版 - 兼容 Direction12 和 CombatFoundation）
// ========================
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("绑定的数据模板")]
    public ShipData data;

    [Header("运行时状态")]
    public int currentHP;
    public bool hasMoved = false;
    public bool hasAttacked = false;
    public bool isDestroyed = false;

    public Direction12 facingDirection = Direction12.Front; // 朝向（默认前）
    public Faction Faction => data.faction;

    void Start()
    {
        currentHP = data.maxHP;
    }

    // ========== 移动控制 ==========
    public void MoveTo(Vector3 target)
    {
        if (!hasMoved)
        {
            transform.position = target;
            hasMoved = true;
        }
    }

    // ========== 攻击控制 ==========
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
                Debug.Log($"{data.shipName} 命中 {target.data.shipName}，造成 {damage} 伤害！");
            }
            else
            {
                Debug.Log($"{data.shipName} 攻击 {target.data.shipName} 未命中。");
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
        Debug.Log($"{data.shipName} 被击沉！");
        Destroy(gameObject);
    }

    // ========== 回合控制 ==========
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

    // ========== 方向判定（占位函数） ==========
    public Direction12 CalculateRelativeDirectionTo(ShipController target)
    {
        // TODO：写一个六边形地图方向判断器来替代这个假设实现
        Vector3 dir = (target.transform.position - this.transform.position).normalized;
        return facingDirection; // 临时默认用自身朝向（后续应根据位置关系计算真实方向）
    }
}
