using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;
using static UnityEngine.GraphicsBuffer;

public class JetController : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;
    [SerializeField] float maxChange;

    Vector3 velocity;
    Vector3 acceleration;

    Vector3 midPoint;
    Vector3 startPoint;
    Vector3 current;

    Person person = new Person();
    Jet jet = new Jet();
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState == TripState.STATIC)
        {
            GameManager.Instance.acceptingNewTargets = true;
            
            return;
        }
            
        TravelSwitch();

    }

    void TravelSwitch()
    {
        switch (GameManager.Instance.currentState)
        {
            case TripState.PACKING:
                if (person.PrepareForTravel())
                {
                    GameManager.Instance.currentState = TripState.BOARDING;
                }
                else
                {
                    GameManager.Instance.currentState = TripState.CANCELLED;
                    Debug.Log("You have too much luggage!");
                }
                
                break;
            case TripState.BOARDING:
                if (jet.PrepareForTravel())
                {
                    Debug.Log("Preparations complete, beginning travel.");
                    GameManager.Instance.currentState = TripState.TRAVELING;
                    midPoint = CalculateAerialMidPoint(transform.position, GameManager.Instance.target.position);
                    current = midPoint;
                    rb.useGravity = false;
                }
                else
                {
                    Debug.Log("You had too much luggage!");
                    GameManager.Instance.currentState = TripState.CANCELLED;
                }
                break;
            case TripState.TRAVELING:
                if (jet.Travel())
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    ApplySteering();
                    Debug.Log("Travelling!");
                }
                else
                {
                    Debug.Log("Engine Failure");
                    GameManager.Instance.currentState = TripState.ENGINE_FAILURE;
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
                break;
            case TripState.CANCELLED:
                GameManager.Instance.currentState = TripState.STATIC;
                Debug.Log("The trip was cancelled!");
                break;
            case TripState.ARRIVED:
                if (jet.Arrive())
                {
                    GameManager.Instance.currentState = TripState.STATIC;
                    Debug.Log("Arrived!");
                }
                else
                {
                    GameManager.Instance.currentState = TripState.STATIC;
                    Debug.Log("You crashed while trying to land!");
                }
                break;
            case TripState.ENGINE_FAILURE:
                if (jet.EngineRecovery())
                {
                    GameManager.Instance.currentState = TripState.TRAVELING;
                    Debug.Log("Engine recovery succesful!");
                }
                
                break;
            default:
                break;
        }
    }

    Vector3 CalculateSteering(Vector3 currentTarget)
    {
        Vector3 desired = currentTarget - transform.position;
        desired = desired.normalized;
        desired *= maxSpeed;
        Vector3 steer = desired - velocity;
        steer = steer.normalized;
        steer *= maxForce;
        return steer;
    }

    void ApplySteering()
    {
        Vector3 targetDiretion = transform.position + velocity;
        // or + velocity
        Quaternion rotation = Quaternion.LookRotation(velocity);

        float distanceToFinal = Vector3.Distance(transform.position, GameManager.Instance.target.position);
        float distanceToMid = Vector3.Distance(transform.position, midPoint);
        float distanceToStart = Vector3.Distance(transform.position, startPoint);

        if (distanceToFinal < 2)
        {
            GameManager.Instance.currentState = TripState.ARRIVED;
            rb.velocity = Vector3.zero;
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
            return;
        }
            
        if (distanceToMid < 2)
            current = GameManager.Instance.target.position;
        if (distanceToStart < 2)
            current = midPoint;

        acceleration += CalculateSteering(current);
        velocity += acceleration;
       
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * maxChange);
        transform.position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
    }

    Vector3 CalculateAerialMidPoint(Vector3 from, Vector3 to)
    {
        float calcX = (from.x + to.x) / 2;
        float calcZ = (from.z + to.z) / 2;

        return new Vector3(calcX, 40f, calcZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform == GameManager.Instance.target)
        {
            GameManager.Instance.currentState = TripState.ARRIVED;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("You crashed!");
            rb.isKinematic = true;
            rb.useGravity = false;
            GameManager.Instance.currentState = TripState.CANCELLED;
        }
    }
}
