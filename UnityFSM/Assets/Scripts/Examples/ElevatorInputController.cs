using UnityEngine;
using System.Collections;

public class ElevatorInputController : MonoBehaviour {

    public Elevator elevator;
    public ElevatorTemplate template;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Up
            SendOnStateEvent(new ElevatorInputEvent(this, ElevatorInputEvent.EName.OnPushUpButton));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Down
            SendOnStateEvent(new ElevatorInputEvent(this, ElevatorInputEvent.EName.OnPushDownButton));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnElevator();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Despawn
            template.Despawn(elevator.gameObject);
        }

    }

    void SpawnElevator()
    {
        if (elevator != null) return;
        GameObject go = template.Spawn(new OnSpawnElevatorEvent(template, 0));
        elevator = go.GetComponent<Elevator>();
    }

    void SendOnStateEvent(ElevatorInputEvent evt)
    {
        if (elevator == null) return;
        elevator.OnStateEvent(evt);
    }

    void Start()
    {
        SpawnElevator();
    }
}

