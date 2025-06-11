using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplayBombScript : MonoBehaviour
{
    public GameObject nextPoint;
    public GameObject bomb;
    public GameObject movePosition;
    public GameObject targetPoint;
    public GameObject ObjectToDeactivate;
    public GameObject extraObjectToActivate;
    private void OnEnable()
    {
        bomb.SetActive(true);
       // MovePlayerAndCamera.Instance.StartAttackAnimation(movePosition, targetPoint, 3, 2, false, Vector3.zero);
        StartCoroutine(StartBlast());
    }

  
     IEnumerator StartBlast()
    {
        yield return new WaitForSeconds(2);

        Camera.main.GetComponent<CameraControl>().playerObj.GetComponent<FPSPlayer>().WeaponEffectsComponent.ExplosionEffect(bomb.transform.position);
        gameObject.GetComponent<AudioSource>().Play();
        bomb.SetActive(false);

        ObjectToDeactivate.SetActive(false);
        extraObjectToActivate.SetActive(true);
        yield return new WaitForSeconds(1);
        if (nextPoint)
        {
            if (nextPoint.GetComponent<SCWave>())
            {
               nextPoint.GetComponent<SCWave>().StartWave();

            }
            nextPoint.SetActive(true);

        }
        gameObject.SetActive(false);

    }



}
