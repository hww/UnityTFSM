using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBehaviour : MonoBehaviour
{

    /// <summary>
    /// Get full path of given object
    /// </summary>
    /// <param name="tr">Transform of the give object</param>
    /// <returns>Full path</returns>
    public static string GetFullPath(Transform tr)
    {
        string path = "";
        while (tr != null)
        {
            path = "/" + tr.name + path;
            tr = tr.parent;
        }
        return path;
    }

    /// <summary>
    /// Get full path of this object
    /// </summary>
    /// <returns>Full path</returns>
    public string FullPath { get { return GetFullPath(transform); } }

    /// <summary>
    /// This method to allow print nicely formated text to the log.
    /// </summary>
    /// <param name="format">format string</param>
    /// <param name="paramList">arguments</param>
    /// <returns></returns>
    protected virtual string LogFormat(string format, params object[] paramList)
    {
        return string.Format("[{0}] ", name) + string.Format(format, paramList);
    }

    /// <summary>
    /// Called when we construct object
    /// </summary>
    /// <param name="evt">Message object</param>
    public virtual void OnSpawned(object evt)
    {
    }

    /// <summary>
    /// Called before unspawn object
    /// </summary>
    public virtual void OnDespawned()
    {
    }
}
