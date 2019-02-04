using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "New Quest", menuName="Quest/Quest")]
public class Quest : ScriptableObject
{
    public string title;
    public string description;
    public UnityEvent action;   // The method that determines what happens at each stage
    public UnityEvent trigger;  // The method that checks to see if the next stage should be started
}
