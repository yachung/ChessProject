using Fusion;

public struct PlayerInfo : INetworkStruct
{
    public NetworkString<_32> Name;

    public PlayerInfo(string name)
    {
        Name = name;
    }
}