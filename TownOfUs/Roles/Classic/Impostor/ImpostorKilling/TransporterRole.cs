
using System.Collections;
using UnityEngine;

public class TransporterAbility : MonoBehaviour
{
    public float cooldown = 20f;
    private float nextUseTime = 0f;

    public bool CanUse()
    {
        return Time.time >= nextUseTime;
    }

    public bool TeleportToNearestVent(
        Transform player,
        Transform[] vents)
    {
        if (!CanUse() || player == null || vents == null)
            return false;

        Transform nearestVent = null;
        float nearestDistance = float.MaxValue;

        foreach (Transform vent in vents)
        {
            if (vent == null)
                continue;

            float distance = Vector2.Distance(
                player.position, vent.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestVent = vent;
            }
        }

        if (nearestVent == null)
            return false;

        player.position = nearestVent.position;
        nextUseTime = Time.time + cooldown;

        return true;
    }
}
