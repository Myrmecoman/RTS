using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputReceiver : MonoBehaviour
{
    public static InputReceiver instance;

    public CamController camController;
    public GlobalSelection selection;
    public SelectedDico selectedDico;
    public AllGroups allGroups;
    public AllCameras allCameras;
    public Button[] buttonGroups = new Button[10];

    [HideInInspector] public char lastKeyPressed = '\0';

    private Controls controls;
    private bool addCameraHold = false;
    private bool addGroupHold = false;
    private bool removeFromGroup = false;
    private bool addAndRemoveFromOtherGroups = false;
    private int lastGroupSelected = -1;
    private double groupTimer = 0;


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

        // CONTROL GROUPS
        controls.TopDownControls.StackAction.performed += _ => selection.StackActionHold = true;
        controls.TopDownControls.StackAction.canceled += _ => selection.StackActionHold = false;

        controls.TopDownControls.AddToGroup.performed += _ => addGroupHold = true;
        controls.TopDownControls.AddToGroup.canceled += _ => addGroupHold = false;

        controls.TopDownControls.RemoveFromGroup.performed += _ => removeFromGroup = true;
        controls.TopDownControls.RemoveFromGroup.canceled += _ => removeFromGroup = false;

        controls.TopDownControls.AddAndRemoveFromOtherGroups.performed += _ => addAndRemoveFromOtherGroups = true;
        controls.TopDownControls.AddAndRemoveFromOtherGroups.canceled += _ => addAndRemoveFromOtherGroups = false;

        controls.TopDownControls.Group1.performed += _ => { ChooseGroupAction(1); buttonGroups[1].Select(); };
        controls.TopDownControls.Group1.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group2.performed += _ => { ChooseGroupAction(2); buttonGroups[2].Select(); };
        controls.TopDownControls.Group2.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group3.performed += _ => { ChooseGroupAction(3); buttonGroups[3].Select(); };
        controls.TopDownControls.Group3.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group4.performed += _ => { ChooseGroupAction(4); buttonGroups[4].Select(); };
        controls.TopDownControls.Group4.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group5.performed += _ => { ChooseGroupAction(5); buttonGroups[5].Select(); };
        controls.TopDownControls.Group5.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group6.performed += _ => { ChooseGroupAction(6); buttonGroups[6].Select(); };
        controls.TopDownControls.Group6.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group7.performed += _ => { ChooseGroupAction(7); buttonGroups[7].Select(); };
        controls.TopDownControls.Group7.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group8.performed += _ => { ChooseGroupAction(8); buttonGroups[8].Select(); };
        controls.TopDownControls.Group8.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group9.performed += _ => { ChooseGroupAction(9); buttonGroups[9].Select(); };
        controls.TopDownControls.Group9.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        controls.TopDownControls.Group0.performed += _ => { ChooseGroupAction(0); buttonGroups[0].Select(); };
        controls.TopDownControls.Group0.canceled += _ => EventSystem.current.SetSelectedGameObject(null);
        // END OF CONTROL GROUPS

        // CAMERAS LOCATIONS
        controls.TopDownControls.AddOrReplaceCamera.performed += _ => addCameraHold = true;
        controls.TopDownControls.AddOrReplaceCamera.canceled += _ => addCameraHold = false;

        controls.TopDownControls.CamBase1.performed += _ => ChooseCameraAction(0);
        controls.TopDownControls.CamBase2.performed += _ => ChooseCameraAction(1);
        controls.TopDownControls.CamBase3.performed += _ => ChooseCameraAction(2);
        controls.TopDownControls.CamBase4.performed += _ => ChooseCameraAction(3);
        controls.TopDownControls.CamBase5.performed += _ => ChooseCameraAction(4);
        controls.TopDownControls.CamBase6.performed += _ => ChooseCameraAction(5);
        controls.TopDownControls.CamBase7.performed += _ => ChooseCameraAction(6);
        controls.TopDownControls.CamBase8.performed += _ => ChooseCameraAction(7);
        controls.TopDownControls.CamBase9.performed += _ => ChooseCameraAction(8);
        controls.TopDownControls.CamBase10.performed += _ => ChooseCameraAction(9);
        // END OF CAMERA LOCATIONS
    }


    private void Update()
    {
        if (groupTimer > 0)
            groupTimer -= Time.deltaTime;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (controls.TopDownControls.Attack.triggered && SelectedDico.instance.selectedTable.Count != 0)
                lastKeyPressed = 'A'; // A is for attack
            else
                lastKeyPressed = '\0';
        }
    }


    public void ChooseAction()
    {
        if (lastKeyPressed == 'A')
            camController.AttackCommand();

        lastKeyPressed = '\0';
    }


    public void ChooseGroupAction(int groupNb)
    {
        if (addGroupHold)
            allGroups.AddToGroup(selectedDico.selectedTable, groupNb);
        else if (removeFromGroup)
            allGroups.RemoveFromGroup(selectedDico.selectedTable, groupNb);
        else if (addAndRemoveFromOtherGroups)
            allGroups.AddAndRemoveFromOtherGroups(selectedDico.selectedTable, groupNb);
        else if (lastGroupSelected != groupNb || groupTimer <= 0)
            selectedDico.SelectGroup(groupNb);
        else
        {
            Vector2 newLoaction = GroupCentroidFinder.GetPlanarCentroid(allGroups.controlGroups[groupNb]);
            camController.transform.position = new Vector3(newLoaction.x, camController.transform.position.y, newLoaction.y);
        }

        lastGroupSelected = groupNb;
        groupTimer = 0.4;
    }


    private void ChooseCameraAction(int camNb)
    {
        if (addCameraHold)
            allCameras.SetCameraPosition(camNb, camController.transform.localPosition);
        else
            allCameras.GoToCameraPosition(camNb);
    }
}
