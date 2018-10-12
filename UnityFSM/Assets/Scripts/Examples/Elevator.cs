using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// hww - ru.linkedin.com/in/valeriyap

/// <summary>
/// Elevators states
/// </summary>
public enum ElevatorStates
{
    Undefined,
    WaitingState,
    MovingState
}

public class Elevator : TFsmEntity<ElevatorStates>
{
    /// <summary>
    /// Elevator's template with shared params
    /// </summary>
    protected ElevatorTemplate template;

    /// <summary>
    /// Curent level of the elevator
    /// </summary>
    protected int curentLevel;

    /// <summary>
    /// Just for demonstration of the state's onExit final code
    /// </summary>
    protected bool isMoving;

    #region STATE MACHINE

    IEnumerator WaitingState()
    {
        // ----------------------
        // On state event code
        // ----------------------

        onStateEvent = (BaseEvent evt) =>
        {
            ElevatorInputEvent e = evt as ElevatorInputEvent;
            if (e != null)
            {
                switch (e.name)
                {
                    case ElevatorInputEvent.EName.OnPushDownButton:
                        Go(ElevatorStates.MovingState, evt);
                        break;
                    case ElevatorInputEvent.EName.OnPushUpButton:
                        Go(ElevatorStates.MovingState, evt);
                        break;
                }
            }
        };

        yield break;
    }

    IEnumerator MovingState()
    {
        // ----------------------
        // On state exit code
        // ----------------------

        onStateExit = () =>
        {
            isMoving = false; // Just to demonstrate onExit finalizer
        };

        // ----------------------
        // On state enter code
        // ----------------------

        // Get state argument. Just to demonstrate the feature
        ElevatorInputEvent e = stateValue as ElevatorInputEvent;
        if (e == null)
            Go(ElevatorStates.WaitingState); // No - argument this is wrong

        // Check if elevator can move
        int oldLevel = curentLevel;
        switch (e.name)
        {
            case ElevatorInputEvent.EName.OnPushDownButton:
                curentLevel = GetAvailableLevel(curentLevel - 1);
                break;
            case ElevatorInputEvent.EName.OnPushUpButton:
                curentLevel = GetAvailableLevel(curentLevel + 1);
                break;
        }
        if (oldLevel == curentLevel)
            Go(ElevatorStates.WaitingState); // No - can't move

        // Yes - can move
        isMoving = true;

        float animTime = 0;
        float animDur = 1f / template.speed;
        float smothTime = animDur * 0.3f;

        Vector3 curentVelocity = Vector3.zero;
        Vector3 targetPosition = GetLevelPosition(curentLevel);
        substate = "while: animTime < animDur";  //< Just to demonstrate substate feture
        while (animTime < animDur)
        {
            // ----------------------
            // The state's arc code
            // ----------------------

            yield return null;
            animTime += Time.deltaTime;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref curentVelocity, smothTime);
        }

        Go(ElevatorStates.WaitingState);
    }

    #endregion

    #region UTILITIES

    /// <summary>
    /// Limit elevator's level to a possible value. 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    int GetAvailableLevel(int level)
    {
        return System.Math.Min(template.maxLevel,System.Math.Max(level, template.minLevel));
    }

    /// <summary>
    /// Calcaulate flore level
    /// </summary>
    /// <param name="level">Level number</param>
    /// <returns>Altitude</returns>
    Vector3 GetLevelPosition(int level)
    {
        return new Vector3(0, level * template.levelHeight, 0);
    }

    /// <summary>
    /// Imediately relocate elevator to this level
    /// </summary>
    /// <param name="level"></param>
    void SetLevel(int level)
    {
        curentLevel = GetAvailableLevel(level);
        transform.position = GetLevelPosition(curentLevel);
    }

    #endregion

    #region EVENTS

    // After spawn object. 
    public override void OnSpawned(object evt)
    {
        base.OnSpawned(evt);
        OnSpawnElevatorEvent e = evt as OnSpawnElevatorEvent;
        template = e.GetTemplate;                // now we can access to template
        tagString = name;                        // designate tag of FSM 
        SetLevel(e.level);
        StopAllCoroutines();
        StartFsm(ElevatorStates.WaitingState);   // start FSM here
    }

    // Before despawn object. Can be ommited. 
    public override void OnDespawned()
    {
        Display.I.text = "Elevator despawned";
    }

    // For demonstration only. Can be ommited.
    public override void OnStateEvent(BaseEvent evt)
    {
        // Before or after this function can be code executed for
        // each state include when state machine was not initialized
        base.OnStateEvent(evt);
    }

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// Just for demo
    /// </summary>
    void Update()
    {
        Display.I.text = string.Format("Elevator:\n  state: {0}:{1}\n  stateTime: {2}\n  level: {3}\n  isMoving: {4}", 
            state, substate, stateTime.ToString("0.00"), curentLevel, isMoving);
    }

    #endregion

}
