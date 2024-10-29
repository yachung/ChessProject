using System;

public class PlayerData
{
    public int Health { get; set; }
    public int Level { get; set; }
    public int Gold { get; set; }
    public int Exp { get; set; }

    public PlayerData()
    {
        Health = 100;
        Level = 1;
        Gold = 5;
        Exp = 0;
    }

    public PlayerData(int health, int level, int gold, int exp)
    {
        Health = health;
        Level = level;
        Gold = gold;
        Exp = Exp;
    }

    public Action<int> OnGoldChanged;
    public Action<int> OnExperienceChanged;
    public Action<int> OnLevelChanged;
    public Action<int> OnHealthChanged;
}
