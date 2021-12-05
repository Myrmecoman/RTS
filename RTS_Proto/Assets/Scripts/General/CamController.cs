using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class CamController : MonoBehaviour
{
    public static CamController instance;

    public float speed = 10f;
    public GameObject moveCommandObj;
    public GameObject attackCommandObj;
    public GameObject patrolCommandObj;
    public Camera cam;

    [HideInInspector] public bool HoldingStack = false;

    private GameObject lastCommandSprite = null;


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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Application.isFocused)
            return;

        Vector2 move = MoveCam();

        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x + move.x * Time.deltaTime * speed, -210, 210),
            transform.localPosition.y,
            Mathf.Clamp(transform.localPosition.z + move.y * Time.deltaTime * speed, -210, 210));
    }


    public void HoldPosition()
    {
        if (SelectedDico.instance.selectedTable.Count == 0)
            return;

        foreach (KeyValuePair<int, Selectable> ag in SelectedDico.instance.selectedTable)
            ag.Value.HoldPosition();
    }


    public void AttackCommand()
    {
        if (SelectedDico.instance.selectedTable.Count == 0)
            return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("ground", "building", "agent"); // cannot attack resource
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            if (lastCommandSprite != null)
                Destroy(lastCommandSprite);

            if (hit.transform.gameObject.tag == "ground")
            {
                lastCommandSprite = Instantiate(attackCommandObj);
                lastCommandSprite.transform.position = hit.point + new Vector3(0, 0.0001f, 0);
                GameManager.instance.AttackCommand(SelectedDico.instance.selectedTable, lastCommandSprite.transform);
            }
            else if (hit.transform.gameObject.tag == "agent" || hit.transform.gameObject.tag == "building")
            {
                hit.collider.GetComponent<Selectable>().MoveTowardsSprite();
                GameManager.instance.AttackCommand(SelectedDico.instance.selectedTable, hit.transform, true);
            }
            else if (hit.transform.gameObject.tag == "resource")
            {
                hit.collider.GetComponent<ResourceManager>().MoveTowardsSprite();
                GameManager.instance.AttackCommand(SelectedDico.instance.selectedTable, hit.transform);
            }
        }
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
            if (lastCommandSprite != null)
                Destroy(lastCommandSprite);

            if (hit.transform.gameObject.tag == "ground")
            {
                lastCommandSprite = Instantiate(moveCommandObj);
                lastCommandSprite.transform.position = hit.point + new Vector3(0, 0.0001f, 0);
                GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, lastCommandSprite.transform);
            }
            else if (hit.transform.gameObject.tag == "agent" || hit.transform.gameObject.tag == "building")
            {
                hit.collider.GetComponent<Selectable>().MoveTowardsSprite();
                if (hit.collider.GetComponent<Selectable>().isAlly)
                    GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, hit.transform, true);
                else
                    GameManager.instance.AttackCommand(SelectedDico.instance.selectedTable, hit.transform, true);
            }
            else if (hit.transform.gameObject.tag == "resource")
            {
                hit.collider.GetComponent<ResourceManager>().MoveTowardsSprite();
                GameManager.instance.MoveCommand(SelectedDico.instance.selectedTable, hit.transform, false, true);
            }
        }
    }


    public void Zoom(Vector2 value)
    {
        if (value.y > 0)
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - 3, 40, 70);
        else
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + 3, 40, 70);
    }


    public Vector2 MoveCam()
    {
        Vector2 keyArrows = Vector2.zero;
        if (Keyboard.current.leftArrowKey.isPressed)
            keyArrows.x = -1;
        if (Keyboard.current.rightArrowKey.isPressed)
            keyArrows.x = 1;
        if (Keyboard.current.downArrowKey.isPressed)
            keyArrows.y = -1;
        if (Keyboard.current.upArrowKey.isPressed)
            keyArrows.y = 1;

        keyArrows.Normalize();

        if (keyArrows != Vector2.zero)
            return keyArrows;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 mouseVector = Vector2.zero;

#if UNITY_EDITOR
        if (mousePos.x <= 0)
        {
            mouseVector.Set(mouseVector.x - 1, mouseVector.y);
        }
        if (mousePos.y <= 1)
        {
            mouseVector.Set(mouseVector.x, mouseVector.y - 1);
        }
        if (mousePos.x >= Handles.GetMainGameViewSize().x - 1)
        {
            mouseVector.Set(mouseVector.x + 1, mouseVector.y);
        }
        if (mousePos.y >= Handles.GetMainGameViewSize().y - 1)
        {
            mouseVector.Set(mouseVector.x, mouseVector.y + 1);
        }
#else
        if (mousePos.x <= 0)
        {
            mouseVector.Set(mouseVector.x - 1, mouseVector.y);
        }
        if (mousePos.y <= 0)
        {
            mouseVector.Set(mouseVector.x, mouseVector.y - 1);
        }
        if (mousePos.x >= Screen.width - 1)
        {
            mouseVector.Set(mouseVector.x + 1, mouseVector.y);
        }
        if (mousePos.y >= Screen.height - 1)
        {
            mouseVector.Set(mouseVector.x, mouseVector.y + 1);
        }
#endif

        mouseVector.Normalize();
        return mouseVector;
    }
}