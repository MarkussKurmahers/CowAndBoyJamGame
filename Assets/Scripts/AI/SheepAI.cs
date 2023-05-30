using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class SheepAI : MonoBehaviour
{

    public bool Fenced;
    public bool Midair, Attacked,ropped;
    NavMeshAgent agent;
    float timer, timer2, RandomTimeStep;
    Rigidbody rb;
    CapsuleCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>(); 
        Fenced = true;
        agent = GetComponent<NavMeshAgent>();
        RandomTimeStep = Random.Range(1.0f, 8.0f);
        rb = GetComponent<Rigidbody>();
    }
    public void SetAttackFalse()
    {
        Attacked = false;
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Attacked)
        {
            timer += Time.deltaTime;
            if (timer > RandomTimeStep && agent.enabled)
            {
                agent.destination = SetRandomPos();
                RandomTimeStep = Random.Range(1.0f, 8.0f);
                timer = 0.0f;
            }
            if (Fenced)
            {
                ShouldILeave();
            }
            if (Midair)
            {
                collider.enabled = false;
                rb.freezeRotation = true;
                agent.enabled = false;
            }
           
        }
        if (ropped)
        {
            Attacked = false;
            GetComponent<BoxCollider>().enabled = false;
            agent.enabled = false;
        }


    }

    public void UnRope()
    {
        ropped = false;
        Attacked = false;
        GetComponent<BoxCollider>().enabled = true;
        agent.enabled = true;
    }

    Vector3 SetRandomPos()
    {
        float x = transform.position.x;
        float z = transform.position.z;

        x += Random.Range(-5.0f, 5.0f);
        z += Random.Range(-5.0f, 5.0f);

        return new Vector3(x,transform.position.y, z);

    }

    void ShouldILeave()
    {
        int temp = Game.instance.GetWave() / 2 - Game.instance.SheepList.ToArray().Length;
        if (temp > 0)
        {
            timer2 += Time.deltaTime;
            if (timer2 > 5.0f)
            {
                float Randomizer = Random.Range(0, 4);
                if (Randomizer == 1)
                {
                    agent.enabled = false;
                    LeaveFence();
                    Midair = true;
                    Fenced = false;
                    Game.instance.AddSheepToArray(this.gameObject.transform);
                }
                timer2 = 0.0f;
            }
        }
      
    }

  

    void LeaveFence()
    {
        RaycastHit hit;
        collider.enabled = false;
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                rb.AddForce(7.0f, 7.0f, 0.0f, ForceMode.Impulse);
                break;
            case 1:
                rb.AddForce(0.0f, 7.0f, 7.0f, ForceMode.Impulse);
                break;
            case 2:
                rb.AddForce(-7.0f, 7.0f, 0.0f, ForceMode.Impulse);
                break;
            case 3:
                rb.AddForce(0.0f, 7.0f, -7.0f, ForceMode.Impulse);
                break;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor" && Midair)
        {
            rb.freezeRotation = false;
            collider.enabled = true;
            agent.enabled = true;
            Midair = false;
        }
    }

    public void Taken(Transform other)
    {
        Attacked = true;
        agent.enabled = false;
        transform.parent = other.transform;

    }

}
