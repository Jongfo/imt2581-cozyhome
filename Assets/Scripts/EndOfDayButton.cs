using UnityEngine;

public class EndOfDayButton : MonoBehaviour
{
    public void OnClick()
    {
        if (GameManager.instance.Day == 3)
        {
            Application.Quit();
        }
        else
        {
            GameManager.instance.StartDayTransition();
        }
    }
}
