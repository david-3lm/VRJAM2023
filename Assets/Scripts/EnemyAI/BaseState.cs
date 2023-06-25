using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public BaseState() {}

    public abstract void EnterState(Vector3 obj);

    public abstract void UpdateState(float speed, float deltaTime);

    public abstract void ExitState();
}
