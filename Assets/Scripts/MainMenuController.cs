using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Text _highScoreTextView;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }
    public int GetScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }
    public void UpdateScore()
    {
        _highScoreTextView.text = GetScore().ToString();
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        UpdateScore();
    }
}
