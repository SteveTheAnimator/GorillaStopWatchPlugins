using HarmonyLib;
using GorillaStats;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace GorillaStopWatch.Patches
{
    [HarmonyPatch(typeof(GorillaStats.Main))]
    [HarmonyPatch("Update", MethodType.Normal)]
    internal class NoMoreUpdateIThink
    {
        static float elapsedTime = 0f;
        static bool isRunning = false;

        static float primaryCooldown = 0f;
        static float secondaryCooldown = 0f;
        const float cooldownDuration = 1f;

        static bool Prefix(GorillaStats.Main __instance)
        {
            if (primaryCooldown > 0f) primaryCooldown -= Time.deltaTime;
            if (secondaryCooldown > 0f) secondaryCooldown -= Time.deltaTime;

            bool primaryPressed = ControllerInputPoller.instance.rightControllerPrimaryButton;
            bool secondaryPressed = ControllerInputPoller.instance.rightControllerSecondaryButton;

            if (primaryPressed && primaryCooldown <= 0f || Keyboard.current.sKey.wasPressedThisFrame)
            {
                isRunning = !isRunning;
                if(isRunning)
                {
                    Plugin.instance.RunEvent("Start");
                }
                else
                {
                    Plugin.instance.RunEvent("Stop");
                }
                primaryCooldown = cooldownDuration;
            }

            if (secondaryPressed && secondaryCooldown <= 0f || Keyboard.current.aKey.wasPressedThisFrame)
            {
                elapsedTime = 0f;
                isRunning = false;
                secondaryCooldown = cooldownDuration;
                Plugin.instance.RunEvent("Stop");
            }

            if (isRunning)
            {
                elapsedTime += Time.deltaTime;
            }

            if (__instance.watchText != null)
            {
                TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
                __instance.watchText.text = $"StopWatch:\n{time:mm\\:ss\\:ff}";
            }

            return false;
        }
    }
}
