using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;

    int _currentScore;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(5);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitLevel();
        }
    }
    public void ExitLevel()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        
        if(_currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", _currentScore);
            Debug.Log("New High Score: " + _currentScore);
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void IncreaseScore(int scoreIncrease)
    {
        _currentScore += scoreIncrease;
        _currentScoreTextView.text = "Current Score: " + _currentScore.ToString();
    }
}
