using System;
using CSharpDiscordWebhook.NET.Discord;
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
            _config ??= GlobalAPI.CoreAPI.LoadModConfig<NotifierConfig>(ConfigFilename);
            if (_config != null) return _config;
            
            _config = new NotifierConfig();
            GlobalAPI.CoreAPI.StoreModConfig(_config, ConfigFilename);

            return _config;
        }
    }

    private static DiscordWebhook? _webhook = null;
    public static DiscordWebhook? Webhook
    {
        get
        {
            if (_webhook == null && string.IsNullOrEmpty(Config.DiscordWebhookUri)) return null;
            return _webhook ??= new DiscordWebhook { Uri = new Uri(Config.DiscordWebhookUri) };
        }
    }

    private enum NotificationType { Info, Warning, Error, Critical }

    private static void Notify(NotificationType type, string content)
    {
        var emoji = type switch
        {
            NotificationType.Info => ":information_source:",
            NotificationType.Warning => ":warning:",
            NotificationType.Error => ":interrobang:",
            NotificationType.Critical =>  ":name_badge:",
            _ => ""
        };
        var mentions = type switch
        {
            NotificationType.Info => "",
            NotificationType.Warning => "<@&1336856485191356416>\t",
            NotificationType.Error => "<@&1336858530321404014>, <@&1336856485191356416>\t",
            NotificationType.Critical =>  "<@&1336858530321404014>, <@&1336856485191356416>\t",
            _ => ""
        };
        
        var message = new DiscordMessage
        {
            Content = mentions,
            Embeds =
            {
                new DiscordEmbed
                {
                    Timestamp = new DiscordTimestamp(DateTime.Now),
                    Title = $"{emoji} {type}",
                    Description = content
                }
            }
        };
        Webhook?.SendAsync(message);
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

    public static void NotifyInfo(string? message = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Info, message, null, typeName, methodName);
    public static void NotifyWarning(string? errorMessage = null, Exception? exception = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Warning, errorMessage, exception, typeName, methodName);
    public static void NotifyError(string? errorMessage = null, Exception? exception = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Error, errorMessage, exception, typeName, methodName);
    public static void NotifyCritical(string? errorMessage = null, Exception? exception = null, string? typeName = null, string? methodName = null) =>
        NotifyProcessor(NotificationType.Critical, errorMessage, exception, typeName, methodName);
}