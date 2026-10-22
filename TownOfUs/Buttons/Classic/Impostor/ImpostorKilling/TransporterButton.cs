
using Mira.CustomButtons;
using Mira.Assets;
using TownOfUs.Modules;
using TownOfUs.Roles.Impostor;
using UnityEngine;

namespace TownOfUs.Buttons.Impostor;

[RegisterButton]
public sealed class TransporterButton : CustomActionButton
{
    public override string Name => "Transporter";

    public override float Cooldown => 20f;

    public override float EffectDuration => 0f;

    public override int MaxUses => 0;

    public override LoadableAsset<Sprite> Sprite =>
        TouImpAssets.PursueSprite;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is TransporterRole;
    }

    protected override void OnClick()
    {
        PlayerControl player = PlayerControl.LocalPlayer;

        if (player == null || player.Data == null ||
            player.Data.IsDead || ShipStatus.Instance == null)
        {
            return;
        }

        Vent nearestVent = null;
        float nearestDistance = float.MaxValue;

        foreach (Vent vent in ShipStatus.Instance.AllVents)
        {
            if (vent == null)
                continue;

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
            return;

        player.NetTransform.RpcSnapTo(
            nearestVent.transform.position);
    }
}


