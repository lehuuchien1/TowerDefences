using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    public static WaypointsManager Instance;
    public Transform[] waypoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform[] GetWaypoints()
    {
        return waypoints;
    }
}
