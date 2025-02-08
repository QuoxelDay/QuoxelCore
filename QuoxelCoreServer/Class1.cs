using System;
using Vintagestory.API.Common;

namespace QuoxelCoreServer;

public class QuoxelCoreServer : ModSystem
{
    public override void StartPre(ICoreAPI api)
    {
        api.Logger.Debug("LOADED MY CUSTOM QUOXEL_CORE_SERVER");
        QuoxelNotifier.NotifyInfo("This is a test message.");
        QuoxelNotifier.NotifyWarning("And another", new Exception("Some random exception!"));
        QuoxelNotifier.NotifyError("Finally, only one more", typeName: nameof(QuoxelCoreServer), methodName: nameof(StartPre));
        QuoxelNotifier.NotifyCritical("Oh well");
    }
}