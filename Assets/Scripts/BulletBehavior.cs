using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 40f;
    [SerializeField] int _bulletDamage = 10;
    [SerializeField] PlayerController _playerController1;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] AudioClip _projectileHit;

    float _bulletDuration = 4;

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        DelayHelper.DelayAction(this, DestroyBullet, _bulletDuration);
    }
    private void FixedUpdate()
    {
        if (this.isActiveAndEnabled && !_playerController1.GetPause())
        {
            MoveBullet();
        }
    }
    void MoveBullet()
    {
        float moveAmountThisFrame = _moveSpeed;

        Vector3 moveDirection = transform.forward * moveAmountThisFrame;

        _rb.AddForce(moveDirection);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if(playerController != null)
        {
            playerController.DealDamage(_bulletDamage);
            AudioHelper.PlayClip2D(_projectileHit, 0.25f);
            DestroyBullet();
        }
        if(other.transform.tag == "Ground")
        {
            AudioHelper.PlayClip2D(_projectileHit, 0.25f);
            DestroyBullet();
        }
        Debug.Log("Tag: " + other.transform.tag);
        
    }
    void DestroyBullet()
    {
        Object.Destroy(this.gameObject);
    }
}
