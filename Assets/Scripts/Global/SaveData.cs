using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HowDoYouFeel.Global
{
    [Serializable]
    public class SaveData
    {
        public int saveDataVersion;
        public bool introductionFinished, focusGameFinished, museumGameFinished, wordsInWordsGameFinished, geniusGameFinished, conclusionFinished;

        public SaveData(bool _introduction, bool _focusGame, bool _museumGame, bool _wordsInWordsGame, bool _geniusGame, bool _conclusion)
        {
            saveDataVersion = GlobalManager.Instance.saveDataVersion;
            introductionFinished = _introduction;
            focusGameFinished = _focusGame;
            museumGameFinished = _museumGame;
            wordsInWordsGameFinished = _wordsInWordsGame;
            geniusGameFinished = _geniusGame;
            conclusionFinished = _conclusion;
        }

        public static SaveData GenerateNewSaveData()
        {
            string path = Application.persistentDataPath + "/MG_ReadBeforeDeleting.txt";
            bool mus = File.Exists(path);
            return new SaveData(false, false, mus, false, false, false);
        }
    }
}
