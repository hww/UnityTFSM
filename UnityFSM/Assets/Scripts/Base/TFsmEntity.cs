using UnityEngine;
using System.Collections;

/// <summary>
/// Parent class can be MonoBehaviour, or your custom BaseMonoBehaviour
/// </summary>
/// <typeparam name="STATES"></typeparam>
public class TFsmEntity<STATES> : BaseFSM {

    /**
     * State update events
     */
#if USE_STATE_UPDATE_DELEGATE
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
            this.previousState = ent.prevState;
        }
    }
    /// <summary>Delegate for the state update event</summary>
    public delegate void StateUpdateDelegate(StateUpdateEvent evt);
    /// <summary>Every state change listeners</summary>
    public StateUpdateDelegate onStateUpdateListeners;
    /// <summary>Send every state change message to the delegate </summary>
    public bool sendEveryStateMessage = false;

#endif
    /**
     * FSM fields 
     */
    /// <summary>The actual state</summary>
    public STATES state;
    /// <summary>Previous state</summary>
    public STATES prevState;
    /// <summary>Print to the log every state change</summary>
    public bool logFsm = false;
    /// <summary>The on state exit will be executed</summary>
    protected System.Action onStateExit;
    /// <summary>
    /// The value transfered to the state. The method Go() can be used
    /// with or without additional argument (state value).
    /// </summary>
    protected object stateValue;
    /// <summary>Any string to markup the FSM by TAG</summary>
    protected string tagString;
    /// <summary>Any string to markup the point inside any state</summary>
    protected string substate;
    
    /**
     * FSM timer
     */
    /// <summary>The state starts at</summary>
    private float stateStartAt;
    /// <summary>Get time from this state start</summary>
    protected float stateTime { get { return Time.time - stateStartAt; } }
    
    /**
     * FSM State's event handler
     */
    /// <summary>Delegate for message receiver by state</summary>
    public delegate void StateEventDelegate(BaseEvent evt);
    /// <summary>The state onEvent block</summary>
    protected StateEventDelegate onStateEvent;

    /**
     * FSM API Methods
     */
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
        Debug.LogFormat("FSM.InterruptAndGo from state: {0}", state);
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
        prevState = state;
        state = nextState;
        substate = string.Empty;
        onStateEvent = null;
        if (logFsm)
            Debug.LogFormat("FSM.Go: {0}", state);
        stateStartAt = Time.time;
        AddFSM(this);
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
        prevState = state;
        state = nextState;
        substate = string.Empty;
        onStateEvent = null;
        if (logFsm)
            Debug.LogFormat("FSM.GoAndStop: {0}", state);
#if USE_STATE_UPDATE_DELEGATE        
        // In case if you need FSM may send message every state change
        if (sendEveryStateMessage && onStateUpdateListeners != null)
            onStateUpdateListeners(new StateUpdateEvent (this));
#endif
        RemoveFSM(this);
        return null;
    }
    /// <summary>Go to previous state</summary>
    /// <returns>The back.</returns>
    public object GoBack()
    {
        if (logFsm)
            Debug.LogFormat("FSM.GoBack: {0}", prevState);
        return Go(prevState);
    }
    /// <summary>Starts the FSM</summary>
    /// <param name="initialState">Initial State</param>
    protected void StartFsm(STATES initialState, object theValue = null) 
    {
        state = initialState;
        stateValue = theValue;
        onStateEvent = null;
        if (logFsm)
            Debug.LogFormat("FSM.StartFsm: {0}", state);
        AddFSM(this);
    }
    /// <summary>Start next state process</summary>
    protected override void StartNextState()
    {
        if (gameObject.activeSelf)
        {
            // Start coroutine of the state. This is most weak 
            // part of code because there is the string as the function's
            // name used
            StartCoroutine(state.ToString());
            // In case if you need FSM may send message every state change
#if USE_STATE_UPDATE_DELEGATE
         if (sendEveryStateMessage && onStateUpdateListeners != null)
             onStateUpdateListeners(new StateUpdateEvent(this));
#endif            
        }
        else
        {
            Debug.LogErrorFormat("{0} GoInternal to state {1} but object is not activeSelf", tagString, state);
        }
    }
    /// <summary>This method allow to send event to the current state</summary>
    /// <param name="evt">Event data</param>
    public virtual void OnStateEvent(BaseEvent evt)
    {
        if (onStateEvent != null) onStateEvent(evt);
    }
    /// <summary>Return the state in user friendly format</summary>
    /// <returns>String value of the state</returns>
    public override string ToString() {
        return string.Format("{0}.{1}\n  state: '{2}:{3}' [{4}]\n  old: '{5}'\n", 
            this.GetType().ToString(), tag, state, substate, stateTime.ToString("0.0"), prevState);
    }
}

/// <summary>
/// State machine with template class.
/// </summary>
/// <typeparam name="STATES">Enum type with states of this object</typeparam>
/// <typeparam name="TEMPLATE">Type of template of this object</typeparam>
public class TFsmEntity<STATES,TEMPLATE> : TFsmEntity<STATES> where TEMPLATE : BaseTemplate
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

/// <summary>
/// Base Finite State Machine
/// <summary>
public class BaseFSM : BaseBehaviour
{
    // Linked list node
    [HideInInspector]public BaseFSM prevFsm = null;
    [HideInInspector]public BaseFSM nextFsm = null;
    // List of FSMs requested Go to next state
    private static BaseFSM first;
    /// <summary>Add FSM to list</summary>    
    protected static void AddFSM(BaseFSM fsm)
    {
        RemoveFSM(fsm);
        if (first != null) {
            fsm.nextFsm = first;
            first.prevFsm = fsm;
        }
        first = fsm;
    }
    /// <summary>Remove FSM from list</summary>    
    protected static void RemoveFSM (BaseFSM fsm)
    {
        if (first == fsm) {
            first = fsm.nextFsm; // remove first
        } else {
            // not head of list
            if (fsm.nextFsm != null) {
                fsm.prevFsm.nextFsm = fsm.nextFsm;
                fsm.nextFsm.prevFsm = fsm.prevFsm;
            } else if (fsm.prevFsm != null) {
                fsm.prevFsm.nextFsm = null;
            }
        }
        fsm.prevFsm = null;
        fsm.nextFsm = null;
    }
    /// <summary>
    /// Start next state for all FSMs
    /// Call it once per frame at the LateUpdate method.
    /// </summary>
    public static void StartAllNextStates()
    {
        if (first == null) return;
        var fsm = first;
        while (fsm != null)
        {
            var next = fsm.nextFsm;
            RemoveFSM(fsm);
            fsm.StartNextState();
            fsm = next;
        }
    }
    /// <summary>Start next state process</summary>
    protected virtual void StartNextState()
    {
        Debug.LogError("Not overrided");
    }
}
