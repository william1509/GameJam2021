using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MenuButton
{
    protected override void ClickAction()
    {
        menu_.GetGameManager().RestartLevel();
    }
}
