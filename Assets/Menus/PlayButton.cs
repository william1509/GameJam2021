using UnityEngine.SceneManagement;

public class PlayButton : MenuButton
{
    protected override void ClickAction()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
