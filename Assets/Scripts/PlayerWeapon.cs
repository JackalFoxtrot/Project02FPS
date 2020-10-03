﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public ParticleSystem _particleSystemToEnable;
    public AudioClip _weaponNoise;

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InvertPausedBool();
        }
        if (Input.GetMouseButtonDown(0) && !paused)
        {
            Debug.Log("LeftMouseButtonPressed");
            _particleSystemToEnable.Play();
            AudioHelper.PlayClip2D(_weaponNoise, 0.5f);
        }
    }
    public void InvertPausedBool()
    {
        paused = !paused;
    }
}