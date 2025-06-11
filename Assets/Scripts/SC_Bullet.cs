using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Bullet : MonoBehaviour
{
    public float speed= 1500;
    public float speedMultiplier=1;
    public float DetectorLength = 2000;
    public float runningLenght = 10;

    public GameObject hitCamera;
    bool targetDetected = false;
    private bool destroyed = false;
    public float DestroyDuration = 1;
    [HideInInspector]
//    public PlayerInputController playerContoller;
    public GameObject bulletEffect;
    public float distance;
    public GameObject realBullet;
    public float bulletActiveTime = 0.1f;
    bool hitDetected = false;
    bool isFastHit = false;
    public GameObject fastHitCamera;
    private void Start()
    {
//        if (FirstDetectTarget() && !isFastHit)
//        {
            hitCamera.SetActive(true);
            bulletEffect.SetActive(true);
            realBullet.SetActive(false);
            fastHitCamera.SetActive(false);

            Invoke("OnBulletMesh", bulletActiveTime);
            Time.timeScale = 0.5f;
            /*if (playerContoller.gunHanddle)
            {
                playerContoller.gunHanddle.CamerasActive(false);
            }*/
//        }
        /*else*/ if(isFastHit)
        {
            hitCamera.SetActive(false);
            fastHitCamera.SetActive(true);
            speedMultiplier = distance * 8;
            
            /*if (playerContoller.gunHanddle)
            {
                playerContoller.gunHanddle.CamerasActive(false);
            }*/
        }
        else
        {
            hitCamera.SetActive(false);
            speedMultiplier = 100;
//            playerContoller.OnBulletHit(0);
        }


    }

    void OnBulletMesh()
    {
        realBullet.SetActive(true);
    }
    void FixedUpdate()
    {

        float step = speed * Time.deltaTime; // calculate distance to move

        transform.Translate(Vector3.forward * step* speedMultiplier);

        if (!targetDetected && hitDetected)
        {
            RunningDetectTarget();
        }
        if (destroyed)
        {
            if (!targetDetected)
            {
//                playerContoller.gunHanddle.CamerasActive(false);
//                playerContoller.OnBulletHit(0);
            }
            GameObject.Destroy(this.gameObject, DestroyDuration);
        }
    }

    public bool  FirstDetectTarget()
    {

        RaycastHit hitcam;
        if (Physics.Raycast(transform.position, transform.forward, out hitcam, DetectorLength))
        {
//            SC_BulletTarget bulletHit = hitcam.collider.gameObject.GetComponent<SC_BulletTarget>();
//            if (bulletHit != null && bulletHit.HasAction )
//            {
//                hitDetected = true;
//                isFastHit = bulletHit.isToDeactive;
//                if (bulletHit.targetScore<=9 && MConstants.CurrentGameMode == MConstants.GAME_MODES.SINGLE_PLAYER)
//                {
//                    isFastHit = true;
//                    // bulletHit.isToDeactive = true;
//                }
//                else if ((bulletHit.targetScore <= 8 && Random.Range(0,100)<50) && MConstants.CurrentGameMode == MConstants.GAME_MODES.MULTI_PLAYER)
//                {
//                    isFastHit = true;
//                   
//                    // bulletHit.isToDeactive = true;
//                }
//
//            
//                distance = hitcam.distance;
//                bulletHit.FreezeObject(true);
//            }
        }

        return true;
//        return hitDetected;
    }


    public void RunningDetectTarget()
    {

        RaycastHit hitcam;
        if (Physics.Raycast(transform.position, transform.forward, out hitcam, runningLenght))
        {
//            SC_BulletTarget bulletHit = hitcam.collider.gameObject.GetComponent<SC_BulletTarget>();
//            if (bulletHit != null && bulletHit.HasAction)
//            {
//
//                targetDetected = true;
//                hitCamera.GetComponent<Animator>().enabled = false;
//                hitCamera.SetActive(false);
//                bulletHit.OnHit(hitcam, this);
//                realBullet.SetActive(false);
//                if (isFastHit)
//                {
//                    fastHitCamera.transform.parent = null;
//                    Destroy(fastHitCamera,1.5f);
//                }
//                else
//                {
//                    fastHitCamera.SetActive(false);
//                }
//                speedMultiplier = 50;
//                destroyed = true;
//            }
        }
    }
}
