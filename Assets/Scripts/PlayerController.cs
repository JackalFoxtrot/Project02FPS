using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool paused = false;
    private float _timeToAdd;
    private float baseSpeed = 12f;

    public CharacterController controller;
    public Level01Controller _level01Controller;

    [Header("Player Settings")]
    [SerializeField] AudioClip _playerDamaged;
    [SerializeField] AudioClip _playerDeath;
    
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
    [SerializeField] Text _weaponText;
    [SerializeField] Slider _weaponBar;
    [SerializeField] ParticleSystem _particleSystemToEnable;
    [SerializeField] AudioClip _weaponNoise;
    [SerializeField] AudioClip _weaponHitNoise;
    [SerializeField] Camera _cameraController;
    [SerializeField] MouseLook _mouseLook;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] float _shootDistance = 24f;
    [SerializeField] GameObject _hitEffect;
    [SerializeField] int _weaponDamage = 1;
    [SerializeField] float _weaponCoolDown = 1f;
    [SerializeField] float _weaponTimer;
    
    [SerializeField] LayerMask hitLayers;
    RaycastHit objectHit;

    [Header("Secondary Ability Settings")]
    [SerializeField] GameObject _shieldObject;
    [SerializeField] Text _shieldText;
    [SerializeField] Slider _shieldBar;
    [SerializeField] float _shieldCoolDown = 1f;
    [SerializeField] float _shieldTimer;
    private bool _shieldActive = false;


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
        if (!paused)
        {
            _timeToAdd = Time.deltaTime;
            _weaponTimer += _timeToAdd;
            _shieldTimer += _timeToAdd;
            if (_weaponTimer >= _weaponCoolDown)
            {
                _weaponTimer = _weaponCoolDown;
            }
            updateTextBars(_weaponBar, _weaponText, _weaponTimer);

            if (_shieldTimer >= _shieldCoolDown)
            {
                _shieldTimer = _shieldCoolDown;
            }
            updateTextBars(_shieldBar, _shieldText, _shieldTimer);

            if (Input.GetMouseButtonDown(0) && _weaponTimer >= _weaponCoolDown && !_shieldActive)
            {
                Debug.Log("LeftMouseButtonPressed");
                Shoot();
                _particleSystemToEnable.Play();
                AudioHelper.PlayClip2D(_weaponNoise, 0.25f);
                _weaponTimer = 0f;
                updateTextBars(_weaponBar, _weaponText, _weaponTimer);
            }

            if (Input.GetMouseButtonDown(1) && _shieldTimer >= _shieldCoolDown)
            {
                Debug.Log("RightMouseButtonPressed");
                Shield();
                //_particleSystemToEnable.Play();
                //AudioHelper.PlayClip2D(_weaponNoise, 0.25f);
                _shieldTimer = 0f;
                updateTextBars(_shieldBar, _shieldText, _shieldTimer);
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

    void updateTextBars(Slider sliderToUpdate, Text textToUpdate, float valueToUpdate)
    {
        textToUpdate.text = (int)(valueToUpdate * sliderToUpdate.maxValue) + " / " + sliderToUpdate.maxValue;
        sliderToUpdate.value = (valueToUpdate * 100);
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
            if (objectHit.transform.tag == "Enemy" || objectHit.transform.tag == "Boss")
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

    void Shield()
    {
        _shieldObject.SetActive(true);
        _shieldActive = true;
        DelayHelper.DelayAction(this, InvertShield, 0.5f);        
    }

    void InvertShield()
    {
        _shieldObject.SetActive(false);
        _shieldActive = false;
    }

    public void DealDamage(float damageAmount)
    {
        _level01Controller.ResetMultiplier();
        if(currentPlayerHealth > 0 && currentPlayerHealth > damageAmount)
        {
            currentPlayerHealth -= damageAmount;
            _healthText.text = currentPlayerHealth + " / " + maxPlayerHealth;
            AudioHelper.PlayClip2D(_playerDamaged, 1f);
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
            AudioHelper.PlayClip2D(_playerDeath, 1f);
            _level01Controller.PlayerDeath();
        }
    }
    public void InvertPausedBool()
    {
        paused = !paused;
    }
    public bool GetPause()
    {
        return paused;
    }
}
