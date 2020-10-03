using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    public float _damageAmount = 15;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

        playerController.DealDamage(_damageAmount);
        Debug.Log(playerController.ToString() + " has entered " + this.ToString());
    }
}
