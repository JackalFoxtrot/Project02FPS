using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    [SerializeField] GameObject _teleporterReciever;
    public void OnTriggerEnter(Collider other)
    {
        GameObject playerController = other.gameObject;
        
        Vector3 playerPosition = playerController.transform.position;
        playerPosition = _teleporterReciever.transform.position;
        playerPosition.y += 6;
        other.transform.position = playerPosition;
    }
}
