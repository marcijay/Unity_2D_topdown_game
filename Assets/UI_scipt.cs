using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_scipt : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource effectsSource;

    public Slider musicSlider;
    public Slider effectsSlider;

    public TMP_Text health;
    public TMP_Text enemiesLeft;
    public TMP_Text toKill;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.volume = musicSlider.value;
    }

    public void OnMusicSliderChange()
    {
        musicSource.volume = musicSlider.value;
    }

    public void OnEffectsSliderChange()
    {
        effectsSource.volume = effectsSlider.value;
    }

    public void SetHealthText(int value)
    {
        health.text = $"Health: {value}";
    }

    public void SetEnemiesLeftText(int value)
    {
        enemiesLeft.text = $"Enemies left: {value}";
    }

    public void SetToKillText(int value)
    {
        toKill.text = $"To kill: {value}";
    }

    public void SetEffectsSourceVolume()
    {
        effectsSource.volume = effectsSlider.value;
    }

}
