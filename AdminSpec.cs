using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Admin;
using Microsoft.Extensions.Localization;

namespace AdminSpec
{
    public class AdminSpec : BasePlugin
    {
        public override string ModuleName => "AdminSpec";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "unfortunate";

        public override void Load(bool hotReload)
        {
            AddCommandListener("jointeam", OnPlayerJoinTeam, HookMode.Pre);
        }

        private HookResult OnPlayerJoinTeam(CCSPlayerController player, CommandInfo info)
        {
            if (player == null) return HookResult.Continue;

            var desiredTeam = info.ArgByIndex(1);
            if(desiredTeam == "1") {
                bool hasPermission = AdminManager.PlayerHasPermissions(player, "@css/ban");
                if(!hasPermission)
                {
                    // TODO: Add sound
                    player.PrintToChat(Localizer["OnlyAdmin"]);
                    return HookResult.Handled;
                }
            }
            
            return HookResult.Continue;
        }
    }
}