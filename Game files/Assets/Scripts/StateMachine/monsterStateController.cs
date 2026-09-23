using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterStateController : MonoBehaviour
{
    iState currentState;

    void Update()
    {
        if (currentState!=null)
        {
            currentState.updateState();
        }
    }
    public void changeState(iState newState)
    {
        currentState.onExit();
        currentState = newState;
        currentState.onEnter();
    }
}

public interface iState
{
    public void onEnter();
    public void updateState();
    public void onHurt();
    public void onExit();
}