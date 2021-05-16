using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
    public void Open()
    {
        GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void Close()
    {
        GetComponent<Animator>().SetBool("isOpen", false);
    }
}
