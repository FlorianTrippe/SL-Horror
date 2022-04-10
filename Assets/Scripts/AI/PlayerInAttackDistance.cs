using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class PlayerInAttackDistance : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject Agent;
    [SharedRequired]
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The NavMeshAgent destination")]
    public SharedGameObject Player;
    [SharedRequired]
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The NavMeshAgent destination")]
    public SharedFloat MaxDistance;
    
    public override TaskStatus OnUpdate()
	{
        if (Vector3.Distance(Player.Value.transform.position, Agent.Value.transform.position) <= MaxDistance.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
}
}