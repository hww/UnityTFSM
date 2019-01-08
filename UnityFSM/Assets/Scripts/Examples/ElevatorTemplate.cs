using UnityEngine;
using System.Collections;

// Just example of template.
public class ElevatorTemplate : BaseTemplate {

    public float levelHeight = 1.5f;  // distance between levels
    public int minLevel = 0;          // 1st floor
    public int maxLevel = 2;          // 2d floor
    public float speed  = 1;          // meters per second

}
