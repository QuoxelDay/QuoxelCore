using Vintagestory.Client;

namespace QuoxelCore;

public static class QuoxelClientCore
{
    public static ServerConnectData? RedirectedFrom { get; private set; }
    public static string? RedirectReason { get; private set; }

    public static void RedirectTo(string host)
    {
        if (ClientProgram.screenManager == null) return;

        if (ClientProgram.screenManager.CurrentScreen is not GuiScreenRunningGame gameScreen) return;

        gameScreen.connectData = ServerConnectData.FromHost(host);
        gameScreen.runningGame.doReconnect = true;
    }
}