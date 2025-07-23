using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RangeDetector", story: "Update [Detector] and assign [player] if [Assinged]", category: "Action", id: "671760218c10ed5a65a9af43445f06f0")]
public partial class RangeDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyRangeDetector> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<bool> Assinged;


    protected override Status OnUpdate()
    {
        Player.Value = Detector.Value.checkForPlayer();
        Assinged.Value = Player.Value == null;
        return Status.Success;
    }


}

