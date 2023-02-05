using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopController : MonoBehaviour
{
    public List<Minion> minions;
    public Minion centerMinion;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = InputManager.instance.dragAxis;
        if (direction.magnitude > 0)
        {
            foreach (var each in minions)
                each.agent.SetDestination(each.transform.position + direction * 100);
        }
        else
            foreach (var each in minions)
            {
                if (!each.agent.SetDestination(centerMinion.transform.position))
                    each.agent.Stop();
            }


    }
}
