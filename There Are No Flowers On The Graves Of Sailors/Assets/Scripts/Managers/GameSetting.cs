public enum GameMode
{
    PVE,
    PVP
}

public static class GameSettings
{
    public static GameMode currentMode = GameMode.PVE;

    // 可选：定义 AI 控制阵营或玩家阵营等
    public static Faction playerFaction = Faction.USA;
    public static Faction enemyFaction = Faction.Japan;
}
