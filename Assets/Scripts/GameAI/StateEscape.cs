using System;
using System.Collections;
using UnityEngine;
using System.Diagnostics;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class StateEscape : State
    {

        public int destPoint = 0; //index of the destination point
        public float value = 0.4f;
        public float delaySeconds = 5.0f;
        public Stopwatch stopwatch = new Stopwatch();    
        public StateEscape(EnemyControllerCustomSP ecsp, StateMachine stateMachine) : base(ecsp, stateMachine)
        {

        }


        public override void Enter()
        {
            UnityEngine.Debug.Log("Entered Escape State");
            stopwatch.Start();
            ecsp.escapeHappened = true;
            ecsp.GetComponent<MeshRenderer>().material = ecsp.materialEscape;
        }


        public override void LogicUpdate()
        {
        
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
           
            if (stopwatch.ElapsedMilliseconds / 1000 >= delaySeconds)
            {
                stopwatch.Reset();
                stopwatch.Start();
                ecsp.bossEnemyHealth += value;
                ecsp.onDamage.Invoke(ecsp.bossEnemyHealth);
                stateMachine.ChangeState(ecsp.statePhase2);
                return;
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

    }
}
