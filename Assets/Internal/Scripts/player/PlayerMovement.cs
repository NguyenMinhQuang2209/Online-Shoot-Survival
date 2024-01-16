using Cinemachine;
using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private NetworkVariable<FixedString128Bytes> username = new NetworkVariable<FixedString128Bytes>();

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform hand;
    [SerializeField] private TextMeshProUGUI usernameTxt;
    float currentSpeed = 0f;
    private Animator animator;
    private Transform character;
    float plusSpeed = 0f;

    private PlayerHealth playerHealth;
    private PlayerLight playerLight;

    bool isDie = false;

    bool triggerDead = false;

    // Shooting
    float currentTimeBwtAttack = 0f;
    private Weapon currentWeapon = null;

    public override void OnNetworkSpawn()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerLight = GetComponent<PlayerLight>();
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
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

            if (gameController != null && gameController.TryGetComponent<GameController>(out var gameC))
            {
                ChangeUsernameServerRpc(gameC.playerName);
            }
            usernameTxt.gameObject.SetActive(false);

        }
        else
        {
            virtualCamera.enabled = false;
            virtualCamera.Priority = 0;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeUsernameServerRpc(string name)
    {
        username.Value = name;
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
        if (!IsOwner)
        {
            usernameTxt.text = username.Value.ToString();
        }
        if (isDie)
        {
            GetComponent<Collider2D>().enabled = false;
            if (IsOwner)
            {
                if (!triggerDead)
                {
                    if (PreferenceController.instance != null)
                    {
                        PlayerDeadController playerDeadController = PreferenceController.instance.playerDeadController;
                        if (playerDeadController != null)
                        {
                            playerDeadController.PlayerDead();
                        }
                    }
                    triggerDead = true;
                }
                if (animator != null)
                {
                    animator.SetTrigger("Dead");
                }
            }
            return;
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
    public string GetUserName()
    {
        return username.Value.ToString();
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
            currentWeapon.Shoot(NetworkObjectId);
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
        if (PreferenceController.instance != null)
        {
            SpawnPlayerController spawnPlayer = PreferenceController.instance.spawnPlayerController;
            if (spawnPlayer != null)
            {
                if (!spawnPlayer.StartGame())
                {
                    animator.SetFloat("Speed", 0f);
                    animator.SetBool("Run", false);
                    return;
                }
            }
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

    public void MovementToPosition(Vector2 newPos)
    {
        if (IsOwner)
        {
            rb.MovePosition(newPos);
        }
    }
    public bool PlayerDie()
    {
        return isDie;
    }

    public CinemachineVirtualCamera GetMainCamera()
    {
        return virtualCamera;
    }
    public void ChangeLightStatus(bool v)
    {
        playerLight.SetUpDieLight(v);
    }
}
