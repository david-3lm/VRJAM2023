using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // VARIABLES //

    private BaseState initialState;
    private BaseState currentState;

    private GameObject objv;

    private float speed = 0.5f;

    // GETTERS & SETTERS //

    public BaseState GetInitialState() { return this.initialState; }
    public BaseState GetCurrentState() { return this.currentState; }
    public GameObject GetObjv() { return this.objv; }

    public void SetInitialState(BaseState initialState) { this.initialState = initialState; }
    public void SetCurrentState(BaseState currentState) { this.currentState = currentState; }
    public void SetObjv(GameObject objv) { this.objv = objv; }


    // CONSTRUCTOR //

    public EnemyFSM(BaseState initialState, BaseState currentState, GameObject objv) 
    {
        this.initialState = initialState;
        this.currentState = currentState;
        this.objv = objv;
    }


    // METHODS //

    private void Start()
    {
        currentState = initialState;
        currentState.EnterState(objv.transform.position);
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
        currentState.EnterState(objv.transform.position);
    }
}
