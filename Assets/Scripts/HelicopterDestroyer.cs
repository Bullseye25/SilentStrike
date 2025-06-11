using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterDestroyer : MonoBehaviour
{
   public GameObject helicopter;
   public GameObject explosion;

   public void DestroyHelicopter()
   {
      StartCoroutine(DestroyDelay());
   }

   IEnumerator DestroyDelay()
   {
      Time.timeScale = 0.5f;
      var rb = helicopter.gameObject.AddComponent<Rigidbody>();
      rb.AddForce(-helicopter.transform.right * 30, ForceMode.Impulse);
      explosion.SetActive(true);

      yield return new WaitForSeconds(2f);

      HudMenuManager.instance.EnemyKilled();
   }
}
