using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WaveData : MonoBehaviour {
    public int WaveId,TotalEnemy;
    [Header("Sucide Boomber")]
    public int TotalSucideBumber;
    public int MinSucideShootCount, MaxSucideShootCount;
    [Header("Knife Enemy")]
    public int TotalknifeEnemy;
    public float MinknifeEnemyHealth, MaxknifeEnemyHealth;
    public float knifeEnemyDamageToPlayer;
    [Header("Gunner")]
    public int TotalGunner;
    public float MinGunnerHealth, MaxGunnerHealth;
    public float GunnerShootRange;
    public float GunnerDamageToPlayer;
    [Header("Helicopter Enemy")]
    public int TotalFlyingenemy;

}
