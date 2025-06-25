using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;
    public float momentumDampening = 2f;
    public float touchSensitivity = 0.1f;

    private float yaw = 0f;
    private float pitch = 0f;
    private float lastMouseX = 0f;
    private float lastMouseY = 0f;
    private float momentumX = 0f;
    private float momentumY = 0f;
    private bool isDragging = false;
    private float dragThreshold = 0.05f;
    private float momentumThreshold = 0.2f;

    private MoveToClick playerScript;

    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
                playerScript = playerObj.GetComponent<MoveToClick>();
            }
        }
        else
        {
            playerScript = target.GetComponent<MoveToClick>();
        }

        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }
        else
        {
            Debug.LogError("CameraOrbit: No target assigned and Player not found!");
        }
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }

        if (!isDragging)
        {
            yaw += momentumX * Time.deltaTime;
            pitch -= momentumY * Time.deltaTime;
            momentumX = Mathf.Lerp(momentumX, 0, Time.deltaTime * momentumDampening);
            momentumY = Mathf.Lerp(momentumY, 0, Time.deltaTime * momentumDampening);
        }

        if (target != null)
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            transform.position = target.position - (rotation * Vector3.forward * 5f);
            transform.LookAt(target);
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = false;
            momentumX = 0f;
            momentumY = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(mouseX) > dragThreshold || Mathf.Abs(mouseY) > dragThreshold)
            {
                isDragging = true;
                playerScript?.SetMovementAllowed(false);
            }

            if (isDragging)
            {
                yaw += mouseX * rotationSpeed;
                pitch -= mouseY * rotationSpeed;
                pitch = Mathf.Clamp(pitch, -30f, 80f);
            }

            lastMouseX = mouseX;
            lastMouseY = mouseY;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging && (Mathf.Abs(lastMouseX) > momentumThreshold || Mathf.Abs(lastMouseY) > momentumThreshold))
            {
                momentumX = lastMouseX * rotationSpeed * 3;
                momentumY = lastMouseY * rotationSpeed * 3;
            }
            else
            {
                Camera cam = Camera.main;
                if (cam != null)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        playerScript?.OnTap(hit.point);
                    }
                }
            }

            playerScript?.SetMovementAllowed(true);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = false;
                momentumX = 0f;
                momentumY = 0f;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float touchX = touch.deltaPosition.x * touchSensitivity;
                float touchY = touch.deltaPosition.y * touchSensitivity;

                if (Mathf.Abs(touchX) > dragThreshold || Mathf.Abs(touchY) > dragThreshold)
                {
                    isDragging = true;
                    playerScript?.SetMovementAllowed(false);
                }

                if (isDragging)
                {
                    yaw += touchX * rotationSpeed;
                    pitch -= touchY * rotationSpeed;
                    pitch = Mathf.Clamp(pitch, -30f, 80f);
                }

                lastMouseX = touchX;
                lastMouseY = touchY;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (isDragging && (Mathf.Abs(lastMouseX) > momentumThreshold || Mathf.Abs(lastMouseY) > momentumThreshold))
                {
                    momentumX = lastMouseX * rotationSpeed * 3;
                    momentumY = lastMouseY * rotationSpeed * 3;
                }
                else
                {
                    Camera cam = Camera.main;
                    if (cam != null)
                    {
                        Ray ray = cam.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            playerScript?.OnTap(hit.point);
                        }
                    }
                }

                playerScript?.SetMovementAllowed(true);
            }
        }
    }
}
