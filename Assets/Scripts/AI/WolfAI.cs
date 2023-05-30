using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    bool chasing, grabbed, wander;
    NavMeshAgent agent;
    public Transform Center;
    float timer, RandomTimeStep;
    Rigidbody rb;
    GameObject Sheep;
    [SerializeField] GameObject WolfObj;
    public void DestroyWolf()
    {
        if (Sheep)
        {
            Sheep.transform.SetParent(null);
            Sheep.GetComponent<SheepAI>().SetAttackFalse();
        }
        WolfObj.transform.SetParent(null);
        Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        RandomTimeStep = 1.0f;
        agent = GetComponent<NavMeshAgent>();
        Center = GameObject.Find("Center").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.instance.SheepList.Any() && !grabbed)
        {
            wander = true;
            chasing = false;
        }
        else if(Game.instance.SheepList.Any() && !grabbed)
        {
            wander = false;
            chasing = true;
        }

        if (chasing)
        {
            agent.destination = GetClosesSheep().position;
        }
        else if (wander)
        {
            timer += Time.deltaTime;
            if (timer > RandomTimeStep)
            {
                agent.destination = SetRandomPos();
                RandomTimeStep = Random.Range(1.0f, 6.0f);
                timer = 0.0f;
            }
        }
        else if (grabbed)
        {
            rb.freezeRotation = true;
            Vector3 moveDir = (transform.position - Center.transform.position).normalized;
            transform.position += (moveDir * 1.5f * Time.deltaTime);
        }



    }

    Transform GetClosesSheep()
    {
        Transform[] temp = Game.instance.SheepList.ToArray();
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;
        foreach(Transform i in temp)
        {
            float distance = Vector3.Distance(i.position, transform.position);
            if(distance < closestDistance)
            {
                closestTarget = i;
                closestDistance = distance;
            }
        }

        return closestTarget;

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sheep" && chasing)
        {
            other.GetComponent<SheepAI>().Taken(this.gameObject.transform);
            agent.enabled = false;
            chasing = false;
            wander = false;
            grabbed = true;
            Sheep = other.gameObject;
        }
    }

   
  

    Vector3 SetRandomPos()
    {
        float x = transform.position.x;
        float z = transform.position.z;

        x += Random.Range(-5.0f, 5.0f);
        z += Random.Range(-5.0f, 5.0f);

        return new Vector3(x, transform.position.y, z);

    }

   
}
