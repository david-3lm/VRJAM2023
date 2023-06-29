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
        Vector3 vecObj = this.objv.transform.position;

        this.getCloseState = new GetCloseState(vecObj);
        this.attackState = new AttackState(vecObj);
        this.fleeState = new FleeState(vecObj);

        //this.fsm = new EnemyFSM();
        this.fsm = this.gameObject.GetComponent<EnemyFSM>() as EnemyFSM;
        this.fsm.SetObjv(this.objv.transform.position);

        Vector3 distVec = this.gameObject.transform.position - this.objv.transform.position;
        this.dist = distVec.magnitude;

        if(this.dist < 5)
        {
            Vector3 ownPos = this.objv.transform.position;
            this.fleeState.SetOwn(ownPos);
            fsm.SetInitialState(fleeState);
        } else if (this.dist < 8) 
        {
            Vector3 ownPos = this.objv.transform.position;
            this.attackState.SetOwn(ownPos);
            fsm.SetInitialState(attackState);
        } else 
        {
            Vector3 ownPos = this.objv.transform.position;
            this.getCloseState.SetOwn(ownPos);
            fsm.SetInitialState(getCloseState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distVec = this.gameObject.transform.position - this.objv.transform.position;
        this.dist = distVec.magnitude;

        StateType currentStateType = this.fsm.GetCurrentState().GetStateType();

        if (currentStateType == StateType.ATTACK && this.dist < 5)
        {
            fsm.ChangeState(fleeState, this.gameObject.transform.position);
        }
        else if (currentStateType == StateType.GET_CLOSE && this.dist < 8)
        {
            fsm.ChangeState(attackState, this.gameObject.transform.position);
        }
        else if (currentStateType == StateType.FLEE && this.dist > 10)
        {
            fsm.ChangeState(getCloseState, this.gameObject.transform.position);
        }
    }
}
