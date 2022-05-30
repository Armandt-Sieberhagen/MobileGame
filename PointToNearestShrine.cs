using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToNearestShrine : MonoBehaviour
{
    public List<Transform> LocationsOfShrines;
    public Transform ClosestShrine;
    public float SmallestDistance;
    public float LookRadius;
    public LayerMask mask;

    public void Start()
    {
        SmallestDistance = 100000;
    }

    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, LookRadius, mask);
        LocationsOfShrines.Clear();
        SmallestDistance = 10000;
        foreach (var hitCollider in hitColliders)
        {
           
            if (hitCollider.GetComponent<LandScapeGen>())
            {
                if (hitCollider.GetComponent<LandScapeGen>().VillageActive)
                {
                    VillageGenerator Gen = hitCollider.GetComponent<LandScapeGen>().VillageGenerator;
                    
                    if (Gen.active)
                    {
                       
                        if (Vector3.Distance(transform.position, hitCollider.transform.position) < SmallestDistance)
                        {
                            
                            SmallestDistance = Vector3.Distance(ClosestShrine.position, hitCollider.transform.position);
                            ClosestShrine = hitCollider.transform;
                        }
                    }
                   
                }
            }
           
        }

    //    transform.right = (ClosestShrine.position - transform.position).normalized;
        transform.forward = transform.right;
        transform.LookAt(ClosestShrine);
    }
}
