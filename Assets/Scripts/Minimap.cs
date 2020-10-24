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

    private float _minCameraDistance;

    public void Start()
    {
        _minCameraDistance = transform.position.y;
        camera.orthographicSize = PlayerPrefs.GetFloat("CurrentZoom");
        ZoomParameterCheck();
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y += _minCameraDistance;
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
