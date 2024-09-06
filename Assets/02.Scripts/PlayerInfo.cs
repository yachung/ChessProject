using Fusion;

public struct PlayerInfo : INetworkStruct
{
    public int Index;
    public NetworkString<_32> Name;
}