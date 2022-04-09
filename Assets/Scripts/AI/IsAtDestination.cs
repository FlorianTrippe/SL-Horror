using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsAtDestination : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject Agent;
    [SharedRequired]
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The NavMeshAgent destination")]
    public SharedVector3 Destination;
    [SharedRequired]
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The NavMeshAgent destination")]
    public SharedFloat DistanceTolerance;

    private EcchiEnemy _enemy;

    public override void OnStart()
    {
        _enemy = Agent.Value.GetComponent<EcchiEnemy>();
    }

    public override TaskStatus OnUpdate()
    {
        float i = Vector3.Distance(Agent.Value.transform.position, Destination.Value);
        if (i <= DistanceTolerance.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
}
}