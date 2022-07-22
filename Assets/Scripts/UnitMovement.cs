using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField]
    NavMeshAgent agent;
    
    #region Server

    [Command]
    void CmdMove(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    #endregion

    #region Client

    [SerializeField]
    Animator animator;

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority)
        {
            Debug.Log("Has no authority!");
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            
            CmdMove(hit.point);
            animator.Play("Run", 0);
        }
    }

    #endregion
}
