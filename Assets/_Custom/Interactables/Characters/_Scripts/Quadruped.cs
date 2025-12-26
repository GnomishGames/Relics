using UnityEngine;

public class Quadruped : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        AlignToTerrain();
    }

    void AlignToTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            // Calculate the target rotation based on the ground normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        }
    }
}
