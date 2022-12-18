using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

[RequireComponent(typeof(Rigidbody))]
public class PlayerLocomotion : MonoBehaviour
{
    private Rigidbody rb;
    public bool GameOnPause;
    private Transform _cursorStartPoint;
    //[SerializeField] private GameObject _book;
    [SerializeField] private AudioSource _steps;

    #region Camera Settings

    public Camera playerCamera;
    public float fov = 60f;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    #endregion

    #region Crosshair Settings
    // Crosshair
    public Sprite crosshairImage;
    [SerializeField] private Image crosshairObject;
    public bool lockedCursor = true;
    public bool crosshair = true;
    #endregion

    #region Movement Settings

    public bool isWalking = false;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    #endregion

    #region Head Bob

    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(0f, .1f, 0f);
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera.fieldOfView = fov;
        jointOriginalPos = joint.localPosition;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lockedCursor = true;
        if (crosshair)
        {
            crosshairObject.sprite = crosshairImage;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameOnPause == false)
        {
            HandleCameraRotate();
        }

       //if (Input.GetKeyDown(KeyCode.Escape))
       //{
       //    GamePauseHandler(GameOnPause);
       //}

    }

    private void FixedUpdate()
    {
        if(GameOnPause == false)
        {
            HandlePlayerMoving();
            
        }
    }
    private void LateUpdate()
    {
        if (GameOnPause == false)
        {
            HandleHeadBob();
        }
        
    }

    public void GamePauseHandler(bool locked)
    {
        if (GameOnPause == false)
        {
            Cursor.lockState = CursorLockMode.None;
            lockedCursor = false;
            GameOnPause = true;

            //_book.SetActive(true);
            _steps.Pause();
        }
        else if (GameOnPause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            lockedCursor = true;
            GameOnPause = false;
            //_book.SetActive(false);
        }
    }

    public void StepPause()
    {
        if (_steps.isPlaying)
        {
            _steps.Pause();
        }
    }

    private void HandlePlayerMoving()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (targetVelocity.x != 0 || targetVelocity.z != 0)
        {
            isWalking = true;
            if(_steps.isPlaying == false)
            {
                _steps.Play();
            }
            
        }
        else
        {
            isWalking = false;
            if (_steps.isPlaying )
            {
                _steps.Pause();
            }
        }
        targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    private void HandleCameraRotate()
    {
        if (lockedCursor)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

    }

    private void HandleHeadBob()
    {
        if (isWalking)
        {
            timer += Time.deltaTime * bobSpeed;
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }

    }


}
