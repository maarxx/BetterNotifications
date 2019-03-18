using Harmony;
using System;
using System.Linq;
using System.Reflection;
using Toggles.Hotkeys;
using Toggles.Source;
using Toggles.Source.Patches;
using Verse;

namespace Toggles
{
    [StaticConstructorOnStartup]
    class TogglesController
    {
        static TogglesController()
        {
            int startTime = Environment.TickCount;

            // Initialize Toggles that determine whether or not methods should be patched.
            InitPatchToggles();

            // Initialize mod settings. Since only toggles for method patches have been initialized so far, only those are loaded.
            ModHandler.ReadSettings();

            // Initialize all patches set as active in mod settings. This also initializes all toggles related to each patch.
            InitPatches();

            // Initialize mod key bindings.
            HotkeyHandler.InitHotkeys();

            // Re-read mod settings so that all toggles get their correct values.
            ModHandler.ReReadSettings();

            DebugUtil.Log("STARTUP TIME: " + (Environment.TickCount - startTime).ToString() + "ms");
        }

        // Initialize Toggles that determine whether or not methods should be patched.
        static void InitPatchToggles()
        {
            // Classes inheriting MethodPatch represent all patches. Creating a toggle for each to only patch methods player has set as active later.
            foreach (Type patch in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(MethodPatch))))
            {
                string name = Labels.Toggles + patch.Name;
                ToggleManager.Add(
                    label: name.Translate(),
                    id: name,
                    description: (name + Labels.Description).Translate(),
                    root: Labels.Internal,
                    category: Labels.Patches
                    );
            }
        }

        // Initialize all patches set as active in mod settings. This also initializes all toggles related to each patch.
        static void InitPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("Toggles");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}