using QuoxelCore;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace QuoxelCoreServer;

public class QuoxelCoreServer : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override void StartPre(ICoreAPI api)
    {
        QuoxelNotifier.LoginDiscordClient().Wait();
        
        QuoxelNotifier.NotifyInfo($"{GlobalAPI.ServerName} is online.");
        if (api is not ServerCoreAPI serverCoreAPI) return;
        serverCoreAPI.eventapi.PlayerChat += OnPlayerChat;
        serverCoreAPI.eventapi.PlayerJoin += OnPlayerJoin;
        serverCoreAPI.eventapi.PlayerLeave += OnPlayerLeave;
    }

    private void OnPlayerLeave(IServerPlayer player) => 
        QuoxelNotifier.NotifySimple($"[{GlobalAPI.ServerName}] ({player.PlayerUID}): {player.PlayerName} left.");

    private void OnPlayerJoin(IServerPlayer player) => 
        QuoxelNotifier.NotifySimple($"[{GlobalAPI.ServerName}] ({player.PlayerUID}): {player.PlayerName} joined!");

    private void OnPlayerChat(IServerPlayer player, int channelid, ref string message, ref string data, BoolRef consumed)
    {
        var finalMessage = message.Replace("<strong>", "").Replace("</strong>", "");
        QuoxelNotifier.NotifySimple($"[{GlobalAPI.ServerName}] ({player.PlayerUID}): {finalMessage}");
    }

    public override void Dispose()
    {
        QuoxelNotifier.NotifyInfo($"{GlobalAPI.ServerName} has disposed mods (likely gone offline).");
    }
}