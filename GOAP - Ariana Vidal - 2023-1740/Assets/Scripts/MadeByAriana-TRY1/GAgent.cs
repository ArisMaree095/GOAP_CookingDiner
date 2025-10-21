using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove; //some agents will want to remove their subgoals but we might need goals that can't be removed until truly satisfied

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>(); //list of actions>
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates beliefs;

    public GameObject customerToServe;
    public GameObject currentTable;

    GPlanner planner;
    Queue <GAction> actionsQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GWorld.Instance.ClearWorld();

        beliefs = new WorldStates(); //create beliefs
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a); //add in actions list
        }
    }

    private bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    private void LateUpdate()
    {       
    if (currentAction != null && currentAction.running)
    {
        if (currentAction.agent.hasPath && currentAction.agent.remainingDistance< 2.0f)
        {
            if (!invoked)
            {
                Invoke("CompleteAction", currentAction.duration);
                invoked = true;
            }
        } 
        return;
    }

    // 2. If no action is running, find a plan.
    if (planner == null || actionsQueue == null)
{
    planner = new GPlanner();
    var sortedGoals = from entry in goals orderby entry.Value descending select entry;

    foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
    {
        WorldStates combinedStates = new WorldStates();
        foreach (var state in GWorld.Instance.GetWorld().GetStates())
        {
            combinedStates.SetState(state.Key, state.Value);
        }
        foreach (var belief in beliefs.GetStates())
        {
            combinedStates.SetState(belief.Key, belief.Value);
        }

        actionsQueue = planner.plan(actions, sg.Key.sgoals, combinedStates);

        if (actionsQueue != null)
        {
            currentGoal = sg.Key;
            break;
        }
    }
}

if (actionsQueue != null && actionsQueue.Count == 0)
{
    if (currentGoal.remove)
    {
        goals.Remove(currentGoal);
    }
    planner = null;
}

if (actionsQueue != null && actionsQueue.Count > 0)
{
    currentAction = actionsQueue.Dequeue();
    if (currentAction.PrePerform())
    {
        if (currentAction.target == null && currentAction.targetTag != "")
        {
            currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
        }

                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
                else if (currentAction.duration > 0)
                {
                    currentAction.running = true;
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
                else
                {
                    Debug.LogError("Action " + currentAction.actionName + " has no target, no targetTag, and no duration! Cannot execute.");
                    actionsQueue = null;
                    planner = null;
                }
            }
    else
    {
        actionsQueue = null;
        planner = null;
    }
}

    }
}
