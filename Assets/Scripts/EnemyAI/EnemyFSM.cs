using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // VARIABLES //

    private BaseState initialState;
    private BaseState currentState;

    private Vector3 objv;

    private float speed = 0.5f;

    // GETTERS & SETTERS //

    public BaseState GetInitialState() { return this.initialState; }
    public BaseState GetCurrentState() { return this.currentState; }
    public Vector3 GetObjv() { return this.objv; }

    public void SetInitialState(BaseState initialState) { this.initialState = initialState; }
    public void SetCurrentState(BaseState currentState) { this.currentState = currentState; }
    public void SetObjv(Vector3 objv) { this.objv = objv; }


    // CONSTRUCTOR //

    public EnemyFSM(BaseState initialState, BaseState currentState, Vector3 objv) 
    {
        this.initialState = initialState;
        this.currentState = currentState;
        this.objv = objv;
    }

    public EnemyFSM() { }


    // METHODS //

    private void Start()
    {
        currentState = initialState;
        currentState.EnterState(objv);
    }

    private void Update()
    {
        currentState.UpdateState(speed, Time.deltaTime);
    }

    /**********************************************************
     * Method that is executed to change the state of the FSM *
     **********************************************************/
    public void ChangeState(BaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState(objv);
    }
}
