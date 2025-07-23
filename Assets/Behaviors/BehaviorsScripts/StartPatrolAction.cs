using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StartPatrol", story: "Set enemy [Move] to a patrol position", category: "Action", id: "1c3fe09813b2fdabbbe431b479411955")]
public partial class StartPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Move;

    protected override Status OnStart()
    {
        Move.Value.startEnemyPatrol();
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        if (Move.Value.reachedPatrolPoint)
        {
            return Status.Success;
        }
        return Status.Running;
    }

}

