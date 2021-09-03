using UnityEngine;
using UnityEngine.InputSystem;

public class Cam_controller : MonoBehaviour
{
    public static Cam_controller instance;

    public float speed = 10f;
    public GameObject moveCommandObj;

    [HideInInspector] public Vector2 move;
    [HideInInspector] public bool HoldingStack = false;

    private Camera cam;
    private GameObject moveCommandSprite = null;


    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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


    public void MoveCommand()
    {
        if (SelectedDico.instance.selectedTable.Count == 0)
            return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("ground", "building", "agent", "resource");
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            if (moveCommandSprite != null)
                Destroy(moveCommandSprite);

            if (hit.transform.gameObject.tag == "ground")
            {
                moveCommandSprite = Instantiate(moveCommandObj);
                moveCommandSprite.transform.position = hit.point + new Vector3(0, 0.0001f, 0);
                GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, moveCommandSprite.transform.position);
            }
            if (hit.transform.gameObject.tag == "agent")
            {
                hit.collider.GetComponent<AgentNavigation>().MoveTowardsSprite();
                GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, hit.transform.position);
            }
            if (hit.transform.gameObject.tag == "building")
            {
                hit.collider.GetComponent<BuildingManager>().MoveTowardsSprite();
                GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, hit.transform.position);
            }
            if (hit.transform.gameObject.tag == "resource")
            {
                hit.collider.GetComponent<ResourceObject>().MoveTowardsSprite();
                GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, hit.transform.position);
            }
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
