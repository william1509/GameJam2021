using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Animator menuAnimator;
    bool clicked = false;

    public void OnPointerEnter(PointerEventData pEvent)
    {
        if(!clicked)
            GetComponent<Animator>().Play("Selected");
    }

    public void OnPointerExit(PointerEventData pEvent)
    {
        if(!clicked)
            GetComponent<Animator>().Play("UnSelected");
    }

    public void OnPointerClick(PointerEventData pEvent)
    {
        if (!clicked)
        {
            GetComponent<Animator>().Play("Clicked");
            menuAnimator.Play("FadeToWhite");
        }

        clicked = true;
    }

    protected abstract void ClickAction();
}
