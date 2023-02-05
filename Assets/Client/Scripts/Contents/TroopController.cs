using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TroopController : MonoBehaviour
{
    public List<Minion> minions;
    public Minion centerMinion;
    // Start is called before the first frame update
    void Start()
    {
        NavMesh.pathfindingIterationsPerFrame = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = InputManager.instance.dragAxis;
        if (direction.magnitude > 0)
        {
            foreach (var each in minions)
            {
                each.agent.stoppingDistance = 0;
                each.agent.SetDestination(each.transform.position + direction);

            }
        }
        else
            foreach (var each in minions)
            {
                if (each != centerMinion && each.agent.destination != centerMinion.transform.position)
                {
                    each.agent.stoppingDistance = 2.5f;
                    each.agent.SetDestination(centerMinion.transform.position);
                }
            }


    }
}
