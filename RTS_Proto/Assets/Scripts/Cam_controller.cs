using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cam_controller : MonoBehaviour
{
    public float speed = 10f;
    public GameObject moveCommandObj;
    public GameManager gameManager;
    public SelectedDico selection;

    [HideInInspector] public Vector2 move;
    [HideInInspector] public bool HoldingStack = false;

    private Camera cam;
    private GameObject moveCommandSprite = null;


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
        if (selection.selectedTable.Count == 0)
            return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("ground");
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            Vector3 prevPos = Vector3.zero;

            if (moveCommandSprite != null)
                Destroy(moveCommandSprite);

            moveCommandSprite = Instantiate(moveCommandObj);
            moveCommandSprite.transform.position = hit.point + new Vector3(0, 0.0001f, 0);

            gameManager.MoveCommand(selection.selectedTable, moveCommandSprite.transform.position);
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
