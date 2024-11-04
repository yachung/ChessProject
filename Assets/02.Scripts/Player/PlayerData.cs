using System;

public class PlayerData
{
    public string Name { get; set; }
    public int Hp 
    {
        get => this.Hp;
        set
        {
            this.Hp = value;
            OnHpChanged(Hp);
        }
    }

    public int Level
    {
        get => this.Level;
        set
        {
            this.Level = value;
            OnLevelChanged(Level);
        }
    }
    public int Gold
    {
        get => this.Gold;
        set
        {
            this.Gold = value;
            OnGoldChanged(Gold);
        }
    }
    public int Exp
    {
        get => this.Exp;
        set
        {
            this.Exp = value;
            OnExperienceChanged(Exp);
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
