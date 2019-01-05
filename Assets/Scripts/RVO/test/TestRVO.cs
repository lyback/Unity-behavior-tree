using RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRVO : MonoBehaviour {
    public List<GameObject> player1;
    public List<GameObject> player2;
    public GameObject aim1;
    public GameObject aim2;
    public List<GameObject> obstacles;

    private List<RVO.Vector2> aims = new List<RVO.Vector2>();
    private List<GameObject> player = new List<GameObject>();
    // Use this for initialization
    void Start () {
        Setup();
    }
	
	// Update is called once per frame
	void Update () {
        if (!reachedGoal())
        {
            for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
            {
                var pos = Simulator.Instance.getAgentPosition(i);
                Vector3 v3 = player[i].transform.position;
                v3.x = pos.x();
                v3.z = pos.y();
                player[i].transform.position = v3;
            }
            setPreferredVelocities();
            Simulator.Instance.doStep();
        }
        
	}

    void Setup()
    {
        Simulator.Instance.setTimeStep(0.25f);
        
        Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 1f, 0.5f, new RVO.Vector2(0.0f, 0.0f));

        //player
        var aim = new RVO.Vector2(aim1.transform.position.x, aim1.transform.position.z);
        for (int i = 0; i < player1.Count; i++)
        {
            float x = player1[i].transform.position.x;
            float z = player1[i].transform.position.z;
            Simulator.Instance.addAgent(new RVO.Vector2(x, z));
            aims.Add(aim);
        }
        aim = new RVO.Vector2(aim2.transform.position.x, aim2.transform.position.z);
        for (int i = 0; i < player2.Count; i++)
        {
            float x = player2[i].transform.position.x;
            float z = player2[i].transform.position.z;
            Simulator.Instance.addAgent(new RVO.Vector2(x, z));
            aims.Add(aim);
        }
        player.AddRange(player1);
        player.AddRange(player2);

        //obstacle
        for (int i = 0; i < obstacles.Count; i++)
        {
            List<RVO.Vector2> ob = new List<RVO.Vector2>();
            int count = obstacles[i].transform.childCount;
            for (int j = 0; j < count; j++)
            {
                Transform child = obstacles[i].transform.GetChild(j);
                float x = child.position.x;
                float y = child.position.z;
                ob.Add(new RVO.Vector2(x, y));
            }
            Simulator.Instance.addObstacle(ob);
        }

        Simulator.Instance.processObstacles();
    }

    void setPreferredVelocities()
    {
        /*
         * Set the preferred velocity to be a vector of unit magnitude
         * (speed) in the direction of the goal.
         */
        for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
        {
            RVO.Vector2 goalVector = aims[i] - Simulator.Instance.getAgentPosition(i);

            if (RVOMath.absSq(goalVector) > 1.0f)
            {
                goalVector = RVOMath.normalize(goalVector);
            }

            Simulator.Instance.setAgentPrefVelocity(i, goalVector);

            /* Perturb a little to avoid deadlocks due to perfect symmetry. */
            float angle = Random.value * 5.0f * Mathf.PI;
            float dist = Random.value * 0.0005f;

            Simulator.Instance.setAgentPrefVelocity(i, Simulator.Instance.getAgentPrefVelocity(i) +
                dist * new RVO.Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        }
    }

    bool reachedGoal()
    {
        /* Check if all agents have reached their goals. */
        for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
        {
            if (RVOMath.absSq(Simulator.Instance.getAgentPosition(i) - aims[i]) > 10.0f)
            {
                return false;
            }
        }
        return true;
    }
}
