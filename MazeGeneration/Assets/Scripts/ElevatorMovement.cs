using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{
    public Vector3 elevatorVelocity;
    public float finalElevatorHeight;
    private bool goingDown = false;
    private bool goingUp = false;
    private float renderIntensity;
    // Start is called before the first frame update
    void Start()
    {
        renderIntensity = RenderSettings.ambientIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown("b"))
        {
            StartCoroutine("MoveDown");
        }

        if (Input.GetKeyDown("v"))
        {
            StartCoroutine("MoveUp");
        } */
    }

    public IEnumerator MoveDown()
    {
        if (GameObject.FindGameObjectWithTag("FuseBox").GetComponent<FuseBoxPuzzle>().correctPlugs == 5)
        {
            goingUp = false;
            StopCoroutine("GoingUp");
            goingDown = true;
            while (transform.position.y > finalElevatorHeight && goingDown)
            {
                transform.position = transform.position + elevatorVelocity * Time.fixedDeltaTime;
                GameObject.FindGameObjectWithTag("CameraRig").transform.position = transform.position + elevatorVelocity * Time.fixedDeltaTime;
                yield return new WaitForSeconds(0f);
            }
            goingDown = false;
            RenderSettings.ambientIntensity = 0;
        } else {
            Debug.Log("Puzzle not done!! " + GameObject.FindGameObjectWithTag("FuseBox").GetComponent<FuseBoxPuzzle>().correctPlugs + "/5 plugs are placed correctly.");
            // Puzzle not solved
            // Some sound feedback or something??
        }
    }

    public IEnumerator MoveUp()
    {
        if (GameObject.FindGameObjectWithTag("FuseBox").GetComponent<FuseBoxPuzzle>().correctPlugs == 5)
        {
            goingDown = false;
            StopCoroutine("GoingDown");
            goingUp = true;
            while (transform.position.y < 0 && goingUp)
            {
                transform.position = transform.position - elevatorVelocity * Time.fixedDeltaTime;
                GameObject.FindGameObjectWithTag("CameraRig").transform.position = transform.position - elevatorVelocity * Time.fixedDeltaTime;
                yield return new WaitForSeconds(0f);
            }
            goingUp = false;
            RenderSettings.ambientIntensity = 1;
        } else {
            // Puzzle not solved
            // Some sound feedback or something??
            Debug.Log("Puzzle not done!! " + GameObject.FindGameObjectWithTag("FuseBox").GetComponent<FuseBoxPuzzle>().correctPlugs + "/5 plugs are placed correctly.");
        }
    }
}
