using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InGameManager : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Finish,
    }

    private event Action StartGame;
    private event Action FinishGame;

    private bool isGameFinished = false;

    private void Start()
    {
        StartGame.Invoke();
    }

    public void FinishedGame()
    {
        if(!isGameFinished)
        {
            FinishGame.Invoke();
            isGameFinished = true;
        }

    }

    public void UnsubscribeEvent(GameState state, Action action)
    {
        switch (state)
        {
            case GameState.Start:
                StartGame -= action;
                break;
            case GameState.Finish:
                FinishGame -= action;
                break;
        }
    }
    public void SubscribeEvent(GameState state, Action action)
    {
        switch (state)
        {
            case GameState.Start:
                StartGame += action;
                break;
            case GameState.Finish:
                FinishGame += action;
                break;
        }
    }
}