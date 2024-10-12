using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents; 
using Unity.MLAgents.Sensors; 
using Unity.MLAgents.Actuators;

public class Agent_catcher_basic : Agent
{
    Rigidbody rBody;
    private float timer = 0f;           // Timer variable to track elapsed time
    [SerializeField] float maxTimePerEpisode = 20f;  // Maximum time allowed per episode (in seconds)
    [SerializeField] public Transform Target; // The target object
    [SerializeField] float forceMultiplier = 3;
    public float InputX;
    public float InputZ;
    

    void Start () {
        rBody = GetComponent<Rigidbody>(); // Get the Rigidbody component in ordee to apply forces, reset velocity...
    }

    
    public override void OnEpisodeBegin()
    {
       // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,0.5f,Random.value * 8 - 4);
        this.transform.localPosition = new Vector3(Random.value * 8 - 4,0.5f,Random.value * 8 - 4);

        rBody.velocity = Vector3.zero;  // Reset the velocity
    }
    public override void CollectObservations(VectorSensor sensor)
    {
    // Target and Agent positions
    sensor.AddObservation(Target.localPosition); // target position
    sensor.AddObservation(this.transform.localPosition); // agent position

    // Agent velocity
    sensor.AddObservation(rBody.velocity.x);
    sensor.AddObservation(rBody.velocity.z);
    // therefore we have 8 continuous observations
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];  // continuous action 0 mapping, we have 2 continuous actions in editor behaviour parameters.
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier); // this actually defines the continuous by applying force to the agent.
        
        

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        
        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        // Fell off platform
        else 
        {
            AddReward(-distanceToTarget * 0.01f);  // Negative reward based on distance
        }
         if (timer > maxTimePerEpisode)
         {
            SetReward(-1.0f); // Optional: Give a negative reward for taking too long
            EndEpisode();
         }
        
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

}
