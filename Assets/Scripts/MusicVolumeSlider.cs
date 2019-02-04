using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public void ChangeVolume()
    {
        SoundManager.instance.ChangeMusicVolume(GetComponent<Slider>().value);
    }
}
