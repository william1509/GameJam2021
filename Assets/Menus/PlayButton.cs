using UnityEngine.SceneManagement;

public class PlayButton : MenuButton
{
    protected override void ClickAction()
    {
        SceneManager.LoadScene("Level-1", LoadSceneMode.Single);
    }
}
