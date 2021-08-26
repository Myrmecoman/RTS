using UnityEngine;

public class Input_Receiver : MonoBehaviour
{
    public Cam_controller cam_controller;

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

        controls.TopDownControls.MoveCommand.performed += _ => cam_controller.MoveCommand();

        controls.TopDownControls.Zoom.performed += _ => cam_controller.Zoom(_.ReadValue<Vector2>());
    }
}
