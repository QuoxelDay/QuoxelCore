using Vintagestory.API.Common;

namespace QuoxelCore;

public class QuoxelCoreServer : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;
    public override void StartPre(ICoreAPI api)
    {
        api.Logger.Debug("LOADED MY CUSTOM QUOXEL_CORE_SERVER");
    }
}