using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : MenuButton
{
    protected override void ClickAction()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
