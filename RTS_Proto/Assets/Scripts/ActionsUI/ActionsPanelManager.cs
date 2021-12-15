using UnityEngine;
using UnityEngine.UI;


public class ActionsPanelManager : MonoBehaviour
{
    public Transform slotsParent;

    private Button b11;
    private Button b12;
    private Button b13;
    private Button b14;
    private Button b15;
    private Button b21;
    private Button b22;
    private Button b23;
    private Button b24;
    private Button b25;
    private Button b31;
    private Button b32;
    private Button b33;
    private Button b34;
    private Button b35;
    private string iconsPath = "icons/";


    private void Awake()
    {
        b11 = slotsParent.GetChild(0).GetComponent<Button>();
        b12 = slotsParent.GetChild(1).GetComponent<Button>();
        b13 = slotsParent.GetChild(2).GetComponent<Button>();
        b14 = slotsParent.GetChild(3).GetComponent<Button>();
        b15 = slotsParent.GetChild(4).GetComponent<Button>();
        b21 = slotsParent.GetChild(0).GetComponent<Button>();
        b22 = slotsParent.GetChild(1).GetComponent<Button>();
        b23 = slotsParent.GetChild(2).GetComponent<Button>();
        b24 = slotsParent.GetChild(3).GetComponent<Button>();
        b25 = slotsParent.GetChild(4).GetComponent<Button>();
        b31 = slotsParent.GetChild(0).GetComponent<Button>();
        b32 = slotsParent.GetChild(1).GetComponent<Button>();
        b33 = slotsParent.GetChild(2).GetComponent<Button>();
        b34 = slotsParent.GetChild(3).GetComponent<Button>();
        b35 = slotsParent.GetChild(4).GetComponent<Button>();
    }


    private void ChangeActionButtons()
    {

    }
}
