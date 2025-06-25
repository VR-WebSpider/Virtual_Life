using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool canMove = true;
    private bool isDragging = false;
    private Vector2 startTouchPosition;
    private float dragThreshold = 10f; 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
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
    }

    public void SetMovementAllowed(bool allowed)
    {
        canMove = allowed;
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(startTouchPosition, Input.mousePosition) > dragThreshold)
            {
                isDragging = true; 
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    OnTap(hit.point);
                }
            }
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (Vector2.Distance(startTouchPosition, touch.position) > dragThreshold)
                {
                    isDragging = true; 
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!isDragging) 
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        OnTap(hit.point);
                    }
                }
            }
        }
    }

    public void OnTap(Vector3 targetPosition)
    {
        if (canMove)
        {
            agent.SetDestination(targetPosition);
        }
    }
}
