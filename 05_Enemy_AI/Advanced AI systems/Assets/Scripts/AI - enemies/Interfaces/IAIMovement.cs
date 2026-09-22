using UnityEngine;

public interface IAIMovement
{
    void Update(float speed);
    void SetDestination(Vector3 destination);
    void Stop();
    bool HasReachedDestination();
    Vector3 CheckDestination(Vector3 destination,float maxDistance);
    Vector3 GetDestination();
    bool HasPath();
    float GetRemainingDistance();
}
