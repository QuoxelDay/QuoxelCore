using Vintagestory.API.Common;

namespace QuoxelCore;

public class QuoxelCoreClient : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;
    public override void StartPre(ICoreAPI api)
    {
        api.Logger.Debug("LOADED MY CUSTOM QUOXEL_CORE_CLIENT");
    }
}