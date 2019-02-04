using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTransitionPanel : MonoBehaviour
{
    public void OnEndAnimation()
    {
        GameManager.instance.StartNewDay();
    }
}
