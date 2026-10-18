using System;
using System.Collections.Generic;
using UnityEngine;
using Reactor.api.Attributes;
using TownOfUs.Roles; // Zakładając użycie bazy Town of Us

namespace TransporterRoleMod
{
    [ReactorPlugin("com.twojanazwa.transporter", "Transporter Role", "1.0.0")]
    public class TransporterPlugin : ReactorPlugin
    {
        // Konfiguracja umiejętności
        private const float CooldownDuration = 20f;
        private float _currentCooldown = 0f;
        
        // Nazwa roli
        public string RoleName => "Transporter";

        private void Update()
        {
            // Aktualizacja odliczania cooldownu
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.deltaTime;
            }

            // Sprawdzenie czy gracz użył przycisku umiejętności (np. klawisz F)
            if (Input.GetKeyDown(KeyCode.F) && _currentCooldown <= 0)
            {
                TeleportToNearestVent();
            }
        }

        private void TeleportToNearestVent()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            
            // Upewnij się, że gracz żyje i posiada tę rolę
            if (localPlayer == null || localPlayer.Data.IsDead) return;

            Vent nearestVent = null;
            float minDistance = float.MaxValue;
            Vector2 playerPos = localPlayer.transform.position;

            // Znajdź wszystkie wentylacje na mapie
            var allVents = UnityEngine.Object.FindObjectsOfType<Vent>();

            foreach (var vent in allVents)
            {
                float distance = Vector2.Distance(playerPos, vent.transform.position);
                
                // Szukamy najbliższego wenta (ale nie tego, przy którym gracz już stoi bardzo blisko)
                if (distance < minDistance && distance > 0.5f)
                {
                    minDistance = distance;
                    nearestVent = vent;
                }
            }

            // Teleportacja, jeśli znaleziono wentylację
            if (nearestVent != null)
            {
                localPlayer.transform.position = nearestVent.transform.position;
                
                // Resetowanie cooldownu do 20 sekund
                _currentCooldown = CooldownDuration;
                
                // Opcjonalnie: odtworzenie dźwięku wenta dla efektu
                if (PlayerControl.LocalPlayer.AmOwner)
                {
                    AudioSource.PlayClipAtPoint(nearestVent.EnterVentSound, localPlayer.transform.position);
                }
            }
        }
    }
}
