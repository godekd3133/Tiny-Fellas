using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float collisionDetectRange = 0.5f;
    [SerializeField] private float movePerSec = 1f;
    [SerializeField] private float maxLifeTime = 10f;
    
    private GameObject targetObject;
    private float birthTime;

    private Action<GameObject, ProjectileBase> collisionCallBack;

    public virtual void Update()
    {
        if (targetObject == null) return;
        if (Time.time >= birthTime + maxLifeTime)
        {
            collisionCallBack(targetObject, this);
            return;
        }

       transform.position += (targetObject.transform.position - transform.position).normalized *
            (movePerSec * Time.deltaTime);
    }

    public void Fire(Action<GameObject, ProjectileBase> collisionCallBack, GameObject target)
    {
        birthTime = Time.time;
        this.collisionCallBack = collisionCallBack;
        targetObject = target;
    }
}
