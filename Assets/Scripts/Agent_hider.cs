using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents; 
using Unity.MLAgents.Sensors; 
using Unity.MLAgents.Actuators;

public class Agent_hider : Agent
{
    
    public EpisodeManager episodeManager;
    
    private float timer = 0f;           // Timer variable to track elapsed time
    [SerializeField] float maxTimePerEpisode = 20f;  // Maximum time allowed per episode (in seconds)
    [SerializeField] System.String TargetTag = "Catcher"; // The target object
    private Transform Target; // The target object
    private Transform AgentArea;
    
    [SerializeField] float speed = 5f;
    private Vector3 lastPosition;
    
    void Start () {
        //rBody = GetComponent<Rigidbody>(); // Get the Rigidbody component in ordee to apply forces, reset velocity...
        
        AgentArea = transform.parent;
        Target = AgentArea.Find(TargetTag);
        if (Target == null)
        {
            Debug.LogError("Target Catcher not found! Make sure the target is assigned and tagged correctly.");
        }
    }

    
    public override void OnEpisodeBegin()
    {
        episodeManager.resetEpisode();
        // local position with respect to the parent object
        this.transform.localPosition = new Vector3(Random.value * 13 - 4,0.5f,Random.value * 13 - 4);

        
        timer = 0f;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
    // Target and Agent positions
    sensor.AddObservation(Target.localPosition); // target position
    sensor.AddObservation(this.transform.localPosition); // agent position

    // Agent velocity
    //sensor.AddObservation(rBody.velocity.x);
    //sensor.AddObservation(rBody.velocity.z);
    // therefore we have 8 continuous observations
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
         // Actions, size = 2
        float moveX = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        float moveZ = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        // Create movement vector
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * speed * Time.deltaTime;

        // Update position
        transform.localPosition += movement;
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        
        
        // Reached target
        if (distanceToTarget < 1.42f)
        {
            episodeManager.EndEpisodeForBothAgents(false); // false -> Hider loose
        }
        // Fell off platform
        
        float previousDistance = Vector3.Distance(lastPosition, Target.position);
        float currentDistance = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if(previousDistance < currentDistance)
        {
            AddReward(0.01f);
        }
        else
        {
            AddReward(-0.01f);
        }
        if (Vector3.Distance(lastPosition, this.transform.localPosition) < 0.01f)
        {
            AddReward(-0.01f); // Penalize for being idle
        }

        lastPosition = this.transform.localPosition;  
        timer += Time.deltaTime;  // Increment the timer
        if (timer > maxTimePerEpisode)
        {
            episodeManager.EndEpisodeForBothAgents(true); // true -> Hider wins
        }
        
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

}
