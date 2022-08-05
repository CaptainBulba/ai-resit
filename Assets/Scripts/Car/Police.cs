using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : Car
{
    StateMachine stateMachine;

    protected override void Start()
    {
        base.Start();
        stateMachine = new StateMachine(new PoliceIdleState(this));
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.Execute();
    }

    public bool Chase(Sedan target)
    {
        Move(target.gameObject, false);
        return target.Captured(gameObject);
    }

    public bool FindClosestTarget()
    {
        GameObject targetSedan = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject sedan in fieldOfView.GetNearObstacles())
        {
            if (sedan.tag == gameManager.sedanTag)
            {
                float distance = Vector2.Distance(sedan.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetSedan = sedan;
                }
            }
        }
        target = targetSedan;

        return target != null ? true : false;
    }
}
