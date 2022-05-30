using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Unity.Jobs;
using System;
using Unity.Collections;
using Unity.Burst;

public class NavmeshBaker : MonoBehaviour
{
    private float Timer;
    [SerializeField] public NavMeshSurface surfaces;

    public void Start()
    {
        surfaces = this.GetComponent<NavMeshSurface>();
    }

    public void FixedUpdate()
    {
        
        if (Timer<Time.deltaTime)
        {
            surfaces.BuildNavMesh();

            Timer = 5;
            
        }
        else
        {
            Timer -= Time.deltaTime;
        }
        
       
    }

    private JobHandle ToughJobStuff()
    {
        NavmeshBuilderBaby NavMeshJob = new NavmeshBuilderBaby();
        
        return NavMeshJob.Schedule();
    }

}


public struct NavmeshBuilderBaby : IJob
{
    NavMeshSurface navMeshSurface;
    private float Timer;
    public void Execute()
    {
        
       
    }
    
}

