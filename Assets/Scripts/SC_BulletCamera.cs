using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_BulletCamera : MonoBehaviour
{
    public SC_Bullet ScBullet;
    public TrailRenderer renderer;
    public void OnSpeedUPAnimation()
    {
        Time.timeScale = 1f;
       // int speed = ScBullet.distance
        ScBullet.speedMultiplier = ScBullet.distance*1.9f;
        ScBullet.bulletEffect.SetActive(false);

        ScBullet.bulletEffect.SetActive(true);

        renderer.enabled = true;
       // Debug.Log("OnSpeedUPAnimation");
        //if (GetComponentInParent<AudioSource>())
        //{
        //    GetComponentInParent<AudioSource>().Play();

        //}

    }
}
