using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Toggles.Patches
{
    [HarmonyPatch(typeof(Alert))]
    [HarmonyPatch("DrawAt")]
    class Alert_Patch
    {
        internal Alert_Patch()
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

        static void RemoveAlert(Alert alert)
        {
            List<Alert> all = GetAllAlerts();
            all.RemoveAll(x => x == alert);
            AccessTools.Field(typeof(AlertsReadout), "AllAlerts").SetValue(alertsReadout, all);

            List<Alert> active = GetActiveAlerts();
            active.RemoveAll(x => x == alert);
            AccessTools.Field(typeof(AlertsReadout), "activeAlerts").SetValue(alertsReadout, active);
        }

        static void AddAlert(Alert alert)
        {
            List<Alert> list = GetAllAlerts();
            if (!list.Contains(alert))
                list.Add(alert);
            AccessTools.Field(typeof(AlertsReadout), "AllAlerts").SetValue(alertsReadout, list);
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            instructions
                .MethodReplacer(GetLabel_Method, GetLabel_Proxy_Method)
                .MethodReplacer(ButtonInvisible_Method, ButtonInvisible_Proxy_Method);

        static Alert alertObject;

        static MethodInfo GetLabel_Method { get; } = AccessTools.Method(typeof(Alert), "GetLabel");
        static MethodInfo GetLabel_Proxy_Method { get; } = AccessTools.Method(typeof(Alert_Patch), "GetLabel_Proxy", new Type[] { typeof(Alert) });

        static MethodInfo ButtonInvisible_Method { get; } = AccessTools.Method(typeof(Widgets), "ButtonInvisible", new Type[] { typeof(Rect), typeof(bool) });
        static MethodInfo ButtonInvisible_Proxy_Method { get; } = AccessTools.Method(typeof(Alert_Patch), "ButtonInvisible_Proxy", new Type[] { typeof(Rect), typeof(bool) });

        public static Dictionary<Alert, int> SleepingAlerts { get; set; } = new Dictionary<Alert, int>();

        static string GetLabel_Proxy(Alert instance)
        {
            alertObject = instance;
            return alertObject.GetLabel();
        }

        static bool ButtonInvisible_Proxy(Rect rect, bool doMouseoverSound = false)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
            {
                SoundDefOf.Click.PlayOneShotOnCamera(null);

                SleepAlert(alertObject);
            }

            return GUI.Button(rect, string.Empty, Widgets.EmptyStyle);
        }

        static void SleepAlert(Alert alert)
        {
            if (!SleepingAlerts.ContainsKey(alert))
            {
                SleepingAlerts.Add(alert, Find.TickManager.TicksGame);
                RemoveAlert(alert);
            }
        }

        internal static void CheckIfSleeping()
        {
            HashSet<Alert> wakeup = new HashSet<Alert>();
            foreach (KeyValuePair<Alert, int> sleeper in SleepingAlerts)
            {
                if ((sleeper.Value + (GenDate.TicksPerHour * 2)) < Find.TickManager.TicksGame)
                {
                    AddAlert(sleeper.Key);
                    wakeup.Add(sleeper.Key);
                }
            }
            foreach (Alert alert in wakeup)
                SleepingAlerts.Remove(alert);
        }
    }
}