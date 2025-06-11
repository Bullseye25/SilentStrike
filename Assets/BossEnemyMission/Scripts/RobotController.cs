using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public Animator RobotAnimator;
   // public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public WaypointGroup Path;
    public float ShakeIntensity;
    [HideInInspector]
    public bool isMoving = true;
    public List<Transform> waypoints = new List<Transform>();
   Transform targetWaypoint;
   int StartWaypointIndex = 0;
   float minDistance = 2f; //If the distance between the enemy and the waypoint is less than this, then it has reacehd the waypoint
    public AudioSource RobotFootStep;
    //[HideInInspector]
    public bool Isminirobot,Isminijet;
    // int lastWaypointIndex;
    //   public float angle;
    NavMeshAgent myNavMeshAgent;

    public float stoppingDistance = 0f;
    // Use this for initialization//
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
       
        RobotAnimator.SetBool("Iswalk", true);

      
        if (Path)
        {
            waypoints = Path.wayPoints;
            targetWaypoint = waypoints[0];
        }
        //if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL)
        //{
        //    myNavMeshAgent.stoppingDistance = 12;
        //   // myNavMeshAgent.radius = 3f;
        //   // myNavMeshAgent.height = 3f;
        //   // myNavMeshAgent.avoidancePriority = 0;
        //    targetWaypoint = FPSPlayer.instance.gameObject.transform;
        //}
    }
    private void OnEnable()
    {
     
    }
    void Update()
    {
        //if (!Path&& MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL)
        //{
        //    RobotAnimator.SetBool("Isidle", true);
        //    RobotAnimator.SetBool("Iswalk", false);
        //    return;
        //}
        if (isMoving)
        {
          
            float rotationStep = rotationSpeed * Time.deltaTime;

            Vector3 directionToTarget = targetWaypoint.position - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
          
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

            Debug.DrawRay(transform.position, transform.forward * 50f, Color.green, 0f); //Draws a ray forward in the direction the enemy is facing
            Debug.DrawRay(transform.position, directionToTarget, Color.red, 0f); //Draws a ray in the direction of the current target waypoint
            //if ( MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL)
            //{
            //    myNavMeshAgent.SetDestination(targetWaypoint.transform.position);
            //}
            //else
            //{
                float distance = Vector3.Distance(transform.position, targetWaypoint.position);
                CheckDistanceToWaypoint(distance);
                myNavMeshAgent.SetDestination(targetWaypoint.transform.position);
            //}
              
            if (RobotAnimator.GetBool("Iswalk") == false)
            {
              
                myNavMeshAgent.isStopped = false;
               RobotAnimator.SetBool("Isidle", false);
               RobotAnimator.SetBool("Iswalk", true);
            }
          
        }
        //else if(!isMoving&& MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL)
        //{

        //    if (RobotAnimator.GetBool("Isidle") == false)
        //    {
        //        RobotAnimator.SetBool("Iswalk", true);
        //        RobotAnimator.SetBool("Isidle", true);
        //        myNavMeshAgent.isStopped = true;
        //    }
               
            
        //}
    }

    /// <summary>
    /// Checks to see if the enemy is within distance of the waypoint. If it is, it called the UpdateTargetWaypoint function 
    /// </summary>
    /// <param name="currentDistance">The enemys current distance from the waypoint</param>
    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            StartWaypointIndex++;
            UpdateTargetWaypoint();
        }
            
    }
 public void CameraShake()
    {
        if (HudMenuManager.instance.player && !Isminirobot)
        {
          //   ThirdPersonCamera.ShakeCam(ShakeIntensity, 1f);
        }
        RobotFootStep.Play();
    }
    /// <summary>
    /// Increaes the index of the target waypoint. If the enemy has reached the last waypoint in the waypoints list, it resets the targetWaypointIndex to the first waypoint in the list (causes the enemy to loop)
    /// </summary>
    void UpdateTargetWaypoint()
    {
        if (StartWaypointIndex > waypoints.Count - 1)
        {
            StartWaypointIndex = 0;
        }
       
        targetWaypoint = waypoints[StartWaypointIndex];
    }
}
