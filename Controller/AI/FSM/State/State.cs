using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/FSM/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;

    public Color stateColor = Color.black;

    public void UpdateAction(AIController controller, float deltaTime)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller, deltaTime);
        }
    }

    public void UpdateCheckTransition(AIController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decision = transitions[i].decision.Decide(controller);
            if (decision)
            {
                if (controller.TransitionToState(transitions[i].trueState))
                    return;
            }
            else
            {
                if (controller.TransitionToState(transitions[i].falseState))
                    return;
            }
        }
    }



    public void OnEnterInit(AIController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].OnEnterAction(controller);
        }
        for (int i = 0; i < transitions.Length; i++)
        {
            transitions[i].decision.OnInitDecide(controller);
        }
    }

    public void OnExitInit( AIController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            //Debug.Log("<color=red> Exit : " + actions[i].name + "</color>");
            actions[i].OnExitAction(controller);
        }
        
    }


    public void DrawDecisionGizmos(AIController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
            transitions[i].decision.DrawDecisionGizmos(controller);
    }

}
