using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance { get; private set; }

        public string currentControlScheme = "Keyboard";

        public int saveDataVersion = 0;

        public bool IntroductionFinished { get; private set; }
        public bool FocusGameFinished { get; private set; }
        public bool MuseumGameFinished { get; private set; }
        public bool WordsInWordsGameFinished { get; private set; }
        public bool GeniusGameFinished { get; private set; }
        public bool ConclusionFinished { get; private set; }

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

        #region updating saved values
        public void UpdateIntroductionFinished(bool _isFinished)
        {
            IntroductionFinished = _isFinished;
            Save();
        }

        public void UpdateMuseumGameFinished(bool _isFinished)
        {
            MuseumGameFinished = _isFinished;
            Save();
        }

        public void UpdateFocusGameFinished(bool _isFinished)
        {
            FocusGameFinished = _isFinished;
            Save();
        }

        public void UpdateWordsInWordsGameFinished(bool _isFinished)
        {
            WordsInWordsGameFinished = _isFinished;
            Save();
        }

        public void UpdateGeniusGameFinished(bool _isFinished)
        {
            GeniusGameFinished = _isFinished;
            Save();
        }

        public void UpdateConclusionFinished(bool _isFinished)
        {
            ConclusionFinished = _isFinished;
            Save();
        }
        #endregion


        #region saving/loading
        public void Save()
        {
            SaveData data = new SaveData(IntroductionFinished, FocusGameFinished, MuseumGameFinished, WordsInWordsGameFinished, GeniusGameFinished, ConclusionFinished);
            SaveSaveData(data);
        }

        public void Load()
        {
            SaveData data = LoadSaveData();
            IntroductionFinished = data.introductionFinished;
            MuseumGameFinished = data.museumGameFinished;
            WordsInWordsGameFinished = data.wordsInWordsGameFinished;
            GeniusGameFinished = data.geniusGameFinished;
            FocusGameFinished = data.focusGameFinished;
            ConclusionFinished = data.conclusionFinished;
        }

        private void SaveSaveData(SaveData data) //what a terrible name to give a method
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = GetPath();

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        private SaveData LoadSaveData()
        {
            string path = GetPath();

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                SaveData result = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                return result;
            }

            return SaveData.GenerateNewSaveData();
        }

        public void ResetData()
        {
            SaveSaveData(SaveData.GenerateNewSaveData());
            Load();
        }

        public string GetPath()
        {
#if UNITY_EDITOR
            string path = Application.persistentDataPath + "/SaveData_Editor.sav";
#else
            string path = Application.persistentDataPath + "/SaveData.sav";
#endif
            return path;
        }
        #endregion
    }
}
