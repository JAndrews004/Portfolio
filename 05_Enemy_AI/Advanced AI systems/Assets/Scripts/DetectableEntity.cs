using UnityEngine;

public class DetectableEntity : MonoBehaviour, IDetectableEntity
{
    [SerializeField]
    private string entityName;

    public string Name
    {
        get => entityName;
        set => entityName = value;
    }
    public Transform CenterPoint;
    public Vector3 GetPosition()
    {
        return CenterPoint.position;
    }
    public Vector3 GetObservationPoint()
    {
        return CenterPoint.position;
    }
    
}
