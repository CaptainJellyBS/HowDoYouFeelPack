using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance { get; private set; }

        public string currentControlScheme = "Keyboard";

        #region cursor sheninigans
        bool cursorVisible, mouseActive;

        public bool CursorVisible
        {
            get { return cursorVisible; }
            set { cursorVisible = value; UpdateCursorVisibility(); }
        }

        public bool MouseActive
        {
            get { return mouseActive; }
            set { mouseActive = value; UpdateCursorVisibility(); }
        }

        #endregion

        private void Awake()
        {
            if (Instance != null) { Destroy(this); return; }
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void UpdateCursorVisibility()
        {
            Cursor.visible = CursorVisible && MouseActive;
        }
    }
}
