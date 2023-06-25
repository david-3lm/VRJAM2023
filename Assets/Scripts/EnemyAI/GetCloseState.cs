using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCloseState : BaseState
{
    // VARIABLES //

    private Vector3 objv;


    // GETTERS & SETTERS //

    public Vector3 GetObjv() { return this.objv; }

    public void SetObjv(Vector3 objv) { this.objv = objv; }


    // CONTRUCTOR //

    public GetCloseState(Vector3 objv) { this.objv = objv; }


    // METHODS //

    /*****************************************************
     * Method that is executed when the state is entered *
     *****************************************************/
    public override void EnterState(Vector3 objv) 
    {
        SetObjv(objv);
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
