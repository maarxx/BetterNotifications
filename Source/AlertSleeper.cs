using BetterNotifications;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Toggles.Patches
{
    [HarmonyPatch(typeof(Alert))]
    [HarmonyPatch("DrawAt")]
    class AlertSleeper
    {
        internal AlertSleeper()
        {
            UIRoot_Play ui = (UIRoot_Play)Find.UIRoot;
            alertsReadout = ui.alerts;
        }

        static AlertsReadout alertsReadout;

        static List<Alert> GetAllAlerts()
        {
            return (List<Alert>)AccessTools.Field(typeof(AlertsReadout), "AllAlerts").GetValue(alertsReadout);
        }
        static List<Alert> GetActiveAlerts()
        {
            return (List<Alert>)AccessTools.Field(typeof(AlertsReadout), "activeAlerts").GetValue(alertsReadout);
        }

        // Removes an alert from all available alerts, and from all alerts currently active.
        static void RemoveAlert(Alert alert)
        {
            List<Alert> all = GetAllAlerts();
            all.Remove(alert);
            AccessTools.Field(typeof(AlertsReadout), "AllAlerts").SetValue(alertsReadout, all);

            List<Alert> active = GetActiveAlerts();
            active.Remove(alert);
            AccessTools.Field(typeof(AlertsReadout), "activeAlerts").SetValue(alertsReadout, active);
        }

        // Adds an alert to all available alerts, unless already in list.
        static void AddAlert(Alert alert)
        {
            List<Alert> list = GetAllAlerts();
            if (!list.Contains(alert))
                list.Add(alert);
            AccessTools.Field(typeof(AlertsReadout), "AllAlerts").SetValue(alertsReadout, list);
        }

        // Replaces an alert's GetLabel() to get Alert instance. Then, also replaces the invisible button over an alert with clickable one.
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            instructions
                .MethodReplacer(GetLabel_Method, GetLabel_Proxy_Method)
                .MethodReplacer(ButtonInvisible_Method, ButtonInvisible_Proxy_Method);

        // The clicked alert.
        static Alert alertObject;

        static MethodInfo GetLabel_Method { get; } = AccessTools.Method(typeof(Alert), "GetLabel");
        static MethodInfo GetLabel_Proxy_Method { get; } = AccessTools.Method(typeof(AlertSleeper), "GetLabel_Proxy", new Type[] { typeof(Alert) });

        static MethodInfo ButtonInvisible_Method { get; } = AccessTools.Method(typeof(Widgets), "ButtonInvisible", new Type[] { typeof(Rect), typeof(bool) });
        static MethodInfo ButtonInvisible_Proxy_Method { get; } = AccessTools.Method(typeof(AlertSleeper), "ButtonInvisible_Proxy", new Type[] { typeof(Rect), typeof(bool) });

        // List of all alerts currently sleeping.
        public static Dictionary<Alert, int> SleepingAlerts { get; set; } = new Dictionary<Alert, int>();

        static string GetLabel_Proxy(Alert instance)
        {
            alertObject = instance;
            return alertObject.GetLabel();
        }

        // When user right-clicks an alert
        static bool ButtonInvisible_Proxy(Rect rect, bool doMouseoverSound = false)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
            {
                SoundDefOf.Click.PlayOneShotOnCamera(null);

                // Put alert to sleep
                SleepAlert(alertObject);
            }

            return GUI.Button(rect, string.Empty, Widgets.EmptyStyle);
        }

        // Adds alert to list of sleeping alerts, and removes it from game's list of available alerts.
        static void SleepAlert(Alert alert)
        {
            if (!SleepingAlerts.ContainsKey(alert))
            {
                SleepingAlerts.Add(alert, Find.TickManager.TicksGame);
                RemoveAlert(alert);
            }
        }

        // Checks every tick whether an alert should stay asleep or be woken up.
        internal static void CheckIfSleeping()
        {
            if (SleepingAlerts.Count > 0)
            {
                for (int i = SleepingAlerts.Count - 1; i > -1; i--)
                {
                    if ((SleepingAlerts.Values.ToArray()[i] + (GenDate.TicksPerHour * Controller.AlertTime)) < Find.TickManager.TicksGame)
                    {
                        Alert alert = SleepingAlerts.Keys.ToArray()[i];
                        AddAlert(alert);
                        SleepingAlerts.Remove(alert);
                    }
                }
            }
        }
    }
}