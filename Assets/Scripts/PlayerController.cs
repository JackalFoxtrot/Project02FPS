using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool paused = false;

    public CharacterController controller;
    public Level01Controller _level01Controller;

    [Header("Player Settings")]
    [SerializeField] AudioClip _playerDamaged;
    [SerializeField] AudioClip _playerDeath;
    private float baseSpeed = 12f;
    public float speed = 12f;

    public Text _healthText;
    public Slider _healthBar;
    public float maxPlayerHealth = 100;
    public float currentPlayerHealth = 100;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    [Header("Primary Ability Settings")]
    [SerializeField] ParticleSystem _particleSystemToEnable;
    [SerializeField] AudioClip _weaponNoise;
    [SerializeField] AudioClip _weaponHitNoise;
    [SerializeField] Camera _cameraController;
    [SerializeField] MouseLook _mouseLook;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] float _shootDistance = 10f;
    [SerializeField] GameObject _hitEffect;
    [SerializeField] int _weaponDamage = 1;
    [SerializeField] LayerMask hitLayers;

    RaycastHit objectHit;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _healthText.text = currentPlayerHealth + " / " + maxPlayerHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InvertPausedBool();
        }
        if(!paused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("LeftMouseButtonPressed");
                Shoot();
                _particleSystemToEnable.Play();
                AudioHelper.PlayClip2D(_weaponNoise, 0.25f);
            }
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -9.81f;
            }
            if (isGrounded && Input.GetKey(KeyCode.LeftShift))
            {
                speed = baseSpeed * 2;
            }
            if (Input.GetButtonUp("Fire3"))
            {
                speed = baseSpeed;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        } 
    }
    void Shoot()
    {
        //calculate direction
        Vector3 rayDirection = _cameraController.transform.forward;
        //Cast debug ray
        Debug.DrawRay(_rayOrigin.position, rayDirection * _shootDistance, Color.red, 1f);
        //Do raycast
        if (Physics.Raycast(_rayOrigin.position, rayDirection, out objectHit, _shootDistance, hitLayers))
        {
            if (objectHit.transform.tag == "Enemy")
            {
                Debug.Log("You hit " + objectHit.transform.name + ": Tag: " + objectHit.transform.tag + ".");
                _hitEffect.transform.position = objectHit.point + (objectHit.normal * 0.2f);
                Debug.Log(_hitEffect.transform.position);
                _hitEffect.GetComponent<ParticleSystem>().Play();
                AudioHelper.PlayClip2D(_weaponHitNoise, 0.25f);
                objectHit.transform.gameObject.GetComponent<EnemyController>().TakeDamage(_weaponDamage);
            }
        }
        else
        {
            Debug.Log("Miss.");
        }
    }
    public void DealDamage(float damageAmount)
    {
        if(currentPlayerHealth > 0 && currentPlayerHealth > damageAmount)
        {
            currentPlayerHealth -= damageAmount;
            _healthText.text = currentPlayerHealth + " / " + maxPlayerHealth;
            AudioHelper.PlayClip2D(_playerDamaged, 0.25f);
        }
        else
        {
            currentPlayerHealth = 0;
            _healthText.text = currentPlayerHealth + " / " + maxPlayerHealth;
        }
        _healthBar.value = currentPlayerHealth;
        if(currentPlayerHealth == 0)
        {
            Debug.Log("Player has Died.");
            InvertPausedBool();
            _mouseLook.InvertPausedBool();
            AudioHelper.PlayClip2D(_playerDeath, 0.25f);
            _level01Controller.PlayerDeath();
        }
    }
    public void InvertPausedBool()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public bool GetPause()
    {
        return paused;
    }
}
