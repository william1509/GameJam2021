using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Menu menu_;

    static string buttonSelectedSound = "Sounds/ButtonSelect";
    public Animator menuAnimator;
    bool clicked = false;

    AudioSource buttonSelectedSource;

    void Start()
    {
        buttonSelectedSource = gameObject.AddComponent<AudioSource>();
        buttonSelectedSource.clip = Resources.Load(buttonSelectedSound) as AudioClip;
    }

    public void OnPointerEnter(PointerEventData pEvent)
    {
        if (!clicked)
        {
            buttonSelectedSource.Play();
            GetComponent<Animator>().Play("Selected");
        }
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
            if (menuAnimator != null)
            {
                menuAnimator.Play("FadeToWhite");
            }
        }

        clicked = true;
    }

    protected abstract void ClickAction();
}
