using UnityEngine;

public class Quadruped : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private LayerMask groundLayer;

    void LateUpdate()
    {
        AlignToTerrain();
    }

    void AlignToTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            // Get current forward direction (where character is facing)
            Vector3 forward = transform.forward;
            
            // Project the forward direction onto the plane defined by the ground normal
            Vector3 projectedForward = Vector3.ProjectOnPlane(forward, hit.normal).normalized;
            
            // Create rotation with ground normal as up and projected forward as forward
            Quaternion targetRotation = Quaternion.LookRotation(projectedForward, hit.normal);
            
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        }
    }
}
