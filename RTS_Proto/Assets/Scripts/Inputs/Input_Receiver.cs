using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Receiver : MonoBehaviour
{
    public CamController cam_controller;
    public GlobalSelection selection;

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
        controls = new Controls();

        controls.TopDownControls.Move.performed += _ => cam_controller.move = _.ReadValue<Vector2>();
        controls.TopDownControls.Move.canceled += _ => cam_controller.move = Vector2.zero;

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

        }
    }
}
