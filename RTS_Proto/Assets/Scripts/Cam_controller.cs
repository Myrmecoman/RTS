using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cam_controller : MonoBehaviour
{
    public float speed = 10f;
    public GameObject MoveCommandObj;
    public GameManager gameManager;

    [HideInInspector] public Vector2 move;

    private Camera cam;
    private GameObject moveCommandSprite = null;
    private List<AgentNavigation> selectedAgents;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        selectedAgents = new List<AgentNavigation>();
        UpdateHeight();
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x + move.x * Time.deltaTime * speed,
            transform.position.y,
            transform.position.z + move.y * Time.deltaTime * speed);
    }


    public void Select()
    {
        foreach (var i in selectedAgents)
            i.UnSelect();

        selectedAgents.Clear();

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.transform.tag == "agent")
            {
                AgentNavigation agent = hit.transform.GetComponent<AgentNavigation>();
                agent.Select();
                selectedAgents.Add(agent);
            }
        }
    }


    public void MoveCommand()
    {
        if (selectedAgents.Count == 0)
            return;

        // Bit shift the index of the layer to get a bit mask, 0 is default
        int layerMask = 1 << 0;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            Vector3 prevPos = Vector3.zero;

            if (moveCommandSprite != null)
            {
                // Don't recalculate path if click at same place
                // if (moveCommandSprite.transform.position == hit.point + new Vector3(0, 0.0001f, 0))
                //     return;
                Destroy(moveCommandSprite);
            }

            moveCommandSprite = Instantiate(MoveCommandObj);
            moveCommandSprite.transform.position = hit.point + new Vector3(0, 0.0001f, 0);

            gameManager.MoveCommand(selectedAgents, moveCommandSprite.transform.position);
        }
    }


    public void Zoom(Vector2 value)
    {
        if (value.y > 0)
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - 2f, 10f, 50f), transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + 2f, 10f, 50f), transform.position.z);

        UpdateHeight();
    }


    private void UpdateHeight()
    {
        transform.eulerAngles = new Vector3(45 + (transform.position.y - 10), transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
