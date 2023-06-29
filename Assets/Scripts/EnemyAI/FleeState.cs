using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : BaseState
{
    // VARIABLES //

    private StateType type = StateType.FLEE;

    private Vector3 objv;
    private Vector3 own;


    // GETTERS & SETTERS //

    public override StateType GetStateType() { return this.type; }
    public Vector3 GetObjv() { return this.objv; }
    public Vector3 GetOwn() { return this.own; }

    public void SetObjv(Vector3 objv) { this.objv = objv; }
    public override void SetOwn(Vector3 own) { this.own = own; }


    // CONTRUCTOR //

    public FleeState(Vector3 objv) { this.objv = objv; }


    // METHODS //

    /*****************************************************
     * Method that is executed when the state is entered *
     *****************************************************/
    public override void EnterState(Vector3 objv)
    {
        Vector3 vecObjv = this.own + (this.own - objv) * 15;

        SetObjv(vecObjv);
    }


    /*****************************************************
     * Method that is executed while the state is active *
     *****************************************************/
    public override Vector3 UpdateState(float speed, string type, int landDistance)
    {
        if (type == "Flying")
        {
            // Calculate movement
            this.own = Vector3.MoveTowards(this.own, this.objv, speed * Time.deltaTime);
        }
        else
        {

            // Calculate movement
            this.own = Vector3.MoveTowards(this.own, new Vector3(this.objv.x, landDistance, this.objv.z), speed * Time.deltaTime);
        }

        // Move
        return this.own;
    }


    /****************************************************
     * Method that is executed when the state is exited *
     ****************************************************/
    public override void ExitState()
    {
        SetObjv(Vector3.zero);
    }
}
