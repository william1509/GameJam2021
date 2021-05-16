using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MenuButton
{
    protected override void ClickAction()
    {
        menu_.GetGameManager().GoToMainMenu();
    }
}
