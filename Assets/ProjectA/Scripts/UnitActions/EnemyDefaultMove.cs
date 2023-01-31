using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefaultMove : MonoBehaviour
{
    private Vector2 moveDir;
    private float moveSpeed;

    void Update()
    {
        Vector3 movePlus = new Vector3(moveDir.x, moveDir.y, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movePlus);
    }
    public void Setup(Vector2 _moveDir, float _moveSpeed)
    {
        moveDir = _moveDir;
        moveSpeed = _moveSpeed;
    }
}
