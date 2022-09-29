using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float rotspeed = 500f;

    float startX;
    float rotAmt;
    
    Vector3 orbitPoint = Vector3.zero;

    bool isRotating = false;
    bool isContinuous = false;
    bool cancelMouse = true;

    void Start()
    {
        startX = Screen.width / 2;
        rotAmt = 0;
    }

    void Update()
    {
        isRotating = false;
        //ConstrainY();

        if (isContinuous) ContinuousRotation();
        else MouseRotation();

        MouseLateralMovement();
        MouseWheelZoom();
        MouseOrbit();
        KeyboardVerticalMovement();
        KeyboardLateralMovement();
    }

    public void ChooseRotationStyle()
    {
        isContinuous = !isContinuous;
    }

    public void ChooseMouseStyle()
    {
        cancelMouse = !cancelMouse;
    }

    void KeyboardLateralMovement()
    {
        float adjustedSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            adjustedSpeed *= 4;
        }


        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * adjustedSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * adjustedSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += ConstrainedLocalVector(Vector3.forward) * adjustedSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += ConstrainedLocalVector(Vector3.back) * adjustedSpeed * Time.deltaTime;
        }
    }

    void MouseLateralMovement()
    {
        if (isRotating)
            return;
        if (cancelMouse)
            return;
        
        float adjustedSpeed = speed * 2;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            adjustedSpeed *= 4;
        }

        if (Input.mousePosition.x < 10)
        {
            transform.Translate(Vector3.left * adjustedSpeed * Time.deltaTime);
        }
        if (Input.mousePosition.x > Screen.width - 10)
        {
            transform.Translate(Vector3.right * adjustedSpeed * Time.deltaTime);
        }
        if (Input.mousePosition.y > Screen.height - 10)
        {
            transform.position += ConstrainedLocalVector(Vector3.forward) * adjustedSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y < 10)
        {
            transform.position += ConstrainedLocalVector(Vector3.back) * adjustedSpeed * Time.deltaTime;
        }
    }
    
    Vector3 ConstrainedLocalVector(Vector3 vector)
    {
        Vector3 movementVec = transform.TransformDirection(vector);
        movementVec.y = 0;

        return movementVec;
    }


    void ContinuousRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            startX = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.mousePosition.x;
            float deltaX = mouseX - startX;

            transform.Rotate(Vector3.up * deltaX * Time.deltaTime, Space.World);
        }
    }

    void MouseRotation()
    {
        if (Input.GetMouseButton(1))
        {
            isRotating = true;
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * rotspeed * Time.deltaTime, Space.World);
        }
        
    }

    void MouseOrbit()
    {
        if (Input.GetMouseButtonDown(2))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                orbitPoint = hit.point;
            }
            else
            {
                orbitPoint = transform.position + transform.TransformDirection(Vector3.forward) * 50;
            }
        }

        //orbitPoint = transform.position + transform.TransformDirection(Vector3.forward) * transform.position.y;

        Vector3 adjustedOrbit = new Vector3(orbitPoint.x, transform.position.y, orbitPoint.z);
        

        if (isContinuous)
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                rotAmt = .1f;
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                rotAmt = -.1f;
            }
        }
        else
        {
            rotAmt = Input.GetAxis("Mouse X");
        }

        
        if (Input.GetMouseButton(2))
        {
            isRotating = true;
            transform.RotateAround(adjustedOrbit, Vector3.up, rotAmt * rotspeed * Time.deltaTime);
        }
    }

    void KeyboardVerticalMovement()
    {
        float adjustedSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            adjustedSpeed *= 4;
        }

        if (Input.GetKey(KeyCode.Q) && transform.position.y > 1)
        {
            transform.Translate(Vector3.down * adjustedSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E) && transform.position.y < 100)
        {
            transform.Translate(Vector3.up * adjustedSpeed * Time.deltaTime);
        }
    }
    
    void MouseWheelZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (transform.position.y <= 15 && scroll > 0 || transform.position.y >= 100 && scroll < 0)
            return;

        float adjustedSpeed = speed * 10;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            adjustedSpeed *= 2;
        }
        
        
        transform.Translate(Vector3.forward * scroll * adjustedSpeed * 100 * Time.deltaTime);
    }

    void ConstrainY()
    {
        if (transform.position.y < 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        if (transform.position.y > 100)
        {
            transform.position = new Vector3(transform.position.x, 100, transform.position.z);
        }
    }
}
