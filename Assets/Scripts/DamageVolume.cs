using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    public float _damageAmount = 15;

    public float _damageCoolDown = 1f;
    public float _damageTimer = 0f;
    
    private void OnTriggerStay(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        _damageTimer += Time.deltaTime;
        if(_damageTimer >= _damageCoolDown)
        {
            playerController.DealDamage(_damageAmount);
            Debug.Log(playerController.ToString() + " has entered " + this.ToString());
            _damageTimer = 0;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        playerController.DealDamage(_damageAmount);
        Debug.Log(playerController.ToString() + " has entered " + this.ToString());
    }
    public void OnTriggerExit(Collider other)
    {
        _damageTimer = 0;
    }
}
