using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.instance.TradeWithCharacterCurrentlyBeingSpokenTo();
    }
}
