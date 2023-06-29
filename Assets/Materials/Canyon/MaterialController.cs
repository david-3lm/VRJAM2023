using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField]private Material material;
    int hashCodeReloaded = Shader.PropertyToID("_Reloaded");
    private bool myReloaded;
    bool reloaded {
        get { return myReloaded; }
        set
        {
            if (value == myReloaded)
                return;

            myReloaded = value;
            StartCoroutine(Reloaded());
        }
    }
    int hashCodeTime = Shader.PropertyToID("_TimeReloaded");

    [Header("Transition Settings")]
    [SerializeField] private float transitionSpeed = 1.0f;
    private float value;
    private int maxValue = 1;
    private int minValue = 0;

    void OnEnable()
    {
        //Evento de reload en el arma
        reloaded = true;
    }

    IEnumerator Reloaded()
    {
        //Si reloaded pasa de true a false es que se ha disparado, por lo que el bucle se hace decreciendo
        if (!reloaded)
        {

            while (value > minValue)
            {
                value -= Time.deltaTime * transitionSpeed;
                material.SetFloat(hashCodeTime, value);
                yield return null;
            }

        }
        //en otro caso se hace incrementando el valor
        else
        {
            while (value < maxValue)
            {
                value += Time.deltaTime * transitionSpeed;
                material.SetFloat (hashCodeTime, value);
                yield return null;
            }
        }

        
    }
    
    //Llamadas desde el cañón para cambiar el material
    public void Reload()
    {
        material.SetFloat(hashCodeReloaded, 1);
        reloaded =true;
    }
    public void Shoot()
    {
        material.SetFloat(hashCodeReloaded, 0);
        reloaded = false;
    }
}
