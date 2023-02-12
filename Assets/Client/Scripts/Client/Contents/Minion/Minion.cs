using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(MinionInstanceStat))]
public class Minion : MonoBehaviour
{
    public PlayerData ownerPlayer;
    public float moveSpeed;
    public NavMeshAgent agent;


    [HideInInspector] public UnityEvent onStatChanged;
    public MinionInstanceStat stat;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
