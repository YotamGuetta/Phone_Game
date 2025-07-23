using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnumToAnimation", story: "Sets [Enum] Animation in [Animator]", category: "Action", id: "9a4e9c4b7404fa25a0a45648bdef4d6c")]
public partial class EnumToAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyAnimation> Enum;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    protected override Status OnUpdate()
    {
        int animationState = (int)Enum.Value;
        Animator.Value.SetInteger("State", animationState);

        return Status.Success;
    }


}

