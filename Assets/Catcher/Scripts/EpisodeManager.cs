using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{
    
    [SerializeField] Agent_hider hiderAgent;
    [SerializeField] Agent1 catcherAgent;
    private bool episodeEnded = false;
    void Start()
    {
       
    }
    
    // Start is called before the first frame update
    public void EndEpisodeForBothAgents(bool HiderWin)
    {
        if (HiderWin)
        {
            hiderAgent.AddReward(2.0f);
            catcherAgent.AddReward(-1.0f);
        }
        else
        {
            hiderAgent.AddReward(-1.0f);
            catcherAgent.AddReward(1f);
        }
        hiderAgent.EndEpisode();
        catcherAgent.EndEpisode();
        episodeEnded = true;
            
    }   

    public void resetEpisode()
    {
        episodeEnded = false;
    }
        
}
