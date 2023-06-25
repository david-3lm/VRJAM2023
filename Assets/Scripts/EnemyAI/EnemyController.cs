using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // VARIABLES //

    private GameObject objv;
    private EnemyFSM fsm;

    private GetCloseState getCloseState;
    private AttackState attackState;
    private FleeState fleeState;

    float dist;
 

    // GETTERS & SETTERS //

    public GameObject GetObjv() { return this.objv; }
    public EnemyFSM GetFSM() { return this.fsm; }

    public void SetObjv(GameObject objv) { this.objv = objv; }
    public void SetFsm(EnemyFSM fsm) { this.fsm = fsm; }


    // CONTRUCTOR //

    public EnemyController() { }


    // Start is called before the first frame update
    void Awake()
    {
        this.objv = GameObject.Find("Player");
        this.fsm = this.gameObject.GetComponent<EnemyFSM>();

        Vector3 distVec = this.gameObject.transform.position - this.objv.transform.position;
        this.dist = distVec.magnitude;

        if(this.dist < 5)
        {
            fsm.SetInitialState(attackState);
            fsm.SetCurrentState(attackState);
        } else if (this.dist < 3) 
        {
            fsm.SetInitialState(fleeState);
            fsm.SetCurrentState(attackState);
        } else 
        { 
            fsm.SetInitialState(getCloseState);
            fsm.SetCurrentState(attackState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.dist < 5)
        {
            fsm.ChangeState(attackState);
        }
        else if (this.dist < 3)
        {
            fsm.ChangeState(fleeState);
        }
        else
        {
            fsm.ChangeState(getCloseState);
        }
    }
}
