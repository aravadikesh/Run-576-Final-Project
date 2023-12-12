/*AUTHOR: YUSEF*/

using UnityEngine;

public class Objects : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    void Start()
    {
        FindLand();
    }

    public void FindLand()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            if(debug) Debug.Log("Land found in negative direction: " + hitInfo.point.ToString());
        }

        else
        {
            ray = new Ray(transform.position, transform.up);
            if (Physics.Raycast(ray, out hitInfo))
            {
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                if(debug) Debug.Log("Land found in positive direction: " + hitInfo.point.ToString());
            }
        }
    }
}