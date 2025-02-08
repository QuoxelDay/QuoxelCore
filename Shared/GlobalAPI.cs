﻿using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client;
using Vintagestory.Server;

namespace QuoxelCore;

public static class GlobalAPI
{
    public static EnumAppSide Side
    {
        get
        {
            if (ClientProgram.screenManager != null)
                return EnumAppSide.Client;
            if (ServerProgram.server != null)
                return EnumAppSide.Server;
            return EnumAppSide.Universal;
        }
    }

    public static ICoreAPI CoreAPI =>
        Side switch
        {
            EnumAppSide.Client => ClientProgram.screenManager.api,
            EnumAppSide.Server => ServerProgram.server.api,
            _ => null
        };
}