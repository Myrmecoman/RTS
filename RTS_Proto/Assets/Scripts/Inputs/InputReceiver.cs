using UnityEngine;
using UnityEngine.InputSystem;


public class InputReceiver : MonoBehaviour
{
    public static InputReceiver instance;

    public CamController camController;
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

        controls.TopDownControls.Zoom.performed += _ => camController.Zoom(_.ReadValue<Vector2>());

        controls.TopDownControls.HoldPosition.performed += _ => camController.HoldPosition();

        controls.TopDownControls.MoveCommand.performed += _ => camController.MoveCommand();

        controls.TopDownControls.StackAction.performed += _ => selection.StackActionHold = true;
        controls.TopDownControls.StackAction.canceled += _ => selection.StackActionHold = false;
    }


    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (controls.TopDownControls.Attack.triggered && SelectedDico.instance.selectedTable.Count != 0)
                lastKeyPressed = 'A'; // A is for attack
            else if (controls.TopDownControls.Patrol.triggered && SelectedDico.instance.selectedTable.Count != 0)
                lastKeyPressed = 'R'; // R is for patrol
            else
                lastKeyPressed = '\0';
        }

        /*
        if (Keyboard.current.anyKey.isPressed)
        {
            if (controls.TopDownControls.AddToGroup.triggered && SelectedDico.instance.selectedTable.Count != 0)
                lastKeyPressed = 'S'; // S is for adding to group
            else if (controls.TopDownControls.RemoveFromGroup.triggered && SelectedDico.instance.selectedTable.Count != 0)
                lastKeyPressed = 'C'; // C is for removing from group
            else if (controls.TopDownControls.AddAndRemoveFromOtherGroups.triggered && SelectedDico.instance.selectedTable.Count != 0)
                lastKeyPressed = 'L'; // L is for adding to group and removing from others
        }
        */
    }


    public void ChooseAction()
    {
        if (lastKeyPressed == 'A')
            camController.AttackCommand();
        else if (lastKeyPressed == 'R')
            camController.PatrolCommand();

        lastKeyPressed = '\0';
    }
}
