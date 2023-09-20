using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Best : MonoBehaviour
{
    TMP_Text m_textComponent;

    int highscore;
    private void Awake()
    {
        m_textComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore");
        m_textComponent.text = "Best: " + highscore;
    }

    public void UpdateBest(int score)
    {
        if (score > highscore)
        {
            
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            highscore = score;
            m_textComponent.text = "Best: " + highscore;
        }
    }

}
