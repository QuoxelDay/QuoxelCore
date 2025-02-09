using System;
using QuoxelCore;

namespace QuoxelCoreServer;

public static class QuoxelNotifier
{
    public class NotifierConfig
    {
        public string DiscordWebhookUri { get; set; } = "";
    }

    private static NotifierConfig? _config = null;
    private static readonly string ConfigFilename = $"{nameof(QuoxelNotifier)}.config.json";
    public static NotifierConfig Config
    {
        get
        {
            _config ??= GlobalAPI.CoreAPI?.LoadModConfig<NotifierConfig>(ConfigFilename);
            if (_config != null) return _config;
            
            _config = new NotifierConfig();
            GlobalAPI.CoreAPI?.StoreModConfig(_config, ConfigFilename);

            return _config;
        }
    }

    
    
    
    
    
    
    private enum NotificationType { Simple, Info, Warn, Error, Crash }

    private static void Notify(NotificationType type, string content)
    {
        var emoji = type switch
        {
            NotificationType.Info => ":information_source:",
            NotificationType.Warn => ":warning:",
            NotificationType.Error => ":interrobang:",
            NotificationType.Crash =>  ":name_badge:",
            NotificationType.Simple => "",
            _ => ""
        };
        var mentions = type switch
        {
            NotificationType.Info => "",
            NotificationType.Warn => "<@&1336856485191356416>\t",
            NotificationType.Error => "<@&1336858530321404014>, <@&1336856485191356416>\t",
            NotificationType.Crash =>  "<@&1336858530321404014>, <@&1336856485191356416>\t",
            NotificationType.Simple => "",
            _ => ""
        };
        //
        // var message = new DiscordMessage
        // {
        //     Content = mentions
        // };
        // message.Content = 
        //     type == NotificationType.Simple ? 
        //         content : $"{emoji} **{type}**\n`{content.Trim('\n')}` {mentions}";
        //
        // Webhook?.SendAsync(message);
    }

    private static void NotifyProcessor(NotificationType type, string? errorMessage = null, Exception? exception = null,
        string? typeName = null, string? methodName = null)
    {
        var content = errorMessage == null ? "" : $"{errorMessage} ";
        content += typeName == null ? "" : $"[{typeName}] ";
        content += methodName == null ? "" : $"::{methodName} ";
        content += (content.Length > 0 ? "\n" : "") + exception;
        Notify(type, content);
    }

    public static void NotifySimple(string message) =>
        Notify(NotificationType.Simple, $"```autohotkey\n{message}\n```");
    public static void NotifyInfo(string? message = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Info, message, null, typeName, methodName);
    public static void NotifyWarning(string? errorMessage = null, Exception? exception = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Warn, errorMessage, exception, typeName, methodName);
    public static void NotifyError(string? errorMessage = null, Exception? exception = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Error, errorMessage, exception, typeName, methodName);
    public static void NotifyCritical(string? errorMessage = null, Exception? exception = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Crash, errorMessage, exception, typeName, methodName);
}