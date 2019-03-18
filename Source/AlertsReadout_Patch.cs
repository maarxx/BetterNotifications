//using Harmony;
//using RimWorld;
//using System;
//using System.Collections.Generic;
//using Toggles.Source;
//using Toggles.Source.Patches;
//using Verse;

//namespace Toggles.Patches
//{
//    [HarmonyPatch(typeof(AlertsReadout))]
//    [HarmonyPatch("AlertsReadoutUpdate")]
//    class AlertsReadout_Patch : MethodPatch
//    {
//        static bool Prepare()
//        {
//            return Prepare(typeof(AlertsReadout_Patch));
//        }

//        static void InitToggles()
//        {
//            foreach (Type type in typeof(Alert).AllLeafSubclasses())
//            {
//                Alert alert = (Alert)Activator.CreateInstance(type);
//                if (!Alerts.Contains(alert))
//                    Alerts.Add(alert);
//                Alerts.Add(alert);
//                string label;
//                try
//                {
//                    label = alert.GetLabel();
//                }
//                catch (NullReferenceException)
//                {
//                    label = type.Name;
//                    if (!label.CanTranslate())
//                        label = label.Replace("Alert_", string.Empty);
//                }
//                string explanation;
//                try
//                {
//                    explanation = alert.GetExplanation();
//                }
//                catch (NullReferenceException)
//                {
//                    explanation = type.Name + Labels.Description;
//                    if (explanation.CanTranslate())
//                        explanation = explanation.Translate();
//                    else
//                        explanation = "NoDescription".Translate();
//                }

//                ToggleManager.Add(
//                    label: label,
//                    id: Format(alert),
//                    description: explanation,
//                    texture: Tex.TexEmpty,
//                    root: Labels.Notifications,
//                    category: Labels.Alerts,
//                    subCategory: alert.Priority.ToString()
//                    );
//            }
//        }

//        static List<Alert> Alerts { get; } = new List<Alert>();
//        static Dictionary<string, int> SleepingAlerts { get; } = new Dictionary<string, int>();

//        internal static void AddSleepingAlert(Alert alert)
//        {
//            if (!SleepingAlerts.ContainsKey(Format(alert)))
//                SleepingAlerts.Add(Format(alert), Find.TickManager.TicksGame);
//        }

//        static string Format(Alert alert) => alert.GetType().Name; // Adding Labels.Toggles as prefix here causes massive slowdown.

//        internal static int hourMultiplier = 2;

//        static void Postfix(ref List<Alert> ___AllAlerts, ref List<Alert> ___activeAlerts)
//        {
//            foreach (Alert alert in Alerts)
//            {
//                string label = Format(alert);

//                if (SleepingAlerts.ContainsKey(label))
//                {
//                    if (Find.TickManager.TicksGame - SleepingAlerts[label] < GenDate.TicksPerHour * hourMultiplier)
//                    {
//                        RemoveAlert(ref ___AllAlerts, ref ___activeAlerts, label);
//                    }
//                    else
//                    {
//                        AddAlert(ref ___AllAlerts, ref ___activeAlerts, alert);
//                    }
//                }
//                else if (!ToggleManager.IsActive(label))
//                {
//                    RemoveAlert(ref ___AllAlerts, ref ___activeAlerts, label);
//                }
//                else
//                {
//                    AddAlert(ref ___AllAlerts, ref ___activeAlerts, alert);
//                }
//            }
//        }

//        static void AddAlert(ref List<Alert> ___AllAlerts, ref List<Alert> ___activeAlerts, Alert alert)
//        {
//            if (!___AllAlerts.Exists(x => x.GetType().Name.Equals(Format(alert))))
//                ___AllAlerts.Add((Alert)Activator.CreateInstance(alert.GetType()));
//            SleepingAlerts.Remove(Format(alert));
//        }

//        static void RemoveAlert(ref List<Alert> ___AllAlerts, ref List<Alert> ___activeAlerts, string label)
//        {
//            ___AllAlerts.RemoveAll(x => x.GetType().Name.Equals(label));
//            ___activeAlerts.RemoveAll(x => x.GetType().Name.Equals(label));
//        }
//    }
//}