using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCloseState : BaseState
{
    // VARIABLES //

    private StateType type = StateType.GET_CLOSE;

    private Vector3 objv;
    private Vector3 own;


    // GETTERS & SETTERS //

    public override StateType GetStateType() { return this.type; }
    public Vector3 GetObjv() { return this.objv; }
    public Vector3 GetOwn() { return this.own; }

    public void SetObjv(Vector3 objv) { this.objv = objv; }
    public override void SetOwn(Vector3 own) { this.own = own; }


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
    public override Vector3 UpdateState(float speed, float deltaTime) 
    {
        // Calculate movement
        this.own = Vector3.MoveTowards(this.own, this.objv, speed * deltaTime);

        // Move
        return own;
    }


    /****************************************************
     * Method that is executed when the state is exited *
     ****************************************************/
    public override void ExitState() 
    {
        SetObjv(Vector3.zero);
    }
}
