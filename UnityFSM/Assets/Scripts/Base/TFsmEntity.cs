using UnityEngine;
using System.Collections;

/// <summary>
/// Parent class can be MonoBehaviour, or your custom BaseMonoBehaviour
/// </summary>
/// <typeparam name="STATES"></typeparam>
public class TFsmEntity<STATES> : BaseBehaviour {

    /// <summary>The actual state</summary>
    public STATES curentState_;
    /// <summary>Previous state</summary>
    public STATES previousState_;
    /// <summary>Print to the log every state change </summary>
    public bool logFsm = false;

    #region State update events

    /// <summary>
    /// The message can be send for every state change
    /// </summary>
    public class StateUpdateEvent : BaseEvent
    {
        public TFsmEntity<STATES> entity;   //< the state machine
        public STATES state;                //< new state
        public STATES previousState;        //< old state
        // <summary> Crate a state update message </summary>
        public StateUpdateEvent(TFsmEntity<STATES> ent) : base (ent)
        {
            this.entity = ent;
            this.state = ent.state;
            this.previousState = ent.previousState;
        }
    }
    /// <summary>Delegate for the state update event</summary>
    public delegate void StateUpdateDelegate(StateUpdateEvent evt);
    /// <summary>Every state change listeners</summary>
    public StateUpdateDelegate onStateUpdateListeners;
    /// <summary>Send every state change message to the delegate </summary>
    public bool sendEveryStateMessage = false;

    #endregion

    #region State's timer

    /// <summary>
    /// The state starts at.
    /// </summary>
    private float stateStartAt;
    /// <summary>
    /// Get time from this state start
    /// </summary>
    /// <value>The state Time.</value>
    protected float stateTime {
        get { return Time.time - stateStartAt; }
    }

    #endregion

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>The state.</value>
    public STATES state { get { return curentState_; } }
    /// <summary>
    /// Gets the previous state.
    /// </summary>
    /// <value>The state of the previous.</value>
    public STATES previousState { get { return previousState_; } }
    /// <summary>
    /// The on state exit will be executed.
    /// </summary>
    protected System.Action onStateExit;

    #region State's event handlers

    /// <summary>Delegate for message receiver by state</summary>
    public delegate void StateEventDelegate(BaseEvent evt);
    /// <summary>The state onEvent block</summary>
    protected StateEventDelegate onStateEvent;

    #endregion

    #region State's value

    /// <summary>
    /// The value transfered to the state. The method Go() can be used
    /// with or without additional argument (state value).
    /// </summary>
    protected object stateValue;

    #endregion

    /// <summary>
    /// External termination of the state. Can be used in unelected cases
    /// This method is same as Go() but guaranty to print the text message to
    /// the log.
    /// </summary>
    /// <param name="nextState">The next state</param>
    /// <param name="theValue">State's argument</param>
    /// <returns></returns>
    public object InterruptAndGo(STATES nextState, object theValue = null)
    {
        Debug.LogFormat("FSM.InterruptAndGo from state: {0}", curentState_);
        return Go(nextState, theValue);
    }
    /// <summary>Go the specified nextState</summary>
    /// <param name="nextState">The next state</param>
    /// <param name="theValue">State's argument</param>
    public object Go(STATES nextState, object theValue = null) 
    {
        StopAllCoroutines();
        if (onStateExit != null) onStateExit(); onStateExit = null;
        stateValue = theValue;
        previousState_ = curentState_;
        curentState_ = nextState;
        onStateEvent = null;
        if (logFsm)
            Debug.LogFormat("FSM.Go: {0}", curentState_);
        stateStartAt = Time.time;
        if (gameObject.activeSelf)
        {
            // Start coroutine of the state. This is most weak
            // part of code because there is the string as the function's
            // name used
            StartCoroutine(curentState_.ToString());
            // In case if you need FSM may send message every state change
            if (sendEveryStateMessage && onStateUpdateListeners != null)
                onStateUpdateListeners(new StateUpdateEvent(this));
        }
        return null;
    }
    /// <summary>Same as Go() but FSM stops after this method</summary>
    /// <param name="nextState">The next state</param>
    /// <param name="theValue">State's argument</param>
    /// <returns></returns>
    public object GoAndStop(STATES nextState, object theValue = null)
    {
        StopAllCoroutines();
        if (onStateExit != null) onStateExit(); onStateExit = null;
        stateValue = theValue;
        previousState_ = curentState_;
        curentState_ = nextState;
        onStateEvent = null;
        if (logFsm)
            Debug.LogFormat("FSM.GoAndStop: {0}", curentState_);
        // In case if you need FSM may send message every state change
        if (sendEveryStateMessage && onStateUpdateListeners != null)
            onStateUpdateListeners(new StateUpdateEvent (this));
        return null;
    }
    /// <summary>Go to previous state</summary>
    /// <returns>The back.</returns>
    public object GoBack()
    {
        if (logFsm)
            Debug.LogFormat("FSM.GoBack: {0}", previousState);
        return Go(previousState);
    }
    /// <summary>Starts the FSM</summary>
    /// <param name="initialState">Initial State</param>
    protected void StartFsm(STATES initialState, object theValue = null) 
    {
        curentState_ = initialState;
        stateValue = theValue;
        onStateEvent = null;
        if (logFsm)
            Debug.LogFormat("FSM.StartFsm: {0}", state);
        StartCoroutine(state.ToString());
    }
    /// <summary>This method allow to send event to the current state</summary>
    /// <param name="evt">Event data</param>
    public virtual void OnStateEvent(BaseEvent evt)
    {
        if (onStateEvent != null) onStateEvent(evt);
    }
    /// <summary>Return the state in user friendly format</summary>
    /// <returns>String value of the state</returns>
    override public string ToString() {
        return string.Format("{0}.{1}\n  state: '{2}' [{4}]\n  old: '{3}'\n", this.GetType().ToString(), name, state, previousState, stateTime.ToString("0.0"));
    }
}

/// <summary>
/// State machine with template class.
/// </summary>
/// <typeparam name="STATES">Enum type with states of this object</typeparam>
/// <typeparam name="TEMPLATE">Type of template of this object</typeparam>
public class TFsmEntity<STATES,TEMPLATE> : TFsmEntity<STATES>, System.IDisposable where TEMPLATE : BaseTemplate
{
    /// <summary>
    /// Pointer to template of this object. The template has data to initialize
    /// this object.
    /// </summary>
    public TEMPLATE template;
    /// <summary>
    /// Called immediately after object was spawned. Initialize the object
    /// by data from template.
    /// </summary>
    /// <param name="evt">OnSpawnEvent have the template pointer</param>
    public virtual void OnSpawned(object evt) {
        template = (evt as OnSpawnEvent).template as TEMPLATE;
    }
}
