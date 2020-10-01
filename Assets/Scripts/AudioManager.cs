using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong;

    public static AudioManager Instance = null;

    AudioSource _audioSource;

    private void Awake()
    {
        #region Singleton Pattern (Simple)
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }
    private void Start()
    {
        if (_startingSong != null)
        {
            AudioManager.Instance.PlaySong(_startingSong);
        }
    }
    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
