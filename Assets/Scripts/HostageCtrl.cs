using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
public class HostageCtrl : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    public Transform Destination;
   
    // Update is called once per frame
    private void Start()
    {
        this.transform.SetParent(null);
        navMeshAgent = GetComponent<NavMeshAgent>();
      //  LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].isHostageLevel = true;
    }
    public void HostageRlease()
    {
    
         
            GetComponent<NavMeshAgent>().baseOffset = 0;
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("Run");
            navMeshAgent.SetDestination(Destination.position);
        Invoke("DisableHostage", 3f);
        //   LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].BoombIsDefusedStartOtherTimer();



    }

    void DisableHostage()
    {
        // MConstants.isPlayerWin = true;
        // HudMenuManager.instance.GameOver();
        this.gameObject.SetActive(false);
    }








}