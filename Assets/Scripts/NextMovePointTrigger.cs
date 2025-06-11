using System;
using UnityEngine;

public class NextMovePointTrigger : MonoBehaviour
{
   public GameObject nextMovePoint;
   
   public TriggerType triggerType;
   public enum TriggerType
   {
      ActivateNextWave,
      ActivateNextPoint,
   }

   private void Start()
   {
      gameObject.SetActive(false);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         switch (triggerType)
         {
            case TriggerType.ActivateNextWave:
               BattleFieldWavesController.instance.ActivateNextWave();
               gameObject.SetActive(false);
               break;
            
            case TriggerType.ActivateNextPoint:
               nextMovePoint.SetActive(true);
               gameObject.SetActive(false);
               break;
         }
         
      }
   }
}
