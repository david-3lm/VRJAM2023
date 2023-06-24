using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class Canyon : MonoBehaviour
{
    public GameObject bullet;

    public GameObject shootOrigin;

    public AudioSource source;

    public GameObject canyonPivot;

    public XRJoystick joystick;

    public GameObject playerCamera;

    [Header("Basic Weapon")]

    public int basicDamage;

    public float basicForce;

    public float reloadTime;


    private bool canShoot;

    private bool reloaded;

    private void Start()
    {
        canShoot = true;
        reloaded = true;

        joystick.onValueChangeX.AddListener(TurnHorizontal);
        joystick.onValueChangeY.AddListener(TurnVertical);
    }
    public void Shoot()
    {
        if(canShoot && reloaded)
        {
            GameObject b = Instantiate(bullet, shootOrigin.transform.position, shootOrigin.transform.rotation);
            b.transform.SetParent(null);
            b.GetComponent<CanyonBullet>().SetBullet(basicDamage, basicForce);
            
            source.Play();

            canShoot = false;

            Invoke("Reload", reloadTime);
        }
    }

    public void Reload()
    {
        canShoot = true;
    }

    public void TurnHorizontal(float x)
    {
        canyonPivot.transform.RotateAround(canyonPivot.transform.position, Vector3.up, x);
        playerCamera.transform.RotateAround(playerCamera.transform.position, Vector3.up, x);
    }

    public void TurnVertical(float y)
    {
        canyonPivot.transform.RotateAround(canyonPivot.transform.position, canyonPivot.transform.right, y);
        playerCamera.transform.RotateAround(playerCamera.transform.position, playerCamera.transform.right, y);
    }
}
