using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{

    public GameState playerActive;
    public float speed;
    private GameObject activeTarget;
    private bool transitionRunning;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (activeTarget != playerActive.ActiveActor.gameObject)
        {
            StopAllCoroutines();
            transitionRunning = true;
            StartCoroutine(MoveToTarget(playerActive.ActiveActor.transform.position));
            activeTarget = playerActive.ActiveActor.gameObject;
        }
        if (activeTarget == playerActive.ActiveActor.gameObject && !transitionRunning)
        {
            transform.position = playerActive.ActiveActor.transform.position;

            StartCoroutine(MoveToTarget(playerActive.ActiveActor.transform.position));
            activeTarget = playerActive.ActiveActor.gameObject;
        }


    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        targetPosition.z = transform.position.z;
        var currentDirection = targetPosition - transform.position;
        float currentDistance = currentDirection.magnitude;

        while (currentDistance > 0.1f)
        {
            currentDirection = transform.position - targetPosition;
            currentDistance = currentDirection.magnitude;

            var tempDirection = targetPosition - transform.position;
            tempDirection = tempDirection.normalized;
            tempDirection *= speed * Time.deltaTime;
            transform.position += tempDirection;
            yield return null;

        }
        transitionRunning = false;
    }
}