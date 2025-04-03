using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float moveSpeed;
    private CharacterController controller;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;
    public float cameraCrouchOffset = 0.5f;
    private bool isCrouching = false;

    [Header("Stamina Settings")]
    public float maxStamina = 7f;
    public float staminaRechargeSpeed = 2f;
    private float currentStamina;
    private bool isSprinting;
    private bool isCooldown;

    [Header("Mouse Look")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;

    private UIManager uiManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        uiManager = FindObjectOfType<UIManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveSpeed = walkSpeed;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleSprint();
        HandleCrouch();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleSprint()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = moveX != 0 || moveZ != 0;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCooldown && !isCrouching && isMoving)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
            currentStamina -= Time.deltaTime;
            uiManager.UpdateSprintBar(currentStamina / maxStamina, isSprinting);
        }
        else
        {
            isSprinting = false;
            moveSpeed = isCrouching ? crouchSpeed : walkSpeed;

            if (currentStamina < maxStamina && !isCooldown)
            {
                currentStamina += Time.deltaTime * staminaRechargeSpeed;
                uiManager.UpdateSprintBar(currentStamina / maxStamina, isSprinting);
            }
        }

        if (currentStamina <= 0 && !isCooldown)
        {
            isCooldown = true;
            uiManager.ShowCooldownBar();
            Invoke(nameof(ResetSprintCooldown), 5f);
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                controller.height = crouchHeight;
                playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, cameraCrouchOffset, playerCamera.localPosition.z);
                moveSpeed = crouchSpeed;
            }
        }
        else if (isCrouching)
        {
            isCrouching = false;
            controller.height = standingHeight;
            playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, 1f, playerCamera.localPosition.z);
            moveSpeed = walkSpeed;
        }
    }

    void ResetSprintCooldown()
    {
        isCooldown = false;
        uiManager.HideCooldownBar();
        currentStamina = maxStamina;
        uiManager.UpdateSprintBar(1f, isSprinting);
    }
}



