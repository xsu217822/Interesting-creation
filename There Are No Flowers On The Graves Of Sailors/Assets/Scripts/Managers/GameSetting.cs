public enum GameMode
{
    PVE,
    PVP
}

public static class GameSettings
{
    public static GameMode currentMode = GameMode.PVE;

    // ��ѡ������ AI ������Ӫ�������Ӫ��
    public static Faction playerFaction = Faction.USA;
    public static Faction enemyFaction = Faction.Japan;
}
