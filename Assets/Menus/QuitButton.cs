using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MenuButton
{
    protected override void ClickAction()
    {
        Application.Quit();
    }
}
