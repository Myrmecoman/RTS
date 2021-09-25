using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Receiver : MonoBehaviour
{
    public static Input_Receiver instance;

    public CamController cam_controller;
    public GlobalSelection selection;

    [HideInInspector] public char lastKeyPressed = '\0';

    private Controls controls;

    #region Enable/Disable

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    #endregion

    private void Awake()
    {
        // make this a singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        controls = new Controls();

        controls.TopDownControls.Zoom.performed += _ => cam_controller.Zoom(_.ReadValue<Vector2>());

        controls.TopDownControls.HoldPosition.performed += _ => cam_controller.HoldPosition();

        controls.TopDownControls.MoveCommand.performed += _ => cam_controller.MoveCommand();

        controls.TopDownControls.StackAction.performed += _ => selection.StackActionHold = true;
        controls.TopDownControls.StackAction.canceled += _ => selection.StackActionHold = false;
    }


    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (controls.TopDownControls.Attack.triggered && SelectedDico.instance.selectedTable.Count != 0)
            {
                lastKeyPressed = 'A'; // A is for attack
                Debug.Log("Attack pressed");
            }
            else if (controls.TopDownControls.Patrol.triggered && SelectedDico.instance.selectedTable.Count != 0)
            {
                lastKeyPressed = 'R'; // R is for patrol
                Debug.Log("Patrol pressed");
            }
            else
                lastKeyPressed = '\0';
        }
    }


    public void ChooseAction()
    {
        if (lastKeyPressed == '\0')
            Debug.LogError("Error last key is \\0");
        else if (lastKeyPressed == 'A')
            cam_controller.AttackCommand();
        else if (lastKeyPressed == 'R')
            cam_controller.PatrolCommand();
        else
            Debug.LogError("Error on last key pressed: " + lastKeyPressed);

        lastKeyPressed = '\0';
    }
}
