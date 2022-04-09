using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNoiseLocation : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject Agent;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Target destination.")]
    public SharedVector3 Destination;

    private EcchiEnemy _enemy;

    public override void OnStart()
    {
        _enemy = Agent.Value.GetComponent<EcchiEnemy>();
    }
public override TaskStatus OnUpdate()
	{
        if (_enemy.CheckForNoiseLocation() == Vector3.zero) return TaskStatus.Failure;

        Destination.Value = _enemy.CheckForNoiseLocation();
        return TaskStatus.Success;

    }
}