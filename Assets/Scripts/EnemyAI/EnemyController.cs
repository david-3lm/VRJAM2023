using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // VARIABLES //

    [SerializeField] private GameObject objv;
    private EnemyFSM fsm;

    private GetCloseState getCloseState;
    private AttackState attackState;
    private FleeState fleeState;

    public float dist;
    public float speed;

    public GameLoop gameLoop;

    public int vida = 100;

    public string type;

    public int landDistance;

    [SerializeField] private int distAlejarse;
    [SerializeField] private int distAtacar;
    [SerializeField] private int distAcercarse;

    public GameObject airBullet;
    public GameObject groundBullet;

    public GameObject shootOrigin;

    public GameObject pivot;

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
        //this.objv = GameObject.Find("Player");
        Vector3 vecObj = this.objv.transform.position;

        this.getCloseState = new GetCloseState(vecObj);
        this.attackState = new AttackState(vecObj);
        this.fleeState = new FleeState(vecObj);

        

        //this.fsm = new EnemyFSM();
        this.fsm = this.gameObject.GetComponent<EnemyFSM>() as EnemyFSM;
        this.fsm.SetObjv(this.objv.transform.position);
        this.fsm.SetSpeed(speed);
        this.fsm.SetLandDistance(landDistance);
        this.fsm.SetType(type);

        Vector3 distVec = this.gameObject.transform.position - this.objv.transform.position;
        this.dist = distVec.magnitude;  

        if (this.dist < distAlejarse)
        {
            Vector3 ownPos = this.gameObject.transform.position;
            this.fleeState.SetOwn(ownPos);
            fsm.SetInitialState(fleeState);
        } else if (this.dist < distAtacar) 
        {
            Vector3 ownPos = this.gameObject.transform.position;
            this.attackState.SetOwn(ownPos);
            fsm.SetInitialState(attackState);
        } else 
        {
            Vector3 ownPos = this.gameObject.transform.position;
            this.getCloseState.SetOwn(ownPos);
            fsm.SetInitialState(getCloseState);
        }
    }

    private void Start()
    {
        //print(attackState.attackEvent);
        attackState.attackEvent.AddListener(Attack);
    }

    // Update is called once per frame
    void Update()
    {
        pivot.transform.LookAt(objv.transform, pivot.transform.up);

        Vector3 distVec = this.gameObject.transform.position - this.objv.transform.position;
        this.dist = distVec.magnitude;

        StateType currentStateType = this.fsm.GetCurrentState().GetStateType();

        if (currentStateType == StateType.ATTACK && this.dist < distAlejarse)
        {
            //Debug.Log("Cambio de Ataque a Flee");
            fsm.ChangeState(fleeState, this.gameObject.transform.position);
        }
        else if (currentStateType == StateType.GET_CLOSE && this.dist < distAtacar)
        {
            //Debug.Log("Cambio de Get_Close a Attack");
            fsm.ChangeState(attackState, this.gameObject.transform.position);
        }
        else if (currentStateType == StateType.FLEE && this.dist > distAcercarse)
        {
            //Debug.Log("Cambio de Flee a Get_Close");
            fsm.ChangeState(getCloseState, this.gameObject.transform.position);
        }
    }

    public void ChangeToGetClose()
    {
        fsm.ChangeState(getCloseState, this.gameObject.transform.position);
    }



    public void ReceiveDamage(int damage)
    {
        vida -= damage;
        if (vida < 0) gameLoop.DestroyEnemy(this.gameObject, type);
    }

    private void Attack()
    {
        if (type == "Flying")
        {
            GameObject bullet = Instantiate(airBullet, shootOrigin.transform.position, shootOrigin.transform.rotation);
            bullet.transform.SetParent(null);
            bullet.GetComponentInChildren<EnemyBullet>().ShootBullet(0.05f, 2000);
        }
        else
        {
            GameObject bullet = Instantiate(groundBullet, shootOrigin.transform.position, shootOrigin.transform.rotation);
            bullet.transform.SetParent(null);
            bullet.GetComponentInChildren<EnemyBullet>().ShootBullet(5f, 1000);
        }
    }

    public void ChangePos(Vector3 pos)
    {
        this.attackState.SetOwn(pos);
        this.getCloseState.SetOwn(pos);
        this.fleeState.SetOwn(pos);
    }
}
