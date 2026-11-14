using System;

namespace Kralt.Framework.InputHandling
{
    /// <summary>
    /// Cursor states
    /// </summary>
    public enum CursorLockMode
    {
        /// <summary>
        /// The cursor visible and cursor motion is not limited.
        /// </summary>
        None,

        /// <summary>
        /// Hides the cursor when over a window.
        /// </summary>
        Hidden,

        /// <summary>
        /// Hides the cursor and locks it to the specified window.
        /// </summary>
        Locked,

        /// <summary>
        /// Confines the cursor to the window content area.
        /// </summary>
        Confined,
    }

    public static class Mouse
    {
        private static Action<CursorLockMode>? SetLockMode;
        private static Func<CursorLockMode>? GetLockMode;

        public static CursorLockMode lockState
        {
            get => GetLockMode?.Invoke() ?? CursorLockMode.None;
            set => SetLockMode?.Invoke(value);
        }

        internal static void Initialize(Action<CursorLockMode> set, Func<CursorLockMode> get)
        {
            SetLockMode = set;
            GetLockMode = get;
        }
    }
}
