using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    

    public AudioMixer mainMixer; //Referencia al mezclado de audio general

    //El slider del volumen
    public Slider volumeSlider;

    public Slider graphicsDropdown;

    private void OnEnable() //Comprobar que funciona
    {
        InitialValues();
    }



    #region set
    public void SetVolume(float volume) //Cambia el volumen segun el float que le proporciona el slider
    {
        mainMixer.SetFloat("MasterVolume", volume);
    }

    public void SetQuality(float qualityIndex) //Cambia la calidad en base al índice de las calidades del proyecto
    {
        int newQA = Mathf.FloorToInt(qualityIndex);
        QualitySettings.SetQualityLevel(newQA);
    }

    #endregion


    #region check

    public void CheckVolume()
    {
        //Comprobación del valor del volumen
        float val;
        mainMixer.GetFloat("MasterVolume", out val);//Pillamos el valor del mixer
        volumeSlider.value = val;
    }

    public void CheckQuality()
    {
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
    }


    #endregion

    public void InitialValues()
    {
        //CheckResolutions(); //Comprobamos las resoluciones
        CheckVolume(); //Comprobamos el volumen
        CheckQuality(); //Comprobamos los gráficos

    }
}
