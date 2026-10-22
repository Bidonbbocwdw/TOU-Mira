
using MiraAPI.Hud;
using TownOfUs.Modules;
using TownOfUs.Roles.Impostor;
using UnityEngine;

namespace TownOfUs.Buttons.Impostor;

public sealed class TransporterButton : TownOfUsRoleButton<TransporterRole>
{
    public override string Name =>
        MiraLocaleManager.Get("TownOfUsMira.Role.Transporter", "Teleport");

    public override BaseKeybind Keybind => Keybinds.SecondaryAction;

    public override Color TextOutlineColor => TownOfUsColors.Impostor;

    public override float Cooldown => 20f;

    public override LoadableAsset<Sprite> Sprite => TouImpAssets.PursueSprite;

    public override bool CanUse()
    {
        return base.CanUse()
            && PlayerControl.LocalPlayer != null
            && !PlayerControl.LocalPlayer.Data.IsDead
            && ShipStatus.Instance != null;
    }

    protected override void OnClick()
    {
        PlayerControl player = PlayerControl.LocalPlayer;

        if (player == null || player.Data == null || player.Data.IsDead)
        {
            return;
        }

        Vent? nearestVent = null;
        float nearestDistance = float.MaxValue;

        foreach (Vent vent in ShipStatus.Instance.AllVents)
        {
            if (vent == null)
            {
                continue;
            }

            float distance = Vector2.Distance(
                player.transform.position,
                vent.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestVent = vent;
            }
        }

        if (nearestVent == null)
        {
            return;
        }

        player.NetTransform.RpcSnapTo(nearestVent.transform.position);
    }
}
