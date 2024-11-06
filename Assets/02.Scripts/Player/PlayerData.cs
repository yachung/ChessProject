using System;

public class PlayerData
{
    public string Name { get; set; }
    private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            OnHpChanged(hp);
        }
    }

    private int level;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            OnLevelChanged(level);
        }
    }

    private int gold;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnGoldChanged(gold);
        }
    }

    private int exp;
    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            OnExperienceChanged(exp);
        }
    }

    public PlayerData()
    {
        Hp = 100;
        Level = 1;
        Gold = 5;
        Exp = 0;
    }

    public PlayerData(int health, int level, int gold, int exp)
    {
        Hp = health;
        Level = level;
        Gold = gold;
        Exp = Exp;
    }

    public Action<int> OnGoldChanged;
    public Action<int> OnExperienceChanged;
    public Action<int> OnLevelChanged;
    public Action<int> OnHpChanged;
}
