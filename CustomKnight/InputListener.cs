using CustomKnight.NewUI;
using InControl;
using UnityEngine;

namespace CustomKnight
{
    internal static class InputListener
    {
        private static Coroutine inputCoroutine;
        internal static void Start()
        {
            inputCoroutine = CoroutineHelper.GetRunner().StartCoroutine(ListenForInput());
        }

        private static IEnumerator ListenForInput()
        {
            while (true)
            {
                if (GameManager.instance != null && GameManager.instance.isPaused)
                {
                    if (IsOpenSkinListPressed())
                    {
                        UIController.ToggleSkinList();
                    }
                }

                if (CustomKnight.GlobalSettings.Keybinds.ReloadSkins.WasPressed)
                {
                    BetterMenu.ReloadSkins();
                }

                yield return new WaitForEndOfFrame();
            }

        }

        private static bool IsOpenSkinListPressed()
        {
            if (CustomKnight.GlobalSettings.Keybinds.OpenSkinList.WasPressed)
            {
                return true;
            }

            var binding = CustomKnight.GlobalSettings.Keybinds.OpenSkinList.GetKeyOrMouseBinding();
            if (binding.Key != Key.None)
            {
                if (System.Enum.TryParse(binding.Key.ToString(), out KeyCode keyCode))
                {
                    return Input.GetKeyDown(keyCode) || IsMacDeleteKeyDown(keyCode);
                }
            }

            return Input.GetKeyDown(KeyCode.Delete) || IsMacDeleteKeyDown(KeyCode.Delete);
        }

        // Mac keyboards label Backspace "delete" and only send Delete with fn, so on macOS
        // accept that key for a Delete binding.
        private static bool IsMacDeleteKeyDown(KeyCode boundKey)
        {
            return boundKey == KeyCode.Delete
                && Application.platform == RuntimePlatform.OSXPlayer
                && Input.GetKeyDown(KeyCode.Backspace);
        }
    }
}
