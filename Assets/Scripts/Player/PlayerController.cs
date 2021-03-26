using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    Travel,
    Combat
}

[RequireComponent(typeof(PlayerInput), typeof(Animator), typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public LayerMask InteractionLayers = -1;
    public Weapon WeaponPrefab;
    public Transform WeaponSocket;
    public Transform Spine;
    public Transform AimCamera;
    public CinemachineVirtualCameraBase VirtualCamera;
    public GameObject Crosshair;
    public Vector2 VerticalClamp = new Vector2(-30f, 40f);
    public float RotationSpeed = 1f;
    public float CameraYCorrection = 70f;

    private PlayerInput playerInput;
    private Animator animator;
    private NavMeshAgent agent;
    private Camera cam;
    private float verticalAngle;
    private float horizontalAngle;
    private float spineAngleCorrection = 10f;
    private Weapon weapon;

    public PlayerState CurrentPlayerState { get; private set; } = PlayerState.Travel;

    public float CurrentSpeed => agent.velocity.magnitude;

    private void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        cam = Camera.main;
        weapon = Instantiate(WeaponPrefab, WeaponSocket);

        LoadPlayerSettings();
    }

    private void Update()
    {
        if (playerInput.EnterCombatState())
        {
            CurrentPlayerState = CurrentPlayerState == PlayerState.Travel ? PlayerState.Combat : PlayerState.Travel;
            SwitchPlayerState();
        }

        if (CurrentPlayerState == PlayerState.Combat)
        {
            HandleFireInput();
        }

    }

    private void LateUpdate()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.Travel:
                HandleMovingInput();
                break;
            case PlayerState.Combat:
                HandleAimingInput();
                break;
        }
    }

    private void LoadPlayerSettings()
    {
        var settings = Resources.Load<PlayerSettings>("PlayerSettings");

        agent.speed = settings.PlayerSpeed;
    }

    private void SwitchPlayerState()
    {
        if (CurrentPlayerState == PlayerState.Combat)
        {
            animator.SetLayerWeight(1, 1);
            VirtualCamera.Priority += 1;
            Crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            animator.SetLayerWeight(1, 0);
            VirtualCamera.Priority -= 1;
            Crosshair.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleAimingInput()
    {
        verticalAngle -= playerInput.GetVertical() * RotationSpeed * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, VerticalClamp.x, VerticalClamp.y);
        horizontalAngle = playerInput.GetHorizontal() * RotationSpeed * Time.deltaTime;

        Spine.localEulerAngles = new Vector3(verticalAngle + spineAngleCorrection, 0, 0);

        this.transform.Rotate(Vector3.up, horizontalAngle);
        AimCamera.transform.Rotate(Vector3.up, horizontalAngle);
        AimCamera.localEulerAngles = new Vector3(verticalAngle, this.transform.eulerAngles.y - CameraYCorrection, 0);

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)), out var hit, float.PositiveInfinity, InteractionLayers))
            weapon.Muzzle.LookAt(hit.point);
    }

    private void HandleMovingInput()
    {
        if (playerInput.GetMoveInput())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, float.PositiveInfinity, InteractionLayers))
                agent.SetDestination(hit.point);
        }

        animator.SetFloat("Speed", CurrentSpeed);
    }

    private void HandleFireInput()
    {
        if (playerInput.GetFireInputDown())
            animator.SetBool("Firing", true);

        if (playerInput.GetFireInput())
            weapon.HandleShoot();

        if (playerInput.GetFireInputUp())
            animator.SetBool("Firing", false);
    }
}
