
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class StateAttack : State
    {

        public StateAttack(EnemyControllerCustomSP ecsp, StateMachine stateMachine) : base(ecsp, stateMachine)
        {

        }


        public override void Enter()
        {
            Debug.Log("Entered Attack State");
            
            ecsp.GetComponent<MeshRenderer>().material = ecsp.materialAttack;
        }


        public override void LogicUpdate()
        {
            //Debug.Log("StatePatrol: LogicUpate");

            //check conditions for transitions
            //-> player outside of attack radius->go to chasing

            Vector3 dir_player = ecsp.targetTransform.position - ecsp.gameObject.transform.position;
            dir_player.y = 0;

            float d_player = dir_player.magnitude;

            if (d_player >= ecsp.radius)
            {
                Debug.Log("outside of attack range -> going back to chasing");
                SoundManager.Instance.PlaySound(SoundManager.Sound.Miss);
                ecsp.GetComponent<MeshRenderer>().material = ecsp.materialChase;
                stateMachine.ChangeState(ecsp.stateChase);
                return;
            }

            if(ecsp.bossEnemyHealth <= 0.25 && !ecsp.escapeHappened)
            {
                ecsp.GetComponent<MeshRenderer>().material = ecsp.materialEscape;
                stateMachine.ChangeState(ecsp.stateEscape);
                return;
            }
            //chase
            ecsp.rb.velocity += dir_player.normalized * ecsp.chaseAttackSpeed * Time.deltaTime;

        }


        public override void Exit()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Alert);
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
