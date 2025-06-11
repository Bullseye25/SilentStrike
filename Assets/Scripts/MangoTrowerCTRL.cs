using DG.Tweening;
using UnityEngine;

public class MangoTrowerCTRL : MonoBehaviour
{
    public GameObject Mango;
    public GameObject Bomb;
    public GameObject CatchPoint;
    public float SpawnDuration;
    public float JumHightStart;
    public float JumHightEnd;
    public float FlightDuration;

    void Start()
    {
        InvokeRepeating("throwMango", 2, SpawnDuration);
    }

    // Update is called once per frame
    void throwMango()
    {
        GameObject obj;
        int Ran = Random.Range(0, 20);
        if (Ran > 12)
        {
            obj = Instantiate(Bomb, transform.position, Quaternion.identity) as GameObject;

        }
        else
        {
            obj = Instantiate(Mango, transform.position, Quaternion.identity) as GameObject;

        }
        obj.transform.DOJump(CatchPoint.transform.position, Random.Range(JumHightStart, JumHightEnd), 1, FlightDuration);
        obj.transform.DOLocalRotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360).SetLoops(-1).SetAutoKill(false).SetEase(Ease.Linear);
        if (obj.GetComponent<MangoCtrl>())
        {
            obj.GetComponent<MangoCtrl>().DestroyMango(FlightDuration);

        }
        if (obj.GetComponent<BompCtrl>())
        {
            obj.GetComponent<BompCtrl>().DestroyMango(FlightDuration-0.5f);
        }
    }
   
}
