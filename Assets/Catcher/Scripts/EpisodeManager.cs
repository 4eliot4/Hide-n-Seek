using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{
    [SerializeField] System.String HiderTag = "Hider";
    [SerializeField] System.String Catcher = "Catcher";
    private Agent_hider hiderAgent;
    private Agent1 catcherAgent;
    // Start is called before the first frame update
    public void EndEpisodeForBothAgents()
    {
        hiderAgent.EndEpisode();
        catcherAgent.EndEpisode();
    }
}
