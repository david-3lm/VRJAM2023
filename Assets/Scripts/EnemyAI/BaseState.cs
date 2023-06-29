using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(Vector3 obj);

    public abstract Vector3 UpdateState(float speed, string type, int landDistance);

    public abstract void ExitState();

    public abstract void SetOwn(Vector3 ownPos);

    public abstract StateType GetStateType();
}

public enum StateType
{
    GET_CLOSE, ATTACK, FLEE
}