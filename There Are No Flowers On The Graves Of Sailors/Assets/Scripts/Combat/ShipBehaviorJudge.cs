// ========================
// ShipBehaviorJudge.cs
// ========================
using UnityEngine;

public static class ShipBehaviorJudge
{
    public static bool CanAct(ShipBehaviorRule rules, bool hasMoved, bool hasAttacked)
    {
        if (rules.HasFlag(ShipBehaviorRule.CanMoveAndAttack))
        {
            return !(hasMoved && hasAttacked);
        }
        if (rules.HasFlag(ShipBehaviorRule.CanMoveThenAttack))
        {
            return !hasMoved || !hasAttacked;
        }
        if (rules.HasFlag(ShipBehaviorRule.CanAttackThenMove))
        {
            return !hasAttacked || !hasMoved;
        }
        if (rules.HasFlag(ShipBehaviorRule.CanOnlyMoveOrAttack))
        {
            return !hasMoved && !hasAttacked;
        }

        return true;
    }
}