
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class StateChase : State
    {

        public StateChase(EnemyControllerCustomSP ecsp, StateMachine stateMachine) : base(ecsp, stateMachine)
        {

        }


        public override void Enter()
        {
            ecsp.bossHealthBar.SetActive(true);
            
            ecsp.GetComponent<MeshRenderer>().material = ecsp.materialChase;
        }


        public override void LogicUpdate()
        {
            //Debug.Log("StatePatrol: LogicUpate");

            //check conditions for transitions
            //-> player outside of rstart -> go to patrol
            Vector3 dir_player = ecsp.targetTransform.position - ecsp.gameObject.transform.position;
            dir_player.y = 0;

            float d_player = dir_player.magnitude;

            if (d_player >= ecsp.radiusStart)
            {
                Debug.Log("outside of chase range -> going back to patrolling");
                SoundManager.Instance.PlaySound(SoundManager.Sound.Miss);
                ecsp.GetComponent<MeshRenderer>().material = ecsp.materialPatrol;
                stateMachine.ChangeState(ecsp.statePatrol);
                return;

            }
            else if (d_player < ecsp.radius && !ecsp.escapeHappened)
            {

                Debug.Log("within attack range -> starting attack");
                SoundManager.Instance.PlaySound(SoundManager.Sound.Alert);
                ecsp.GetComponent<MeshRenderer>().material = ecsp.materialAttack;
                stateMachine.ChangeState(ecsp.stateAttack);
                return;
            }
            else if (d_player < ecsp.radius && ecsp.escapeHappened)
            {

                Debug.Log("within attack range -> starting attack");
                SoundManager.Instance.PlaySound(SoundManager.Sound.Alert);
                ecsp.GetComponent<MeshRenderer>().material = ecsp.materialAttack;
                stateMachine.ChangeState(ecsp.statePhase2);
                return;
            }
            if (ecsp.bossEnemyHealth <= 0.25 && !ecsp.escapeHappened)
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
