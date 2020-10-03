using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem _particleSystemToEnable;
    public AudioClip _weaponNoise;
    public MouseLook _mouseLook;
    public Text _healthText;
    public Slider _healthBar;

    private bool paused = false;

    public CharacterController controller;
    public Level01Controller _level01Controller;

    private float baseSpeed = 12f;
    public float speed = 12f;
    public float maxPlayerHealth = 100;
    public float currentPlayerHealth = 100;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

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
    public void DealDamage(float damageAmount)
    {
        if(currentPlayerHealth > 0 && currentPlayerHealth > damageAmount)
        {
            currentPlayerHealth -= damageAmount;
            _healthText.text = currentPlayerHealth + " / " + maxPlayerHealth;
        }
        else
        {
            currentPlayerHealth = 0;
            _healthText.text = currentPlayerHealth + " / " + maxPlayerHealth;
        }
        _healthBar.value = currentPlayerHealth;
        if(currentPlayerHealth == 0)
        {
            InvertPausedBool();
            _mouseLook.InvertPausedBool();
            _level01Controller.PlayerDeath();
        }
    }
    public void InvertPausedBool()
    {
        paused = !paused;
    }
}
