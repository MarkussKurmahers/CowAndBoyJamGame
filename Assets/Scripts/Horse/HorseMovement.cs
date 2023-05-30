using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=8ZxVBCvJDWk

    [SerializeField] Rigidbody rb;
    [SerializeField] float Speed;
    [SerializeField] float MaxSpeed,MinSpeed;
    [SerializeField] float Acceleration;
    [SerializeField] float BreakingDrag;
    [SerializeField] Animator animator,CamAnimator;

    [SerializeField] float[] SprintSpeedLevels;
    [SerializeField] float SprintLevel;
    bool isSprinting;
    [SerializeField] float StaminaDepletion;
    [SerializeField] Slider StaminaSlider;
    [SerializeField] Slider[] StaminaSliderAlt;

    [SerializeField] AudioSource MooSound;
    [SerializeField] GameObject CowTiredSound,CowCrashSound;
 
    Vector3 _Input;
    public float SpeedMultiplier = 1;

    bool FirstBar=true,SecondBar=true, ThirdBar=true;
    void GatherInput()
    {
        _Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }
    private void Start()
    {
        Speed = MinSpeed;
    }

    public void SprintModeIncrease()
    {
        SprintLevel++;
        switch(SprintLevel)
        {
            case 1:
                SpeedMultiplier = SprintSpeedLevels[0];
                animator.SetFloat("Speed", 2);
                MooSound.pitch = Random.Range(.8f, .9f);
                CamAnimator.Play("CamShake1");
                break;
            case 2:
                SpeedMultiplier = SprintSpeedLevels[1];
                animator.SetFloat("Speed", 3);
                MooSound.pitch = Random.Range(1, 1.1f);
                CamAnimator.Play("CamShake2");
                break;
            case 3:
                SpeedMultiplier = SprintSpeedLevels[2];
                animator.SetFloat("Speed", 4);
                MooSound.pitch = Random.Range(1.2f, 1.3f);
                CamAnimator.Play("CamShake3");
                break;
            default:
                SpeedMultiplier = SprintSpeedLevels[2];
                animator.SetFloat("Speed", 4);
                MooSound.pitch = Random.Range(1.3f, 1.4f);
                CamAnimator.Play("CamShake4");
                break;
        }
        MooSound.Play();
    }
    public void SprintModeDecrease()
    {
        if(SprintLevel > 1)
        {
            SprintLevel--;
        }

        switch (SprintLevel)
        {
            case 1:
                SpeedMultiplier = SprintSpeedLevels[0];
                animator.SetFloat("Speed", 2);
                CamAnimator.Play("CamShake1");
                break;
            case 2:
                SpeedMultiplier = SprintSpeedLevels[1];
                animator.SetFloat("Speed", 3);
                CamAnimator.Play("CamShake2");
                break;
            case 3:
                SpeedMultiplier = SprintSpeedLevels[2];
                animator.SetFloat("Speed", 4);
                CamAnimator.Play("CamShake3");
                break;
            default:
                SpeedMultiplier = SprintSpeedLevels[2];
                animator.SetFloat("Speed", 4);
                CamAnimator.Play("CamShake4");
                break;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(isSprinting )
        {
            if(rb.velocity.magnitude >= 3)
            {
                Instantiate(CowCrashSound, transform.position, Quaternion.identity);
            }
           
            if( collision.gameObject.GetComponent<CowEnemy>() != null)
            {
                SprintModeIncrease();
                collision.gameObject.GetComponent<CowEnemy>().TakeDamage(transform.position, collision.contacts[0].point);
                StaminaSlider.value += .333f;

                if (StaminaSlider.value > 1)
                {
                    StaminaSlider.value = 1;
                    CheckBarState();
                }
            }
        
        }

        
    }


    void CheckBarState()
    {
        if (StaminaSlider.value > .33333f && !FirstBar || (StaminaSlider.value > .333f && !SecondBar) || StaminaSlider.value > .666f && !ThirdBar)
        {
            if (!FirstBar)
            {
                FirstBar = true;
             
                return;
            }
            if (!ThirdBar)
            {
                ThirdBar = false;
             
                return;
            }
            if (!SecondBar)
            {
                SecondBar = false;
                
                return;
            }

        }
    }

    void UpdateAlt()
    {
        StaminaSliderAlt[0].value = StaminaSlider.value;
        StaminaSliderAlt[1].value = StaminaSlider.value;
    }

    void StopSprint()
    {
        CamAnimator.Play("CamStill");
        animator.SetFloat("Speed", 1);
        SpeedMultiplier = 1;
    }

    void SprintInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && StaminaSlider.value > 0 && !isSprinting && FirstBar)
        {
            isSprinting = true;
            SprintLevel = 0;
            SprintModeIncrease();

        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            StopSprint();
        }

        if(isSprinting && StaminaSlider.value > 0)
        {
            StaminaSlider.value -= StaminaDepletion * Time.deltaTime;
           
            if (StaminaSlider.value < .33333f && FirstBar || (StaminaSlider.value < .333f && SecondBar) || StaminaSlider.value < .666f && ThirdBar )
            {
                if (FirstBar)
                {
                    FirstBar = false;
                    SprintModeDecrease();
                    return;
                }
                if (ThirdBar)
                {
                    ThirdBar = false;
                    SprintModeDecrease();
                    return;
                }
                if(SecondBar)
                {
                    SecondBar = false;
                    SprintModeDecrease();
                    return;
                }

            }
            if(StaminaSlider.value <= 0)
            {
                Instantiate(CowTiredSound, transform.position, Quaternion.identity);
                StopSprint();
            }
        }
        if(!isSprinting && StaminaSlider.value < 1)
        {
            StaminaSlider.value += StaminaDepletion * Time.deltaTime;
           
            if (StaminaSlider.value > .33333f && !FirstBar || (StaminaSlider.value > .6666f && !SecondBar) || StaminaSlider.value >= 1 && !ThirdBar)
            {
                if(!FirstBar)
                {
                    FirstBar = true;
                    return;
                }
                if(!SecondBar)
                {
                    SecondBar = true;
                    return;
                }
                if(!ThirdBar)
                {
                    ThirdBar = true;
                    return;
                }
            }
        }

        UpdateAlt();


    }


    void Move()
    {

        Vector3 NormalInput = _Input;

        NormalInput = Vector3.ClampMagnitude(NormalInput, 1);

        //Vector3 direction = transform.position + (transform.right * NormalInput.magnitude) * (Speed * SpeedMultiplier) * Time.deltaTime;

        //  rb.AddForce(direction * 5,ForceMode.Acceleration);
        if(_Input != Vector3.zero)
        {
            rb.AddForce(transform.right * Speed * SpeedMultiplier, ForceMode.Force);
            switch(SpeedMultiplier)
            {
                case 1:
                    rb.drag = 0.2f;
                    break;
                case 2:
                    rb.drag = 0.1f;
                    break;
                case 3:
                    rb.drag = 0f;
                    break;
            }
         
            Speed += Acceleration * SpeedMultiplier;
            animator.Play("Run"); 

        }
        else
        {
            rb.drag = BreakingDrag;
            Speed -= Acceleration;
            animator.Play("Idle");
        }
        Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);
    }


    private void FixedUpdate()
    {
        Move();
    }

    void Look()
    {
    

        switch (_Input.x)
        {
            case 0:
                switch (_Input.z)
                {
                    case 1:
                        transform.eulerAngles = new Vector3(0, 315, 0);
                        break;
                    case -1:
                        transform.eulerAngles = new Vector3(0, 135, 0);
                        break;

                }
                break;
            case 1:
                switch (_Input.z)
                {
                    case 0:
                        transform.eulerAngles = new Vector3(0, 45, 0);
                        break;
                    case 1:
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case -1:
                        transform.eulerAngles = new Vector3(0, 90, 0);
                        break;
                }
                break;
            case -1:
                switch (_Input.z)
                {
                    case 0:
                        transform.eulerAngles = new Vector3(0, 225, 0);
                        break;
                    case 1:
                        transform.eulerAngles = new Vector3(0, 270, 0);
                        break;
                    case -1:
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        break;
                }
                break;
        }




    }



    // Update is called once per frame
    void Update()
    {
        SprintInput();
        GatherInput();
        
        Look();


    }
}
