using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject _gameObjectToTransform;
    [SerializeField] GameObject _bulletModel;

    [SerializeField] int _maximumHealth = 5;
    [SerializeField] int _currentHealth;

    float _fireTimer = 3f;
    float _timeSinceLastFire = 0f;
    
    private void Awake()
    {
        _currentHealth = _maximumHealth;
    }

    private void OnTriggerStay(Collider other)
    {
       PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
       if(playerController != null && !playerController.GetPause() &&playerController.currentPlayerHealth > 0)
       {
            _timeSinceLastFire += Time.deltaTime;
            _gameObjectToTransform.transform.LookAt(playerController.transform.position);
            if(_timeSinceLastFire >= _fireTimer)
            {
                _timeSinceLastFire = 0f;
                SpawnBullet();
            }
       }
    }

    private void SpawnBullet()
    {
        float spawnDistance = 2;
        Vector3 enemyPosition = transform.position;
        Vector3 spawnPosition = enemyPosition + transform.forward * spawnDistance;

        Quaternion spawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        GameObject newBullet = GameObject.Instantiate(_bulletModel, spawnPosition, spawnRotation);
        newBullet.GetComponent<BulletBehavior>().enabled = true;
        newBullet.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if(_currentHealth > 0)
        {
            _currentHealth -= damage;
        }
        if(_currentHealth <= 0)
        {
            DestroyObject();
        }
    }
    public int GetHealth()
    {
        return _currentHealth;
    }
    void DestroyObject()
    {
        Object.Destroy(_gameObjectToTransform);
    }
}
