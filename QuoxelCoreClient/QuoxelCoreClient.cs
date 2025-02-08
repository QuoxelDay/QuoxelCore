using Vintagestory.API.Common;

namespace QuoxelCoreClient;

public class QuoxelCoreClient : ModSystem
{
    public override void StartPre(ICoreAPI api)
    {
        api.Logger.Debug("LOADED MY CUSTOM QUOXEL_CORE_CLIENT");
    }
}