using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
    private bool shouldJump => characterController.isGrounded && Input.GetKeyDown(jumpKey);

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pause;

    public bool hasKey = false;
    private bool isDead = false;
    private bool isPaused;
    public GameObject checkpoint;
    public bool isChaseable;
    private Vector3 Checkpoint;
    private AudioSource adsrc;

    [Header("Control")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Movement Parameters")]
    [SerializeField] private float walkspeed = 3.0f;
    [SerializeField] private float sprintspeed = 6.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float LookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float LookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float UpperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float LowerLookLimit = 80.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    private Camera playerCamera;
    private CharacterController characterController;
    private Animator anim;

    private Vector3 MoveDirection;
    private Vector2 CurrentInput;

    private float rotationX;
    private void OnEnable()
    {
        Menus.OnClicked += Respawn;
    }

    private void OnDisable()
    {
        Menus.OnClicked -= Respawn;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        adsrc = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameOver.SetActive(false);
        pause.SetActive(false);
    }

    void Update()
    {
        if(CanMove)
        {
            HandleMovement();
            HandleMouseLook();
            if (canJump)
                HandleJump();
            ApplyFinalMovement();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead) //Pause Menu
        {
            if (!isPaused)
            {
                pause.SetActive(true);
                CanMove = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                isPaused = true;
            }
            else
            {
                pause.SetActive(false);
                CanMove = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPaused = false;
            }
        }
    }

    private void HandleMovement() //Handle Player Movement
    {
        CurrentInput = new Vector2((isSprinting ? sprintspeed : walkspeed) * Input.GetAxis("Vertical"), (isSprinting ? sprintspeed : walkspeed) * Input.GetAxis("Horizontal"));
        float moveDirectionY = MoveDirection.y;
        MoveDirection = (transform.TransformDirection(Vector3.forward) * CurrentInput.x) + (transform.TransformDirection(Vector3.right) * CurrentInput.y);
        MoveDirection.y = moveDirectionY;
    
    }

    private void HandleMouseLook() // Handle Mouse movement
    {
        rotationX -= Input.GetAxis("Mouse Y") * LookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -UpperLookLimit, LowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeedX, 0); 
    }

    private void HandleJump() // Handles Player jump
    {
        if (shouldJump)
            MoveDirection.y = jumpForce;
    }

    private void ApplyFinalMovement() // Applies all the movement  values calculated above
    {
        if(!characterController.isGrounded)
        {
            MoveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(MoveDirection * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.gameObject.CompareTag("Key")) //Getting key
        {
            Destroy(hit.collider.gameObject);
            hasKey = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController")) //Checkpoint
        {
            isChaseable = true;
            Checkpoint = transform.position;
        }
        if(other.gameObject.CompareTag("Hand") && !isDead) //If hit by the monster
        {
            OverMenu();
            anim.enabled = true;
            anim.SetTrigger("Dead");
            isDead = true;
        }
    }

    public void OverMenu()
    {
        StartCoroutine(Dying());
    }

    IEnumerator Dying() //Dying
    {
        adsrc.Play();
        isDead = true;
        anim.Play("Dying");
        yield return new WaitForSeconds(1f);
        gameOver.SetActive(true);
        CanMove = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        yield return null;
    }

    void Respawn() //Respawn script for checkpoint
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameOver.SetActive(false);
        Moving();
        CanMove = true;
        isDead = false;
    }

    void Moving() //Changing player transform to checkpoint transform
    {
        characterController.enabled = false;
        characterController.transform.position = Checkpoint;
        characterController.transform.rotation = Quaternion.identity;
        anim.enabled = false;
        characterController.enabled = true;
    }
}
