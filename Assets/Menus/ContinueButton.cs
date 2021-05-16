using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MenuButton
{
    protected override void ClickAction()
    {
        menu_.GetGameManager().TogglePause();
        GetComponent<Animator>().Play("NotSelected");
    }
}
