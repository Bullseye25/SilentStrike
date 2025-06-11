using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterClone : MonoBehaviour
{
    public Transform headTransform;
    public TacticalAI.TargetScript playerTacticalAiTargetScript;

    public void SetTacticalAiTargetTransform()
    {
        playerTacticalAiTargetScript.targetObjectTransform = headTransform;
    }
}
