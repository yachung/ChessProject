using Fusion;

public struct PlayerInfo
{
    public int Index;
    public string Name;
    public string UserId;
    //public PlayerRef PlayerRef;
}

public struct NetworkPlayerInfo : INetworkStruct
{
    public int Index;
    public NetworkString<_16> Name;
    public NetworkString<_16> UserId;

    //public NetworkPlayerInfo(PlayerInfo playerInfo)
    //{
    //    Index = playerInfo.Index;
    //    Name = playerInfo.Name;
    //    UserId = playerInfo.UserId;
    //}
}