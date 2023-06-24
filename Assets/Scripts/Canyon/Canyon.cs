using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class Canyon : MonoBehaviour
{
    public GameObject shootOrigin;

    public AudioSource source;

    public GameObject canyonPivot;

    public XRJoystick joystick;

    public GameObject playerCamera;


    public enum Weapon { first = 0, second = 1}

    public Weapon currentWeapon;


    [Header("First Weapon")]

    public GameObject firstBullet;

    public int firstDamage;

    public float firstForce;

    public float firstReloadTime;

    private bool firstShooting;

    private bool firstReloaded;

    [Header("Second Weapon")]

    public GameObject secondBullet;

    public int secondDamage;

    public float secondForce;

    public float secondReloadTime;

    private bool secondReloaded;


    private bool canShoot;

    private void Start()
    {
        firstShooting = false;
        firstReloaded = true;

        secondReloaded = true;

        canShoot = true;

        joystick.onValueChangeX.AddListener(TurnHorizontal);
        joystick.onValueChangeY.AddListener(TurnVertical);
    }

    private void Update()
    {
        if (currentWeapon == Weapon.first)
        {
            print("CanShoot = " + canShoot + ", Shooting = " + firstShooting + ", Reloaded = " + firstReloaded);
            if (canShoot && firstShooting && firstReloaded)
            {
                FirstShoot();
            }
        }    
        else if(currentWeapon == Weapon.second)
        {

        }
    }

    public void RotateWeapon()
    {
        currentWeapon++;
        if(currentWeapon > Weapon.second)
        {
            currentWeapon = Weapon.first;
        }
    }

    private void FirstShoot()
    {
        GameObject b = Instantiate(firstBullet, shootOrigin.transform.position, shootOrigin.transform.rotation);
        b.transform.SetParent(null);
        b.GetComponent<CanyonBullet>().SetBullet(firstDamage, firstForce);

        source.Play();

        firstReloaded = false;

        Invoke("FirstReload", firstReloadTime);
    }

    private void FirstReload()
    {
        firstReloaded = true;
    }

    public void StartFirstShoot()
    {
        firstShooting = true;
    }

    public void EndFirstShoot()
    {
        firstShooting = false;
    }

    public void SecondShoot()
    {
        if(currentWeapon == Weapon.second)
        {
            if (canShoot && secondReloaded)
            {
                GameObject b = Instantiate(secondBullet, shootOrigin.transform.position, shootOrigin.transform.rotation);
                b.transform.SetParent(null);
                b.GetComponent<CanyonBullet>().SetBullet(secondDamage, secondForce);

                source.Play();

                secondReloaded = false;

                Invoke("SecondReload", secondReloadTime);
            }
        }
        
    }

    public void SecondReload()
    {
        secondReloaded = true;
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
