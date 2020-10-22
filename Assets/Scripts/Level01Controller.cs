﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;
    [SerializeField] GameObject _ingamePanel;
    [SerializeField] GameObject _ingameDeathPanel;
    [SerializeField] GameObject _miniMapController;

    int _currentScore;

    private void Start()
    {
        LockCursor();
        HideCursor();
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
            if(_ingameDeathPanel.gameObject.activeSelf)
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
        LockCursor();
        HideCursor();
        _ingamePanel.SetActive(false);
    }
    public void PauseLevel()
    {
        UnlockCursor();
        UnhideCursor();
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
}
