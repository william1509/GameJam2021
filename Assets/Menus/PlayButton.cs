using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MenuButton
{
    protected override void ClickAction()
    {
        menu_.GetGameManager().NextLevel();
    }
}
