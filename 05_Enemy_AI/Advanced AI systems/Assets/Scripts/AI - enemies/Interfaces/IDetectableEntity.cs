using UnityEngine;
using UnityEngine.Splines;

public interface IDetectableEntity
{
    public string Name { get; set; }
    public Vector3 GetPosition();
    public Vector3 GetObservationPoint();
    
}
