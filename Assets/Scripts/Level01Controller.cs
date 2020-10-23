using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;
    [SerializeField] Text _currentMultiplierTextView;
    [SerializeField] GameObject _ingamePanel;
    [SerializeField] GameObject _ingameDeathPanel;
    [SerializeField] GameObject _ingameVictoryPanel;
    [SerializeField] GameObject _miniMapController;
    [SerializeField] GameObject _crosshair;
    private int _scoreMultiplier = 1;

    int _currentScore;

    private void Start()
    {
        ResumeLevel();
        _currentMultiplierTextView.text = "Current Multiplier: " + _scoreMultiplier.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_ingamePanel.gameObject.activeSelf)
            {
                ResumeLevel();
            }
            else
            {
                PauseLevel();
            }
            if(_ingameDeathPanel.gameObject.activeSelf || _ingameVictoryPanel.gameObject.activeSelf)
            {
                ExitLevel();
            }
        }
        if(Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            _miniMapController.GetComponent<Minimap>().ZoomIn();
        }
        if(Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Underscore))
        {
            _miniMapController.GetComponent<Minimap>().ZoomOut();
        }
    }
    public void ResumeLevel()
    {
        Time.timeScale = 1;
        LockCursor();
        HideCursor();
        UnhideCrosshair();
        _ingamePanel.SetActive(false);
    }
    public void PauseLevel()
    {
        Time.timeScale = 0;
        UnlockCursor();
        UnhideCursor();
        HideCrosshair();
        _ingamePanel.SetActive(true);
    }
    public void PlayerDeath()
    {
        UnlockCursor();
        UnhideCursor();
        _ingameDeathPanel.SetActive(true);
    }
    public void ExitLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void CheckHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");

        if (_currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", _currentScore);
            Debug.Log("New High Score: " + _currentScore);
        }
    }
    
    public void Victory()
    {
        Time.timeScale = 0;
        UnlockCursor();
        UnhideCursor();
        HideCrosshair();
        CheckHighScore();
        _ingameVictoryPanel.SetActive(true);
    }

    public void IncreaseScore(int scoreIncrease)
    {
        _currentScore += (scoreIncrease * _scoreMultiplier);
        _currentScoreTextView.text = "Current Score: " + _currentScore.ToString();
    }
    public void IncreaseMultiplier()
    {
        _scoreMultiplier++;
        _currentMultiplierTextView.text = "Current Multiplier: " + _scoreMultiplier.ToString();
    }
    public void ResetMultiplier()
    {
        _scoreMultiplier = 1;
        _currentMultiplierTextView.text = "Current Multiplier: " + _scoreMultiplier.ToString();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }
    public void UnhideCursor()
    {
        Cursor.visible = true;
    }
    public void HideCrosshair()
    {
        _crosshair.SetActive(false);
    }
    public void UnhideCrosshair()
    {
        _crosshair.SetActive(true);
    }
}
