using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject _gameObjectToTransform;
    [SerializeField] Level01Controller _levelController;
    [SerializeField] ParticleSystem _particleSystemToEnable;
    [SerializeField] GameObject _bulletModel;
    [SerializeField] AudioClip _projectileSpawn;
    [SerializeField] AudioClip _enemyDeath;
    [SerializeField] AudioClip _enemyDamaged;
    [SerializeField] AudioClip _enemyWeaponShot;
    [SerializeField] GameObject _bulletPosition1;
    [SerializeField] GameObject _bulletPosition2;

    [SerializeField] int _maximumHealth = 5;
    [SerializeField] int _currentHealth;
    [SerializeField] int _scoreToAdd = 100;

    float _fireTimer = 3f;
    float _timeSinceLastFire = 0f;
    private ParticleSystem particleSystem2;
    
    private void Awake()
    {
        _currentHealth = _maximumHealth;
        if(this.gameObject.CompareTag("Boss"))
        {
            particleSystem2 = Instantiate(_particleSystemToEnable);
        }
        
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
                AudioHelper.PlayClip2D(_enemyDeath, 0.25f);
                if(this.gameObject.CompareTag("Boss"))
                {
                    Debug.Log("Boss");
                    SpawnBullet(_bulletPosition1.transform, _bulletPosition2.transform);
                }
                else
                {
                    Debug.Log("Not a Boss");
                    SpawnBullet();
                }
                
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
        _particleSystemToEnable.Play();
        AudioHelper.PlayClip2D(_projectileSpawn, 0.25f, 1.25f);
    }

    private void SpawnBullet(Transform spawnPos1, Transform spawnPos2)
    {
        float spawnDistance = 2;
        Vector3 enemyPosition = transform.position;
        Vector3 spawnPosition = spawnPos1.position + transform.forward * spawnDistance;
        Vector3 spawnPosition2 = spawnPos2.position + transform.forward * spawnDistance;

        Quaternion spawnRotation = spawnPos1.rotation;
        Quaternion spawnRotation2 = spawnPos2.rotation;

        GameObject newBullet = GameObject.Instantiate(_bulletModel, spawnPosition, spawnRotation);
        newBullet.GetComponent<BulletBehavior>().enabled = true;
        newBullet.SetActive(true);
        _particleSystemToEnable.Play();

        GameObject newBullet2 = GameObject.Instantiate(_bulletModel, spawnPosition2, spawnRotation2);
        newBullet2.GetComponent<BulletBehavior>().enabled = true;
        newBullet2.SetActive(true);
        
        particleSystem2.transform.position = spawnPos2.position;
        particleSystem2.Play();
        
        AudioHelper.PlayClip2D(_projectileSpawn, 0.25f, 1.25f);
    }

    public void TakeDamage(int damage)
    {
        if(_currentHealth > 0)
        {
            AudioHelper.PlayClip2D(_enemyDamaged, 0.25f);
            _currentHealth -= damage;
        }
        if(_currentHealth <= 0)
        {
            AudioHelper.PlayClip2D(_enemyDeath, 0.25f);
            _levelController.IncreaseScore(_scoreToAdd);
            _levelController.IncreaseMultiplier();
            if(this.CompareTag("Boss"))
            {
                _levelController.Victory();

            }
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
