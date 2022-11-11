
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class StatePatrol:State
    {

        public int destPoint = 0; //index of the destination point


        public StatePatrol(EnemyControllerCustomSP ecsp, StateMachine stateMachine) : base(ecsp, stateMachine)
        {
       
        }


        public override void Enter()
        {
            Debug.Log("Entered Patrolling State");
          
            ecsp.bossHealthBar.SetActive(false);
            ecsp.GetComponent<MeshRenderer>().material = ecsp.materialPatrol;
        }


        public override void LogicUpdate()
        {
            //Debug.Log("StatePatrol: LogicUpate");

            //check conditions for transitions
            //-> player within r_start -> go to chasing
            Vector3 dir_player = ecsp.targetTransform.position - ecsp.gameObject.transform.position;
            dir_player.y = 0;

            float d_player = dir_player.magnitude;

            if (d_player < ecsp.radiusStart)
            {
                //go chasing
                Debug.Log("within chasing range -> starting to chase");
                stateMachine.ChangeState(ecsp.stateChase);

                return;
            }

            //handle state update
            //->check radius
            Vector3 dir_waypoint = ecsp.points[destPoint].position - ecsp.gameObject.transform.position; //check direction of dest. and player
            dir_waypoint.y = 0;

            float d_waypoint = dir_waypoint.magnitude;
            //Debug.Log("d_wp: " + d_waypoint);
            if (d_waypoint <= ecsp.radiusWaypoint)
            {

                GotoNextPoint();

            }
            else
            {

                ecsp.rb.velocity += dir_waypoint.normalized * ecsp.patrolSpeed * Time.deltaTime;

            }

            //break;

        }


        public override void Exit()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Alert);
        }


        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (ecsp.points.Length == 0)
                return;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % ecsp.points.Length;
            //Debug.Log("next index: " + destPoint);
        }


        //void OnDrawGizmos()
        //{
        //    //Debug.Log("OnDrawGizmos");

        //    Gizmos.color = Color.white;
        //    Gizmos.DrawSphere(ecsp.points[destPoint].position, 0.5f);

 
        //    Gizmos.color = Color.blue;

        //    Gizmos.DrawWireSphere(ecsp.transform.position, ecsp.radiusStart);

        //}


    }
} 
