using Cinemachine;
using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform hand;
    float currentSpeed = 0f;
    private Animator animator;
    private Transform character;
    float plusSpeed = 0f;

    private PlayerHealth playerHealth;

    bool isDie = false;


    // Shooting
    float currentTimeBwtAttack = 0f;
    private Weapon currentWeapon = null;

    public override void OnNetworkSpawn()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Animator>(out animator))
            {
                character = child;
                break;
            }
        }
        if (CameraCompoundController.instance != null)
        {
            if (virtualCamera.TryGetComponent<CinemachineConfiner2D>(out var compound))
            {
                compound.m_BoundingShape2D = CameraCompoundController.instance.compoundCamera;
            }
        }

        if (SceneController.instance != null)
        {
            SceneController.instance.ChangeSceneEvent += HandleChangeSceneEvent;
        }

        if (IsOwner)
        {
            virtualCamera.enabled = true;
            virtualCamera.Priority = 1;

        }
        else
        {
            virtualCamera.enabled = false;
            virtualCamera.Priority = 0;
        }
    }

    private void HandleChangeSceneEvent(object sender, EventArgs e)
    {
        if (CameraCompoundController.instance != null)
        {
            if (virtualCamera.TryGetComponent<CinemachineConfiner2D>(out var compound))
            {
                compound.m_BoundingShape2D = CameraCompoundController.instance.compoundCamera;
            }
        }

        if (IsOwner)
        {
            if (HealthManaTxtController.instance != null)
            {
                playerHealth.HealthSliderConfig(HealthManaTxtController.instance.healthSlider,
                    HealthManaTxtController.instance.manaSlider,
                    HealthManaTxtController.instance.healthTxt,
                    HealthManaTxtController.instance.manaTxt);
            }
        }
    }
    private void Update()
    {
        if (isDie)
        {
            GetComponent<Collider2D>().enabled = false;
            if (IsOwner)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Dead");
                }
            }
        }
        if (playerHealth.GetCurrentHealth() == 0)
        {
            isDie = true;
            return;
        }

        if (!IsOwner)
        {
            return;
        }

        HandRotate();
        Shooting();
    }
    private void Shooting()
    {
        if (currentWeapon == null)
        {
            return;
        }

        currentTimeBwtAttack += Time.deltaTime;
        if (currentTimeBwtAttack >= currentWeapon.GetTimeBwtAttack())
        {
            currentTimeBwtAttack = 0f;
            currentWeapon.Shoot();
        }
    }
    private void FixedUpdate()
    {
        if (isDie)
        {
            return;
        }

        if (!IsOwner)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Movement(new(horizontal, vertical));
    }
    private void Movement(Vector2 input)
    {

        if (GameController.instance != null && !GameController.instance.CanMove())
        {
            return;
        }

        currentSpeed = moveSpeed + plusSpeed;
        bool running = false;
        if (input.sqrMagnitude >= 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (playerHealth.UseMana())
                {
                    currentSpeed = runSpeed + plusSpeed;
                    running = true;
                }
            }
            character.rotation = Quaternion.Euler(0f, input.x < 0f ? 180f : 0f, 0f);
            rb.MovePosition(rb.position + currentSpeed * Time.fixedDeltaTime * input.normalized);
        }
        if (animator != null)
        {
            animator.SetFloat("Speed", input.sqrMagnitude >= 0.1f ? 1f : 0f);
            animator.SetBool("Run", running);
        }
    }
    private void HandRotate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetPos = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        hand.transform.rotation = Quaternion.Euler(new(0f, 0f, angle));
    }
    public void ObjectDie()
    {
        isDie = true;
    }
    public void Equipment(Weapon weapon)
    {
        foreach (Transform child in hand)
        {
            Destroy(child.gameObject);
        }
        if (weapon != null)
        {
            currentTimeBwtAttack = 0f;
            currentWeapon = Instantiate(weapon, hand.transform);
        }
    }
    public void ChangePlusSpeed(float value)
    {
        plusSpeed = value;
    }
}
