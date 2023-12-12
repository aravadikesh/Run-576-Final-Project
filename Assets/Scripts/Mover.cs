using UnityEngine;

public class Mover : MonoBehaviour
{
    private float speed = 2.0f;
    public GameObject character;

    void Update ()
    {
		
		if (Input.GetKey(KeyCode.D)){
			transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A)){
			transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.W)){
			transform.position += character.transform.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S)){
			transform.position -= character.transform.forward* speed * Time.deltaTime;
		}
    }
}