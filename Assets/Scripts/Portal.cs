using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portal;
    public GameObject player;

    private float secondsToWait = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && StatusController.Instance.teleported == false)
        {
            Debug.Log("Player collided Portal");
            StatusController.Instance.teleported = true;
            player.transform.position = portal.transform.position;
            StartCoroutine(WaitTeleport(secondsToWait));
        }
    }
    IEnumerator WaitTeleport(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        StatusController.Instance.teleported = false;
    }
}
