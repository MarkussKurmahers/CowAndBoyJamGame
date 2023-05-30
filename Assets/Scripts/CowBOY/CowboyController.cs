using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyController : MonoBehaviour
{
    LineRenderer lr;
    [SerializeField]LayerMask GrappableMask;
    [SerializeField] float LassoRadius;
    [SerializeField] Transform LassoPos;
    [SerializeField] Transform LassoTarget;
    Rigidbody LassoTargetBody;
    [SerializeField] SpringJoint joint;
    [SerializeField] bool LassoStuck;
    [SerializeField] float LassoSpeed;
    Rigidbody body;
    [SerializeField] GameObject LassoCircle;
    bool WindingLasso;
    [SerializeField] Vector3 offset;
    [SerializeField] Rigidbody lassobody;
    [SerializeField] float minLassoLength, maxLassoLength;
    [SerializeField] float LassoRotSpeed;
    [SerializeField] GameObject LassoHop;
    [SerializeField] float LassoHopSpeed;
    [SerializeField] Transform LassoHopOGPos;
    [SerializeField] GameObject LassoAnim,LassoHitSound;
   
    bool isSendingLasso;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        body = GetComponent<Rigidbody>();
    }
    //https://www.youtube.com/watch?v=Xgh4v1w5DxU

    void Look()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 LookAtPos = new Vector3(hit.point.x,transform.position.y,hit.point.z);

            Quaternion lookAt = Quaternion.LookRotation(LookAtPos - transform.position);
            Quaternion correction = Quaternion.Euler(offset);

            transform.rotation = lookAt * correction;

            //  transform.LookAt(LookAtPos);
         

        }


    }

    private void FixedUpdate()
    {
        if (LassoStuck)
        {
            float x = Input.GetAxis("Mouse X") * LassoRotSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * LassoRotSpeed * Time.fixedDeltaTime;

            LassoTargetBody.AddTorque(Vector3.down * x);
            LassoTargetBody.AddTorque(Vector3.right * y);

            // LassoTargetBody.AddForce(Vector3.down * x);
            // LassoTargetBody.AddForce(Vector3.right * y);
            LassoTargetBody.AddForce(Vector3.up * 15);

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitpos = new Vector3(hit.point.x, LassoCircle.transform.position.y, hit.point.z);
                //  LassoCircle.transform.position = Vector3.MoveTowards(LassoCircle.transform.position, hitpos, LassoSpeed * Time.deltaTime);

                LassoTargetBody.AddForce((lassobody.position- LassoTargetBody.position).normalized * LassoRotSpeed, ForceMode.Force);
            }


        }

    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !WindingLasso)
        {
            LassoCircle.transform.position = new Vector3(transform.position.x, LassoCircle.transform.position.y, transform.position.z);
            LassoAnim.SetActive(true);
            if (LassoStuck)
            {
                Destroy(joint);

                if (LassoTarget.GetComponent<SheepAI>() != null)
                {
                    LassoTarget.GetComponent<SheepAI>().UnRope();
                }
                LassoStuck = false;
                LassoHop.transform.SetParent(null);
                lr.positionCount = 0;
            }
        }
        if(Input.GetMouseButton(0) && !LassoStuck && !isSendingLasso)
        {
          
            WindingLasso = true;
            LassoCircle.SetActive(true);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out  RaycastHit hit))
            {
                Vector3 hitpos = new Vector3(hit.point.x, LassoCircle.transform.position.y, hit.point.z);
                LassoCircle.transform.position = Vector3.MoveTowards(LassoCircle.transform.position, hitpos, LassoSpeed * Time.deltaTime);
            }
            
          
        }

        if(Input.GetMouseButtonUp(0) && !LassoStuck && WindingLasso && !isSendingLasso)
        {
            LassoCircle.SetActive(false);
            LassoAnim.SetActive(false);
            StartCoroutine(SendingLasso());
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
           

        }
    }

    IEnumerator SendingLasso()
    {
        isSendingLasso = true;
        lr.positionCount = 2;
        LassoHop.SetActive(true);
        LassoHop.transform.position = LassoHopOGPos.transform.position;
        while (LassoHop.transform.position != LassoCircle.transform.position)
        {
            lr.SetPosition(0, LassoPos.position);
            lr.SetPosition(1, LassoHop.transform.position);

            LassoHop.transform.position = Vector3.MoveTowards(LassoHop.transform.position, LassoCircle.transform.position, LassoHopSpeed * Time.deltaTime);
            yield return null;

          
        }
        

        WindingLasso = false;
        Collider[] hitColliders = Physics.OverlapSphere(LassoCircle.transform.position, LassoRadius, GrappableMask);
        isSendingLasso = false;
        if (hitColliders.Length > 0)
        {
            Instantiate(LassoHitSound, transform.position, Quaternion.identity);

            LassoStuck = true;
            //perhaps more catches if powerup in future
            
            GameObject Target = hitColliders[0].gameObject;
            LassoTargetBody = Target.GetComponent<Rigidbody>();

            if(Target.GetComponent<SheepAI>() != null)
            {
                Target.GetComponent<SheepAI>().ropped = true;
            }
           

            joint = Target.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedBody = lassobody;
            joint.connectedAnchor = new Vector3(0, 0, -1);

            float distanceFromGrapple = Vector3.Distance(transform.position, Target.transform.position);

            joint.maxDistance = maxLassoLength;
            joint.minDistance = minLassoLength;

            joint.spring = 200f;
            joint.damper = 10;
            joint.massScale = 5;

            LassoTarget = Target.transform;

            LassoHop.transform.parent = LassoTarget.transform;

            lr.positionCount = 2;
        }
        else
        {
            LassoHop.SetActive(false);
            lr.positionCount = 0;
        }    
    }


    private void LateUpdate()
    {
        DrawRope();
        Look();
    }

    void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0,LassoPos.position);
        lr.SetPosition(1, LassoHop.transform.position);
      
    }

}
