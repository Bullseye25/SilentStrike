using UnityEngine;
public class SpawnPoint : MonoBehaviour {
    public WaypointGroup waypointGroup;
    public bool isOccupied;
    public bool isSideHide;
    public float attackRange;
    public float shootRange;
    public RuntimeAnimatorController SideHideAnimatorController;

    // Constructor
    public SpawnPoint(WaypointGroup waypointGroup, bool isOccupied = false)
    {
        this.waypointGroup = waypointGroup;
        this.isOccupied = isOccupied;
    }
}