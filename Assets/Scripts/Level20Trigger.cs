using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level20Trigger : MonoBehaviour
{
   public CivilianAI[] hostages;

   private bool once;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 15 && !once)
      {
         once = true;
         GetComponent<BoxCollider>().enabled = false;
         foreach (var civilian in hostages)
         {
            civilian.GetComponent<Animator>().SetBool("kneel", true);
         }
      }
   }
}
