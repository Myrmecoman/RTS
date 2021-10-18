using UnityEngine;
using UnityEngine.InputSystem;


public class InputReceiver : MonoBehaviour
{
    public static InputReceiver instance;

    public CamController camController;
    public GlobalSelection selection;
    public SelectedDico selectedDico;
    public AllGroups allGroups;

    [HideInInspector] public char lastKeyPressed = '\0';

    private Controls controls;
    private bool addGroupHold = false;
    private bool removeFromGroup = false;
    private bool addAndRemoveFromOtherGroups = false;


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

        controls.TopDownControls.AddToGroup.performed += _ => addGroupHold = true;
        controls.TopDownControls.AddToGroup.canceled += _ => addGroupHold = false;

        controls.TopDownControls.RemoveFromGroup.performed += _ => removeFromGroup = true;
        controls.TopDownControls.RemoveFromGroup.canceled += _ => removeFromGroup = false;

        controls.TopDownControls.AddAndRemoveFromOtherGroups.performed += _ => addAndRemoveFromOtherGroups = true;
        controls.TopDownControls.AddAndRemoveFromOtherGroups.canceled += _ => addAndRemoveFromOtherGroups = false;

        controls.TopDownControls.Group1.performed += _ => ChooseGroupAction(1);
        controls.TopDownControls.Group2.performed += _ => ChooseGroupAction(2);
        controls.TopDownControls.Group3.performed += _ => ChooseGroupAction(3);
        controls.TopDownControls.Group4.performed += _ => ChooseGroupAction(4);
        controls.TopDownControls.Group5.performed += _ => ChooseGroupAction(5);
        controls.TopDownControls.Group6.performed += _ => ChooseGroupAction(6);
        controls.TopDownControls.Group7.performed += _ => ChooseGroupAction(7);
        controls.TopDownControls.Group8.performed += _ => ChooseGroupAction(8);
        controls.TopDownControls.Group9.performed += _ => ChooseGroupAction(9);
        controls.TopDownControls.Group0.performed += _ => ChooseGroupAction(0);
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
    }


    public void ChooseAction()
    {
        if (lastKeyPressed == 'A')
            camController.AttackCommand();
        else if (lastKeyPressed == 'R')
            camController.PatrolCommand();

        lastKeyPressed = '\0';
    }


    private void ChooseGroupAction(int groupNb)
    {
        if (addGroupHold)
            allGroups.AddToGroup(selectedDico.selectedTable, groupNb);
        else if (removeFromGroup)
            allGroups.RemoveFromGroup(selectedDico.selectedTable, groupNb);
        else if (addAndRemoveFromOtherGroups)
            allGroups.AddAndRemoveFromOtherGroups(selectedDico.selectedTable, groupNb);
        else
            selectedDico.SelectGroup(groupNb);
    }
}
