using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsButton : MenuButton
{
    protected override void ClickAction()
    {
        menu_.GetGameManager().ShowControls();
    }
}
