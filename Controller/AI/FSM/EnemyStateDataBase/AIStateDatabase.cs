using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/FSM/State Database")]
public class AIStateDatabase : ScriptableObject
{
    public State startState = null;

    public List<State> enemyStates = null;  //즉 state를 따로 만드는게 아니라  Action, decisions은 스크립터블로 만들지만, 해당 상속받은 
                                            // OrcStateDataBase의 스크립터블을 생성하고 이 orc데이터베이트의 state에 등록하는식으로.
                                            // State가 여러개 있고 복잡할수도있으니 여기에 등록해서알아보기 싶게 함. 


    //start가 null이 아니면. 해당 state에 연결된 전환 state들 enemyStates에 추가하기??
    private void OnValidate()
    {
        SettingEnemyStatesList();
    }

    [ContextMenu("Register EnemyStates")]
    private void SettingEnemyStatesList()
    {
        if (startState == null) return;

        enemyStates.Clear();
        RegisterNewState(startState);

        Queue<State> statesToProcess = new Queue<State>(enemyStates);

        while (statesToProcess.Count > 0)
        {
            State currentState = statesToProcess.Dequeue();
            RegisterNewState(currentState);
        }
    }

    private void RegisterNewState(State state)
    {
        if (state == null) return;

        for (int i = 0; i < state.transitions.Length; i++)
        {
            if (state.transitions[i].trueState != null && !enemyStates.Contains(state.transitions[i].trueState))
                enemyStates.Add(state.transitions[i].trueState);

            if (state.transitions[i].falseState != null && !enemyStates.Contains(state.transitions[i].falseState))
                enemyStates.Add(state.transitions[i].falseState);
        }
    }
}

