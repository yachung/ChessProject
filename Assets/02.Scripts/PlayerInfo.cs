using Fusion;

public struct PlayerInfo : INetworkStruct
{
    public int Index;
    public int Hp;
    public NetworkString<_32> Name;
    public PlayerRef PlayerRef;
}