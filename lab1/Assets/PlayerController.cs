using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required to check if clicking UI

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 playerVelocity;
    private float xRotation = 0f;

    [Header("Settings")]
    public float speed = 5.0f;
    public float lookSensitivity = 0.1f;
    public float gravityValue = -9.81f;

    [Header("References")]
    public GameObject playerCamera;
    public Vector3 respawnPoint;
    public Button respawnButton;

    public AudioSource shootSFX; 
    public GameObject bulletPrefab;
public Transform firePoint; // A point at the front of the camera

    void Start()
    {
        controller = GetComponent<CharacterController>();
        respawnPoint = transform.position;

        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(Respawn);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }
    public void OnLook(InputValue value) { lookInput = value.Get<Vector2>(); }

    void Update()
    {
        // 1. ESC to unlock
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // 2. ONLY relock if we click the game world, NOT a UI button
        if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState == CursorLockMode.None)
        {
            // This is the fix! It checks if the mouse is NOT over a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // 3. Camera Look
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = lookInput.x * lookSensitivity;
            float mouseY = lookInput.y * lookSensitivity;
            transform.Rotate(Vector3.up * mouseX);
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // 4. Movement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = respawnPoint;
        controller.enabled = true;
    }

    // Use this to handle the actual shooting logic in one place
void Shoot()
{
    if (bulletPrefab != null && firePoint != null)
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        if (shootSFX != null) 
        {
            shootSFX.Play(); 
            Debug.Log("Sound is playing!"); // Check the console for this!
        }
        else
        {
            Debug.LogWarning("ShootSFX is missing in the Inspector!");
        }
    }
}

// These are the "listeners" for the Input System
public void OnAttack(InputValue value)
{
    if (value.isPressed) Shoot();
}

public void OnFire(InputValue value)
{
    if (value.isPressed) Shoot();
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DeathPlane") { Respawn(); }
    }
}