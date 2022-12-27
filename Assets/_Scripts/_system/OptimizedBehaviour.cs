using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every component/script created should inherit from this. When calls are needed
/// to gameObject and transform, use CachedGameObject and CachedTransform instead.
/// </summary>
public class OptimizedBehaviour : MonoBehaviour
{
    // --- Properties ------------------------------------------------------------------------ //

    // NOTE: These properties are getters only and will assign their respective variables
    //       when called, do not set the variables yourself unless there is a good reason.

    public GameObject CachedGameObject
    {
        get
        {
            // Always check if object reference is null or instance id is 0
            if (ReferenceEquals(cachedGameObject, null) || cachedGameObject.GetInstanceID() == 0)
            {
                try
                {
                    cachedGameObject = gameObject;
                }
                catch
                {
                    // If any Unity shenanigans happen, we'll catch it here and just return a null object
                    Debug.LogError(this.GetType().Name + " - Unable to get gameObject.");
                    cachedGameObject = null;
                }
            }

            // Will either be the gameobject this component is attached to or
            // null because of weird Unity shenanigans
            return cachedGameObject;
        }
    }

    public Transform CachedTransform
    {
        get
        {
            // Always check if object reference is null or instance id is 0
            if (ReferenceEquals(cachedTransform, null) || cachedTransform.GetInstanceID() == 0)
            {
                try
                {
                    cachedTransform = transform;
                }
                catch
                {
                    // If any Unity shenanigans happen, we'll catch it here and just return a null object
                    Debug.LogError(this.GetType().Name + " - Unable to get transform.");
                    cachedTransform = null;
                }
            }

            // Will either be the tranform of the gameobject this object is
            // attached to or null because of weird Unity shenanigans
            return cachedTransform;
        }
    }

    // --- Variables ------------------------------------------------------------------------- //

    private GameObject cachedGameObject;
    private Transform cachedTransform;

    // --- All of your functions here -------------------------------------------------------- //
}
