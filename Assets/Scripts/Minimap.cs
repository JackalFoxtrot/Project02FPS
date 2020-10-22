using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    public Camera camera;

    public float _increments = 5f;
    public float _maxZoom = 30f;
    public float _minZoom = 5f;
    public float _currentZoom = 15f;

    public void Start()
    {
        if(PlayerPrefs.GetFloat("CurrentZoom") >= _minZoom && PlayerPrefs.GetFloat("CurrentZoom") <= _maxZoom)
        {
            _currentZoom = PlayerPrefs.GetFloat("CurrentZoom");
        }
        camera.orthographicSize = _currentZoom;
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    public void ZoomIn()
    {
        camera.orthographicSize += _increments;
        ZoomParameterCheck();
    }

    public void ZoomOut()
    {
        camera.orthographicSize -= _increments;
        ZoomParameterCheck();
    }

    public void ZoomParameterCheck()
    {
        if(camera.orthographicSize > _maxZoom)
        {
            camera.orthographicSize = _maxZoom;
        }
        if (camera.orthographicSize < _minZoom)
        {
            camera.orthographicSize = _minZoom;
        }
        PlayerPrefs.SetFloat("CurrentZoom", camera.orthographicSize);
    }
}
