using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

namespace SpaceBaboon
{
    public class SaveData : MonoBehaviour
    {
        public Score m_score = new Score();

        public void SaveJson()
        {
            string ScoreData = JsonUtility.ToJson(m_score);
            string ScoreFilePath = Application.persistentDataPath + "/ScoreData.json";
            Debug.Log("ScoreFilePath");
            System.IO.File.WriteAllText(ScoreFilePath,ScoreData);
            Debug.Log("Saved Done");
        }

        public void LoadJson()
        {
            string ScoreFilePath = Application.persistentDataPath + "/ScoreData.json";
            string ScoreData =  System.IO.File.ReadAllText(ScoreFilePath);
            m_score = JsonUtility.FromJson<Score>(ScoreData);
            Debug.Log("Reload Completed");

        }
        

        [System.Serializable]
        public class Score
        {
            public int HighScoreData;
            public int LastScoreData;
            public string HighScoreUser;
            public string LastScoreUser;
        }

        public void SetCurrentScore(int _LastScoreData, string _LastScoreUser)
        {
            m_score.LastScoreData = _LastScoreData;
            m_score.LastScoreUser = _LastScoreUser;

            if (_LastScoreData > m_score.HighScoreData)
            {
                m_score.HighScoreData = _LastScoreData;
                m_score.HighScoreUser = _LastScoreUser;
            }
        }
        
        public Score GetSavedScoreVariables()
        {
            return m_score;
        }
    }
}
