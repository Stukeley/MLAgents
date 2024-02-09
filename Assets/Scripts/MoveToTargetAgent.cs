using System;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToTargetAgent : Agent
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private GameObject text;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(1f, 5f), 0.5f, UnityEngine.Random.Range(5f, 9f));
        target.position = new Vector3(UnityEngine.Random.Range(1f, 5f), 0.5f, UnityEngine.Random.Range(5f, 9f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        float moveSpeed = 5f;
        
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            AddReward(10f);
            text.GetComponent<TextMeshProUGUI>().text = "Last result: Success!";
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            AddReward(-2f);
            text.GetComponent<TextMeshProUGUI>().text = "Last result: Hit the wall!";
            EndEpisode();
        }
    }
}
