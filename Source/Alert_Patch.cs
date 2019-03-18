//using Harmony;
//using RimWorld;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEngine;
//using Verse;
//using Verse.Sound;

//namespace Toggles.Patches
//{
//    [HarmonyPatch(typeof(Alert))]
//    [HarmonyPatch("DrawAt")]
//    class Alert_Patch
//    {
//        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
//            instructions
//                .MethodReplacer(GetLabel_Method, GetLabel_Proxy_Method)
//                .MethodReplacer(ButtonInvisible_Method, ButtonInvisible_Proxy_Method);

//        static Alert alert;

//        static MethodInfo GetLabel_Method { get; } = AccessTools.Method(typeof(Alert), "GetLabel");
//        static MethodInfo GetLabel_Proxy_Method { get; } = AccessTools.Method(typeof(Alert_Patch), "GetLabel_Proxy", new Type[] { typeof(Alert) });

//        static MethodInfo ButtonInvisible_Method { get; } = AccessTools.Method(typeof(Widgets), "ButtonInvisible", new Type[] { typeof(Rect), typeof(bool) });
//        static MethodInfo ButtonInvisible_Proxy_Method { get; } = AccessTools.Method(typeof(Alert_Patch), "ButtonInvisible_Proxy", new Type[] { typeof(Rect), typeof(bool) });

//        static string GetLabel_Proxy(Alert instance)
//        {
//            alert = instance;
//            return alert.GetLabel();
//        }

//        static bool ButtonInvisible_Proxy(Rect rect, bool doMouseoverSound = false)
//        {
//            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
//            {
//                SoundDefOf.Click.PlayOneShotOnCamera(null);
//                AlertsReadout_Patch.AddSleepingAlert(alert);
//            }

//            return GUI.Button(rect, string.Empty, Widgets.EmptyStyle);
//        }
//    }
//}