using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class CanSeePlayer : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject Agent;
    [SharedRequired]
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Target destination.")]
    public SharedVector3 Destination;

    // cache the navmeshagent component
    private EcchiEnemy _enemy;
    private GameObject _target;

    public override void OnStart()
    {
        _enemy = Agent.Value.GetComponent<EcchiEnemy>();
    }
    public override TaskStatus OnUpdate()
    {
        List<GameObject> targetList = _enemy.CheckSight();
        if (targetList.Count > 0)
        {
            _target = targetList[0];
            if (_target != null)
            {
                Destination.Value = _target.transform.position;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
}
}