using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameStates
{
    public static GAME_STATE currentState = GAME_STATE.EXIT, previousState = GAME_STATE.EXIT;
    public static CurrentState _currentState;
    static Hashtable States = new Hashtable();

    private static List<GAME_STATE> lastStates = new List<GAME_STATE>(); 

    public static void GoBack()
    {
        if (lastStates.Count > 0)
        {
            GAME_STATE s = lastStates[lastStates.Count - 1];

            lastStates.RemoveAt(lastStates.Count - 1);
            SetCurrent_State_TO(s);
        }
    }

    public static void SetCurrent_State_TO(GAME_STATE state)
    {
        if (currentState == state)
        {
            Debug.Log("Set GAMESTATE not required already in state: " + state.ToString());
            return;
        }

        previousState = currentState;

        if (!lastStates.Contains(previousState))
            lastStates.Add(previousState);

        if (GameManager.Instance)
            GameManager.Instance.IsBackKeyPressed = false;

        currentState = state;
        _currentState = States[currentState] as CurrentState;

        Debug.Log("Set GAMESTATE to : " + state.ToString());

        _currentState.Invoke();

          //ImagesLoader.ReleaseAllImages();
    }

    public static void RegisterStates(GAME_STATE enum_states, CurrentState deleg_State)
    {
        if (!States.ContainsKey(enum_states))
            States.Add(enum_states, (CurrentState)deleg_State);
    }
}

