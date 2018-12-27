using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float[] xPositions;
    private float[] yPositions;
    private float[] velocities;
    private float[] accelerations;
    private LineRenderer body;
    private GameObject[] meshObjects;
    private Mesh[] meshes;
    private GameObject[] colliders;
    private const float SPRING_CONSTANT = 0.02f;
    private const float DAMPING = 0.04f;
    private const float SPREAD = 0.05f;
    private const float Z = -1.0f;
    private float baseHeight;
    private float left;
    private float bottom;
    [SerializeField] private GameObject splash;
    [SerializeField] private Material mat;
    [SerializeField] private GameObject waterMesh;

    public void SpawnWater(float left, float width, float top, float bottom)
    {
        int edgeCount = Mathf.RoundToInt(width) * 5;
        int nodeCount = edgeCount + 1;
        body = gameObject.AddComponent<LineRenderer>();
        body.material = mat;
        body.material.renderQueue = 1000;
        body.positionCount = nodeCount;
        body.startWidth = 0.1f;
        body.endWidth = 0.1f;
        xPositions = new float[nodeCount];
        yPositions = new float[nodeCount];
        velocities = new float[nodeCount];
        accelerations = new float[nodeCount];

        meshObjects = new GameObject[edgeCount];
        meshes = new Mesh[edgeCount];
        colliders = new GameObject[edgeCount];

        baseHeight = top;
        this.bottom = bottom;
        this.left = left;

        for (int i = 0; i < nodeCount; i++)
        {
            xPositions[i] = left + width * i / edgeCount;
            yPositions[i] = top;
            velocities[i] = 0;
            accelerations[i] = 0;
            body.SetPosition(i,new Vector3(xPositions[i],yPositions[i],Z));
        }

        for (int i = 0; i < edgeCount; i++)
        {
            meshes[i]=new Mesh();
            Vector3[] vertices = new Vector3[4];
            vertices[0]=new Vector3(xPositions[i],yPositions[i],Z);
            vertices[1]=new Vector3(xPositions[i+1],yPositions[i+1],Z);
            vertices[2]=new Vector3(xPositions[i],bottom,Z);
            vertices[3]=new Vector3(xPositions[i+1],bottom,Z);
            Vector2[] UVs = new Vector2[4];
            UVs[0]=new Vector2(0,1);
            UVs[1]=new Vector2(1,1);
            UVs[2]=new Vector2(0,0);
            UVs[3]=new Vector2(1,0);
            int[] tris = new int[6] {0, 1, 3, 3, 2, 0};
            meshes[i].vertices = vertices;
            meshes[i].uv = UVs;
            meshes[i].triangles = tris;
            meshObjects[i]=Instantiate(waterMesh,Vector3.zero,Quaternion.identity) as GameObject;
            meshObjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshObjects[i].transform.parent = transform;
            colliders[i]=new GameObject();
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;
            colliders[i].transform.position=new Vector3(left+width*(i+0.5f)/edgeCount,top-0.5f,0.0f);
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<WaterDetector>();
        }
    }

    private void UpdateMeshes()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            Vector3[] vertices = new Vector3[4];
            vertices[0]=new Vector3(xPositions[i],yPositions[i],Z);
            vertices[1]=new Vector3(xPositions[i+1],yPositions[i+1],Z);
            vertices[2]=new Vector3(xPositions[i],bottom,Z);
            vertices[3]=new Vector3(xPositions[i+1],bottom,Z);
            meshes[i].vertices = vertices;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < xPositions.Length; i++)
        {
            float force = SPRING_CONSTANT * (yPositions[i] - baseHeight) + velocities[i] * DAMPING;
            //accelerations[i]=-force/mass;
            accelerations[i] = -force;
            yPositions[i] += velocities[i];
            velocities[i] += accelerations[i];
            body.SetPosition(i,new Vector3(xPositions[i],yPositions[i],Z));
        }

        float[] leftDeltas = new float[xPositions.Length];
        float[] rightDeltas=new float[xPositions.Length];

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xPositions.Length; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = SPREAD * (yPositions[i] - yPositions[i - 1]);
                    velocities[i - 1] += leftDeltas[i];
                }

                if (i < xPositions.Length - 1)
                {
                    rightDeltas[i] = SPREAD * (yPositions[i] - yPositions[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }

            for (int i = 0; i < xPositions.Length; i++)
            {
                if (i > 0)
                {
                    yPositions[i - 1] += leftDeltas[i];
                }

                if (i < xPositions.Length - 1)
                {
                    yPositions[i + 1] += rightDeltas[i];
                }
            }
        }
        
        UpdateMeshes();
    }

    private void Start()
    {
        SpawnWater(-10,20,0,-10);
    }

    public void Splash(float xpos, float velocity)
    {
        if (xpos >= xPositions[0] && xpos <= xPositions[xPositions.Length - 1])
        {
            xpos -= xPositions[0];
            int index = Mathf.RoundToInt((xPositions.Length - 1) *
                                         (xpos / (xPositions[xPositions.Length - 1] - xPositions[0])));
            velocities[index] = velocity;
            float lifetime = 0.93f + Math.Abs(velocity) * 0.07f;
            var myMain = splash.GetComponent<ParticleSystem>().main;
            myMain.startSpeed = 8 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
            myMain.startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
            myMain.startLifetime = lifetime;
            Vector3 position = new Vector3(xPositions[index],yPositions[index]-0.35f,5);
            Quaternion rotation = Quaternion.LookRotation(new Vector3(xPositions[Mathf.FloorToInt(xPositions.Length/2)],baseHeight+8,5)-position);
            GameObject splish = Instantiate(splash,position,rotation) as GameObject;
            Destroy(splish,lifetime+0.3f);
        }
    }
}
