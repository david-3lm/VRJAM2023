using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : BaseState
{
    // VARIABLES //

    private Vector3 objv;


    // GETTERS & SETTERS //

    public Vector3 GetObjv() { return this.objv; }

    public void SetObjv(Vector3 objv) { this.objv = objv; }


    // CONTRUCTOR //

    public FleeState(Vector3 objv) { this.objv = objv; }


    // METHODS //

    /*****************************************************
     * Method that is executed when the state is entered *
     *****************************************************/
    public override void EnterState(Vector3 objv)
    {
        Vector3 vec = this.gameObject.transform.position;

        Vector3 vecObjv = vec + (vec - objv) * 15;

        SetObjv(vecObjv);
    }


    /*****************************************************
     * Method that is executed while the state is active *
     *****************************************************/
    public override void UpdateState(float speed, float deltaTime)
    {
        // Get object position
        var position = this.gameObject.transform.position;

        // Calculate movement
        position = Vector3.MoveTowards(position, this.objv, speed * deltaTime);

        // Move
        this.gameObject.transform.position = position;
    }


    /****************************************************
     * Method that is executed when the state is exited *
     ****************************************************/
    public override void ExitState()
    {
        SetObjv(Vector3.zero);
    }
}
