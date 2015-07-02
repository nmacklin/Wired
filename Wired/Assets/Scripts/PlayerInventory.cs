using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInventory : MonoBehaviour {
    // Very temporary player inventory code with associated HUD.
    // Currently contains Wire at position 0 and Power Source at 1.
    // Selection is lowered/raised by left bracket and right bracket, respectively.

    public GameObject powerSource;
    public Text backpackSelectionUI;

    private string[] backpack = new string[10];
    private int currentBackpackSelection;

	void Start () 
    {
        backpack[0] = "Wire";
        backpack[1] = "Power Source";

        currentBackpackSelection = 0;
        backpackSelectionUI.text = currentBackpackSelection.ToString();
	}

    public void CycleBackpackSelection(string direction)
    {
        if (direction == "forward" || direction == "backward")
        {
            if (direction == "forward")
            {
                if (currentBackpackSelection < 9)
                {
                    currentBackpackSelection += 1;
                }
                else
                {
                    currentBackpackSelection = 0;
                }
            }

            else
            {
                if (currentBackpackSelection > 0)
                {
                    currentBackpackSelection -= 1;
                }
                else
                {
                    currentBackpackSelection = 9;
                }
            }

            backpackSelectionUI.text = currentBackpackSelection.ToString();
        }
    }

    public int GetBackpackSelection()
    {
        return currentBackpackSelection;
    }
}
