using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMovementController : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float maxDistanceRebuildPath = 1;
    [SerializeField] private float acceleration = 1;
    [Tooltip("The minimum distance an agent must get from an object before it is considered to have reached it.")]
    public float minReachDistance = 2f;
    [SerializeField] private float pathPointRadius = 0.2f;
    [SerializeField] private Octree octree;
    [SerializeField] private LayerMask playerSeeLayerMask = -1;
    private Octree.PathRequest currentPath;
    private Octree.PathRequest nextPath;
    private Rigidbody rigidbody;
    private Vector3 currentDestination;
    private Vector3 lastDestination;
    private Collider collider;
    
    [Header("Height")]
    [Tooltip("preferred height to fly at.")]
    public float preferredFlightHeight = 1.5f;
    [Tooltip("Minimum height to fly at (does not impact landing).")]
    public float minFlightHeight = 1f;
    [Tooltip("Maximum height to fly at.")]
    public float maxFlightHeight = 7;

    public Transform Target
    {
        get { return _target; }
        set {
            if (!GameObject.ReferenceEquals(_target, value))
            {
                _target = value;
                UpdatePath(true);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        octree = FindObjectOfType<Octree>();
        if (octree == null)
        {
            Debug.LogError("There is no `octree` component in your seen. Please add one so that Flying-Pathfinding can work.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (!octree.IsBuilding
            && (nextPath == null || !nextPath.isCalculating) 
            && Vector3.SqrMagnitude(_target.position - lastDestination) > (maxDistanceRebuildPath * maxDistanceRebuildPath) 
            && Vector3.SqrMagnitude(_target.position - transform.position) > (minReachDistance * minReachDistance))
        {
            UpdatePath();
        }

        // if the agent appears not to be moving then perhaps it is stuck. Try updating the path

        /*if (newPath != null && !newPath.isCalculating)
		{
			if (newPath.Path.Count > 0)
			{
				float distanceSoFar = 0;
				int lastPoint = newPath.Path.Count - 1;
				for (int i = lastPoint; i >= 1; i--)
				{
					distanceSoFar += Vector3.Distance(newPath.Path[i], newPath.Path[i - 1]);

					if (distanceSoFar <= minFollowDistance)
					{
						lastPoint = i;
					}
					else
					{
						break;
					}
				}
				if (lastPoint > 0)
				{
					newPath.Path.RemoveRange(lastPoint, newPath.Path.Count - lastPoint);
				}
			}
		}*/

        var curPath = Path;

        if (curPath != null && !curPath.isCalculating && curPath.Path.Count > 0)
        {
            if (Vector3.SqrMagnitude(transform.position - _target.position) < (minReachDistance * minReachDistance))
            {
                curPath.Reset();
                return;
            }

            currentDestination = curPath.Path[0] + Vector3.ClampMagnitude(rigidbody.position - curPath.Path[0], pathPointRadius);

            rigidbody.velocity += Vector3.ClampMagnitude(currentDestination - transform.position, 1) * Time.deltaTime * acceleration;
            float sqrMinReachDistance = minReachDistance * minReachDistance;

            Vector3 predictedPosition = rigidbody.position + rigidbody.velocity * Time.deltaTime;
            float shortestPathDistance = Vector3.SqrMagnitude(predictedPosition - currentDestination);
            int shortestPathPoint = 0;

            for (int i = 0; i < curPath.Path.Count; i++)
            {
                float sqrDistance = Vector3.SqrMagnitude(rigidbody.position - curPath.Path[i]);
                if (sqrDistance <= sqrMinReachDistance)
                {
                    if (i < curPath.Path.Count)
                    {
                        curPath.Path.RemoveRange(0, i + 1);
                    }
                    shortestPathPoint = 0;
                    break;
                }

                float sqrPredictedDistance = Vector3.SqrMagnitude(predictedPosition - curPath.Path[i]);
                if (sqrPredictedDistance < shortestPathDistance)
                {
                    shortestPathDistance = sqrPredictedDistance;
                    shortestPathPoint = i;
                }
            }

            if (shortestPathPoint > 0)
            {
                curPath.Path.RemoveRange(0, shortestPathPoint);
            }
        }
        else // no path
        {
            rigidbody.velocity -= rigidbody.velocity * Time.deltaTime * acceleration;
        }
    }

    /// <summary>
    /// Update the path if the current target location is a different destination to the lastDestination.
    /// <paramref name="force">Force a rebuild even if not outside the normal MaxDistanceREbuildPath</paramref>
    /// </summary>
    private void UpdatePath(bool force = false)
    {
        if (force || Vector3.SqrMagnitude(transform.position - lastDestination) > (maxDistanceRebuildPath * maxDistanceRebuildPath))
        {
            lastDestination = _target.transform.position;

            currentPath = nextPath;
            nextPath = octree.GetPath(transform.position, lastDestination, this);
        }
    }

    /// <summary>
    /// Check to see if a node is navigable, that is whether something can navigate through the node.
    /// To be navigable the node must be empty and it must be within the navigation space.
    /// </summary>
    /// <param name="position">The position of the node.</param>
    /// <returns>True if the node is navigable, otherwise false.</returns>
    internal bool IsNodeNavigable(Vector3 position)
    {
        Octree.OctreeElement node = octree.GetNode(position);
        if (octree.IsBuilding || node == null)
        {
            return false;
        } else
        {
            return node.Empty;
        }
    }

    private Octree.PathRequest Path
    {
        get
        {
            if ((nextPath == null || nextPath.isCalculating) && currentPath != null)
            {
                return currentPath;
            }
            return nextPath;
        }
    }

    public bool HasPathToTarget
    {
        get
        {
            return Path != null && Path.Path.Count > 0;
        }
    }

    public Vector3 CurrentTargetPosition
    {
        get
        {
            if (Path != null && Path.Path.Count > 0)
            {
                return currentDestination;
            }
            else
            {
                return _target.position;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (rigidbody != null)
        {
            Gizmos.color = Color.blue;
            Vector3 predictedPosition = rigidbody.position + rigidbody.velocity * Time.deltaTime;
            if (collider.GetType() == typeof(SphereCollider))
            {
                Gizmos.DrawWireSphere(predictedPosition, ((SphereCollider)collider).radius);
            } else if (collider.GetType() == typeof(CapsuleCollider))
            {
                Gizmos.DrawWireSphere(predictedPosition, ((CapsuleCollider)collider).radius);
            } else
            {
                Gizmos.DrawWireCube(predictedPosition, collider.bounds.size);
            }
        }

        if (Path != null)
        {
            var path = Path;
            for (int i = 0; i < path.Path.Count - 1; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(path.Path[i], minReachDistance);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(path.Path[i], Vector3.ClampMagnitude(rigidbody.position - path.Path[i], pathPointRadius));
                Gizmos.DrawWireSphere(path.Path[i], pathPointRadius);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(path.path[i], path.Path[i + 1]);
            }
        }

        octree.GetNode(transform.position).DrawGizmos(false, true);
        if (Target != null)
        {
            Octree.OctreeElement node = octree.GetNode(Target.position);
            if (node != null)
            {
                node.DrawGizmos(false, true);
            }
        }
    }
}
