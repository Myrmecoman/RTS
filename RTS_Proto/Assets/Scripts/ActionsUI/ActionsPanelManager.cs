using UnityEngine;
using UnityEngine.UI;


public class ActionsPanelManager : MonoBehaviour
{
    public Transform slotsParent;

    private Button[,] buttons = new Button[3, 5];
    private string iconsPath = "icons/";


    private void Awake()
    {
        buttons[0,0] = slotsParent.GetChild(0).GetComponent<Button>();
        buttons[0,1] = slotsParent.GetChild(1).GetComponent<Button>();
        buttons[0,2] = slotsParent.GetChild(2).GetComponent<Button>();
        buttons[0,3] = slotsParent.GetChild(3).GetComponent<Button>();
        buttons[0,4] = slotsParent.GetChild(4).GetComponent<Button>();
        buttons[1,0] = slotsParent.GetChild(5).GetComponent<Button>();
        buttons[1,1] = slotsParent.GetChild(6).GetComponent<Button>();
        buttons[1,2] = slotsParent.GetChild(7).GetComponent<Button>();
        buttons[1,3] = slotsParent.GetChild(8).GetComponent<Button>();
        buttons[1,4] = slotsParent.GetChild(9).GetComponent<Button>();
        buttons[2,0] = slotsParent.GetChild(10).GetComponent<Button>();
        buttons[2,1] = slotsParent.GetChild(11).GetComponent<Button>();
        buttons[2,2] = slotsParent.GetChild(12).GetComponent<Button>();
        buttons[2,3] = slotsParent.GetChild(13).GetComponent<Button>();
        buttons[2,4] = slotsParent.GetChild(14).GetComponent<Button>();
    }


    private void ChangeActionButtons()
    {

    }
}
