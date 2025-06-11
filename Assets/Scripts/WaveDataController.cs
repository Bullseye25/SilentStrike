using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveDataController : MonoBehaviour
{
    public static WaveDataController Instance;

    public List<WaveData> Waves;
    [Header("Sucide Boomber Prefab")]
    public GameObject[] SucideBoomberPrefab;
    [Header("Fly Enemy Prefab")]
    public GameObject[] FlyingEnemyPrefab;
    [Header("knifeEnmy Prefab")]
    public GameObject[]  knifeEnmyPrefab;
    [Header("GunnerPrefab Prefab")]
    public GameObject[]GunnerPrefab;
    [Header("Temporary Enemycount After Wave limit")]
    int TempsucideBoomber,TempknifeEnemy,TempGunner,TempTotalEnemy,TemptotalFlyEnemy;
    public int WaveId ;
    [Header("Dropable Gun")]
    public GameObject[] Dropablegun;
    [Header("Dropable Gun")]
    public GameObject ParticlePrefab;
    public Vector3 enemySpawningPostion;

    int spawninge;
  
    private Vector3 SpawnPosition;
    public int DistanceToplayer= 5;
    public float  spherecastradias;
    int count = 0;
    private void Awake()
    {

          WaveId = 0;
        Instance = this;
    }
    private void Start()
    {
        RandomSpawner();
    }
    public void RandomSpawner()
    {

        RaycastHit hitdata;
        SpawnPosition = HudMenuManager.instance.player.gameObject.transform.forward * DistanceToplayer + HudMenuManager.instance.player.gameObject.transform.position;

      Vector3 randomRadius = SpawnPosition+new Vector3(Random.Range(10f,50f), Random.Range(2f, 20f), Random.Range(2f, 20f))  ;
      

       SpawnPosition =  randomRadius;
        
        Physics.SphereCast(SpawnPosition + (Vector3.up * 500f), spherecastradias, Vector3.down, out hitdata, 600f);
      
            if (hitdata.collider && (hitdata.collider.gameObject.layer == 0)  )
        {
           
            float dist = Vector3.Distance(hitdata.point, HudMenuManager.instance.player.gameObject.transform.position);
            if (dist > 8)
            {
                Debug.Log("hasTarget =========================");
                enemySpawningPostion = hitdata.point;
                StartCoroutine("SpowningWait");
                count = 0;
            }
            else
            {
                if (count < 10)
                {
                    count++;
                    //   Debug.Log("hitobjname"+hitdata.collider.gameObject.name);
                    RandomSpawner();
                }
                else
                {

                    randmspowning();
                }
            }
        }
        else
        {
            if (count < 10)
            {
                count++;
                //   Debug.Log("hitobjname"+hitdata.collider.gameObject.name);
                RandomSpawner();
            }
            else
            {
                randmspowning();
            }
        }
    }
    void randmspowning()
    {
        int randpoint = Random.Range(0, LevelsManager.instance.survivalModeSpwanPoints.Length);
        float dist = Vector3.Distance(LevelsManager.instance.survivalModeSpwanPoints[randpoint].transform.position, HudMenuManager.instance.player.gameObject.transform.position);
        if (dist > 12)
        {
            enemySpawningPostion = LevelsManager.instance.survivalModeSpwanPoints[randpoint].transform.position;
            StartCoroutine("SpowningWait");
            count = 0;
        }
        else
        {
            if(randpoint+1> LevelsManager.instance.survivalModeSpwanPoints.Length)
            {
                enemySpawningPostion = LevelsManager.instance.survivalModeSpwanPoints[randpoint - 1].transform.position;
            }
            else
            {
                enemySpawningPostion = LevelsManager.instance.survivalModeSpwanPoints[randpoint + 1].transform.position;
            }
            StartCoroutine("SpowningWait");
            count = 0;
        }
    }
    public void wavecount()
    {
        if (Waves[WaveId].TotalEnemy>1){
            Waves[WaveId].TotalEnemy--;
        }
        else
        {
            if (WaveId <Waves.Count-1)
            {

                WaveId++;
              TempTotalEnemy = Waves[WaveId].TotalEnemy;
                TempGunner= Waves[WaveId].TotalGunner;
                TempknifeEnemy = Waves[WaveId].TotalknifeEnemy;
                TempsucideBoomber = Waves[WaveId].TotalSucideBumber;
                TemptotalFlyEnemy= Waves[WaveId].TotalFlyingenemy;
                RandomSpawner();
               

            }
            else
            {
               
                Waves[WaveId].TotalEnemy = TempTotalEnemy;
                Waves[WaveId].TotalSucideBumber = TempsucideBoomber;
                Waves[WaveId].TotalGunner = TempGunner;
                Waves[WaveId].TotalknifeEnemy = TempknifeEnemy;
                Waves[WaveId].TotalFlyingenemy = TemptotalFlyEnemy;

                Waves[WaveId].MaxSucideShootCount += (Waves[WaveId].MaxSucideShootCount / 100) * 5;
                Waves[WaveId].MinSucideShootCount += (Waves[WaveId].MinSucideShootCount / 100) * 5;
                Waves[WaveId].MinknifeEnemyHealth += (Waves[WaveId].MinknifeEnemyHealth / 100) * 5;
                Waves[WaveId].MaxknifeEnemyHealth += (Waves[WaveId].MaxknifeEnemyHealth / 100) * 5;
                Waves[WaveId].MinGunnerHealth += (Waves[WaveId].MinGunnerHealth / 100) * 5;
                Waves[WaveId].MaxGunnerHealth += (Waves[WaveId].MaxGunnerHealth / 100) * 5;
                RandomSpawner();

            }
           
        }
    }
   
    IEnumerator SpowningWait()
    {
        yield return new WaitForSeconds(1f);
        HandelWaves();
    }
    void Sucideboomberspawn()
    {
        if (Waves[WaveId].TotalSucideBumber > 0 )
        {
            Waves[WaveId].TotalSucideBumber--;
            Instantiate(ParticlePrefab, enemySpawningPostion, Quaternion.Euler(-90f,0,0));
            int sucidRandInstaniate = Random.Range(0, SucideBoomberPrefab.Length);
            int Shootcount = Random.Range(Waves[WaveId].MinSucideShootCount, Waves[WaveId].MaxSucideShootCount);
            

            SucideBoomberPrefab[sucidRandInstaniate].GetComponent<SocideBoomber>().ShootCount = Shootcount;
                Instantiate(SucideBoomberPrefab[sucidRandInstaniate], enemySpawningPostion, Quaternion.identity);
               
            RandomSpawner();
        }
        else
        {
            HandelWaves();
        }
    }
    void HelicopterSpawn()
    {
        if (Waves[WaveId].TotalFlyingenemy > 0)
        {
            Waves[WaveId].TotalFlyingenemy--;

            int HeliRandInstaniate = Random.Range(0, FlyingEnemyPrefab.Length);



          
            Instantiate(FlyingEnemyPrefab[HeliRandInstaniate], enemySpawningPostion+new Vector3(Random.Range(-30f, 30f), Random.Range(10f, 20f), 0f), Quaternion.identity);

            RandomSpawner();
        }
        else
        {
            HandelWaves();
        }
    }
    void KnifeEnemySpawn()
    {
        if (Waves[WaveId].TotalknifeEnemy > 0)
        {
            Instantiate(ParticlePrefab, enemySpawningPostion, Quaternion.Euler(-90f, 0, 0));
            Waves[WaveId].TotalknifeEnemy--;
                int RandInstaniate = Random.Range(0, knifeEnmyPrefab.Length);
                float hitPoints = Random.Range(Waves[WaveId].MinknifeEnemyHealth, Waves[WaveId].MaxknifeEnemyHealth);

                knifeEnmyPrefab[RandInstaniate].GetComponent<CharacterDamage>().hitPoints = hitPoints;
                knifeEnmyPrefab[RandInstaniate].GetComponent<NPCAttack>().damage = Waves[WaveId].knifeEnemyDamageToPlayer;
                Instantiate(knifeEnmyPrefab[RandInstaniate], enemySpawningPostion, Quaternion.identity);
            RandomSpawner();
        }
        else
        {
            HandelWaves();
        }
    }
    void GunnerSpawn()
    {
        if (Waves[WaveId].TotalGunner > 0)
        {
            Instantiate(ParticlePrefab, enemySpawningPostion, Quaternion.Euler(-90f, 0, 0));
            Waves[WaveId].TotalGunner--;
                int RandInstaniate = Random.Range(0, GunnerPrefab.Length);
                float hitPoints = Random.Range(Waves[WaveId].MinGunnerHealth, Waves[WaveId].MaxGunnerHealth);
                GunnerPrefab[RandInstaniate].GetComponent<CharacterDamage>().hitPoints = hitPoints;
                GunnerPrefab[RandInstaniate].GetComponent<NPCAttack>().damage = Waves[WaveId].GunnerDamageToPlayer;
                GunnerPrefab[RandInstaniate].GetComponent<AI>().shootRange = Waves[WaveId].GunnerShootRange;
                Instantiate(GunnerPrefab[RandInstaniate], enemySpawningPostion, Quaternion.identity);
            RandomSpawner();
        }
        else
        {
            HandelWaves();
        }
    }
    public void HandelWaves()
    {
        if (Waves[WaveId].TotalSucideBumber > 0 || Waves[WaveId].TotalknifeEnemy > 0 ||Waves[WaveId].TotalGunner > 0 || Waves[WaveId].TotalFlyingenemy > 0)
        {
            //spawninge = Random.Range(0, enemySpawning.Length);
            
            int RandamSpaning = Random.Range(0, 4);
           
            switch (RandamSpaning)
            {
                
                case 0:
                    Sucideboomberspawn();
                    break;
                case 1:
                    KnifeEnemySpawn();
                    break;
                case 2:
                    GunnerSpawn();
                    break;
                case 3:
                    HelicopterSpawn();
                    break;
            }
           // StartCoroutine(" RandomSpawner");
        }
      




    }
}