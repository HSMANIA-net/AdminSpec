using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace AdminSpec;

public class AdminSpecConfig : BasePluginConfig
{
    [JsonPropertyName("flag")]
    public string Flag { get; set; } = "@css/ban";
}

public class AdminSpec : BasePlugin, IPluginConfig<AdminSpecConfig>
{
    public override string ModuleName => "AdminSpec";
    public override string ModuleVersion => "1.0.1";
    public override string ModuleAuthor => "unfortunate";

    public AdminSpecConfig Config { get; set; } = new();

    public override void Load(bool hotReload)
    {
        AddCommandListener("jointeam", OnPlayerJoinTeam, HookMode.Pre);
    }

    public void OnConfigParsed(AdminSpecConfig config)
    {
        Config = config;
    }

    private HookResult OnPlayerJoinTeam(CCSPlayerController? player, CommandInfo info)
    {
        if (!IsPlayerValid(player))
            return HookResult.Continue;

        var desiredTeam = info.ArgByIndex(1);
        if (desiredTeam == "1")
        {
            if (!AdminManager.PlayerHasPermissions(player, Config.Flag))
            {
                // TODO: Add sound
                player?.PrintToChat(Localizer["OnlyAdmin"]);
                return HookResult.Handled;
            }
        }

        return HookResult.Continue;
    }

    #region Helpers
    public static bool IsPlayerValid(CCSPlayerController? player)
    {
        return player != null
            && player.IsValid
            && !player.IsBot
            && player.Pawn != null
            && player.Pawn.IsValid
            && player.Connected == PlayerConnectedState.PlayerConnected
            && !player.IsHLTV;
    }
    #endregion
}
