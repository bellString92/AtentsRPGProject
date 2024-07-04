using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class EnemyMovement : BattleSystem
{
    public enum State { Create, Normal, Battle, Death }
    public State myState = State.Create;
    Vector3 startPos;
    Coroutine coRoam = null;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        OnChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void StopRoamCorutine()
    {
        if (coRoam != null)
        {
            StopCoroutine(coRoam);
            coRoam = null;
        }
    }

    void OnChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                StopMoveingCoroutine();
                StopRoamCorutine();
                coRoam = StartCoroutine(Roaming());
                break;
            case State.Battle:
                StopMoveingCoroutine();
                StopRoamCorutine();
                FollowTarget(myTarget, v => v < myBattleState.AttackRange, OnAttack);
                break;
            case State.Death:
                StopAllCoroutines();
                deadAct?.Invoke();
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                break;
            case State.Battle:
                BattleUpdate();
                break;
            case State.Death:
                break;
        }
    }
    public void OnBattel(Transform target)
    {
        myTarget = target;
        OnChangeState(State.Battle);

    }
    public void OnNomal()
    {
        myTarget = null;
        OnChangeState(State.Normal);
    }

    IEnumerator Roaming()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        Vector3 dir = Vector3.forward;
        dir = Quaternion.Euler(0, Random.Range(0.0f, 360), 0) * dir;
        dir = dir * Random.Range(1.0f, 5.0f);
        Vector3 pos = startPos + dir;

        MoveToPos(pos, () =>
        {
            StopRoamCorutine();
            coRoam = StartCoroutine(Roaming());
        });

    }
    protected override void OnDead()
    {
        OnChangeState(State.Death);
    }
    public void onDisApear()
    {
        StartCoroutine(DisApear(0.3f));
    }
    IEnumerator DisApear(float downSpeed)
    {
        yield return new WaitForSeconds(2.0f);
        Vector3 dir = Vector3.down;
        float dist = 1.0f;
        while (dist > 0.0f)
        {
            float delta = downSpeed * Time.deltaTime;
            transform.Translate(dir * delta);
            dist -= delta;
            yield return null;
        }
        Destroy(gameObject);
    }
}
