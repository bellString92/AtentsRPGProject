using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate bool CheckAction<T>(T v);

public class Movement : AnimatorProperty
{
    public float moveSpeed = 1.0f;
    public float RotSpeed = 720.0f;
    protected Coroutine coMove = null;
    Coroutine coRotat = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MovingToPos(Vector3 pos, UnityAction done) // 지정된 위치로 이동시키기
    {
        myAnim.SetBool("IsMoving", true);
        Vector3 moveDir = pos - transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        coRotat = StartCoroutine(Rotating(moveDir));

        while (moveDist > 0.0f)
        {
            if (myAnim.GetBool("IsAttacking") == false)
            {
                float delta = moveSpeed * Time.deltaTime;
                if (delta > moveDist) delta = moveDist;
                transform.Translate(moveDir * delta, Space.World);
                moveDist -= delta;
            }
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }
    IEnumerator Rotating(Vector3 dir) // 
    {
        float angle = Vector3.Angle(transform.forward, dir);
        float rotDir = 1.0f;
        if (Vector3.Dot(transform.right, dir) < 0.0f)
        {
            rotDir = -1.0f;
        }
        while (angle > 0.0f)
        {
            if (myAnim.GetBool("IsAttacking") == false)
            {
                float delta = RotSpeed * Time.deltaTime;
                if (delta > angle) delta = angle;
                transform.Rotate(Vector3.up * rotDir * delta);
                angle -= delta;
            }
            yield return null;
        }
    }
    public void StopMoveingCoroutine()
    {
        if (coMove != null)
        {
            StopCoroutine(coMove);
            coMove = null;
        }
        if (coRotat != null)
        {
            StopCoroutine(coRotat);
            coRotat = null;
        }
        myAnim.SetBool("IsMoving", false);
        myAnim.SetBool("IsRunning", false);
        moveSpeed = 1.0f;
    }

    public Coroutine MoveToPos(Vector3 pos, UnityAction done)
    {
        StopMoveingCoroutine();
        return coMove = StartCoroutine(MovingToPos(pos, done));
    }

    protected virtual void UpdateTargetPos(out Vector3 dir, out float dist, Transform target)
    {
        dir = target.position - transform.position;
        dist = dir.magnitude;
        dir.Normalize();
    }
    protected float TargetDist(Transform target)
    {
        return Vector3.Distance(transform.position, target.position);
    }
    protected IEnumerator FollowingTarget(Transform target, CheckAction<float> checkAct, UnityAction act)
    {
        myAnim.SetBool("IsMoving", false);
        myAnim.SetBool("IsRunning", true);
        moveSpeed = 3.0f;
        while (target != null)
        {
            UpdateTargetPos(out Vector3 dir, out float dist, target);
            float delta = 0.0f;

            if (checkAct != null && checkAct.Invoke(TargetDist(target)))
            {
                myAnim.SetBool("IsRunning", false);
                act?.Invoke();
            }
            else if (myAnim.GetBool("IsAttacking") == false)
            {
                myAnim.SetBool("IsRunning", true);
                delta = moveSpeed * Time.deltaTime;
                if (delta > dist) delta = dist;
                transform.Translate(dir * delta, Space.World);
            }
            float angle = Vector3.Angle(transform.forward, dir);
            float rotdir = Vector3.Dot(transform.right, dir) < 0.0f ? -1.0f : 1.0f;

            delta = RotSpeed * Time.deltaTime;
            if (delta > angle) delta = angle;

            transform.Rotate(Vector3.up * delta * rotdir);

            yield return null;

        }
        myAnim.SetBool("IsRunning", false);
    }
}
