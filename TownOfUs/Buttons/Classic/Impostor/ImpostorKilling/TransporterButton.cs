
using System;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Modules;
using UnityEngine;

namespace TownOfUs.Roles.Impostor;

public sealed class TransporterRole(IntPtr cppPtr)
    : ImpostorRole(cppPtr), ITownOfUsRole
{
    public string IdPart => "Transporter";

    public Color RoleColor => TownOfUsColors.Impostor;

    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;

    public RoleAlignment RoleAlignment =>
        RoleAlignment.ImpostorKilling;

    public CustomRoleConfiguration Configuration =>
        new(this)
        {
            CanUseVent = true,
            CanUseSabotage = true
        };

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
    }
}
