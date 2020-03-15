using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // the distance of the view cone
    public float viewRadius;

    // angle of the view cone
    [Range(0, 360)]
    public float viewAngle;

    // time in seconds for delaying sensor state
    public float sensorDelay;

    // layer mask to detect
    public LayerMask targetMask;
    // layer mask to act as an obstacle
    public LayerMask obstacleMask;

    // list of targets that are within the view range
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    // list of targets that are within the view radius
    [HideInInspector]
    public List<Transform> audibleTargets = new List<Transform>();

    // the number of raycasts it will shoot to draw the line of sight
    // 0.2 is a good value, but higher value means more calculations
    public float meshResolution;

    // raycast edge detection refresh rate
    [HideInInspector]
    public int edgeResolveIterations = 4;

    // distance for objects to be considered separate
    [HideInInspector]
    public float edgeDstThreshold = 0.5f;

    // mesh filter for the line of sight. The mesh that will be rendered
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        // set the mesh and mesh filter
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        // start method FindTargetsWithDelay(sensorDelay);
        StartCoroutine("FindTargetsWithDelay", sensorDelay);
    }

    // using LateUpdate to reduce jittering vector updates
    private void LateUpdate()
    {
        // draws and updates the mesh that represents the object's field of view
        DrawFieldOfView();
    }

    /// <summary>
    /// Finds the target that is within the view sight. The delay is in seconds, and because this uses yield to make the delay,
    /// it must be used with the StartCoroutine
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator FindTargetsWithDelay(float delay)
    {
        //used to loop the method
        while (true)
        {
            //yield is used to stop the Coroutine for the given amount of seconds in float
            yield return new WaitForSeconds(delay);

            FindVisibleTargets(targetMask);
        }
    }

    /// <summary>
    /// populates the list visibleTargets with all the objects that are part of the targetLayer,
    /// and is within the viewing angle of this object. This uses raycast to determine if the object is
    /// not behind any obstacle layer
    /// </summary>
    /// <param name="targetLayer"></param>
    void FindVisibleTargets(LayerMask targetLayer)
    {
        // clear the list when the method starts
        visibleTargets.Clear();
        audibleTargets.Clear();

        // get the array of all the objects that are in the target mask and is within the view radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            // get the transform of the target in the current index
            Transform target = targetsInViewRadius[i].transform;

            audibleTargets.Add(target);

            // get the direction from this object to the target object
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // check if the direction of the target is with in the view angle. aka; is it visible to this object?
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                // get the distance from this object to target object
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // check if there is no obstacle layer mask, and than add the target's transform data
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    audibleTargets.Remove(target);
                }
            }
        }
    }

    /// <summary>
    /// Draws the field of view of this object to the view mesh. This method works by shooting multiple raycasts
    /// that stops at either the radius, or the obstacle layer object, and make a triangle from two neighboring rays
    /// to draw
    /// </summary>
    void DrawFieldOfView()
    {
        // calculate the triangle count
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        //determine the triangle size
        float stepAngleSize = viewAngle / stepCount;

        // list of view points the raycast will hit
        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            //the angle for the triangles
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        // the number of points plus the origin
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        // set the vertex origin
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                // set the points for the triangle
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;

            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
