/*AUTHOR: ARAV*/

using UnityEngine;

public class Objects : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    private int ignoreLayerMask;

    void Update()
    {
        FindLand();
    }

    public void FindLand()
    {
        ignoreLayerMask = 1 << 2;
        ignoreLayerMask = ~ignoreLayerMask;

        if(debug) Debug.Log("In FindLand");
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, ignoreLayerMask))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            if(debug) Debug.Log("Land found in negative direction: " + hitInfo.point.ToString());
        }

        else
        {
            ray = new Ray(transform.position, transform.up);
            if (Physics.Raycast(ray, out hitInfo,  Mathf.Infinity, ignoreLayerMask))
            {
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                if(debug) Debug.Log("Land found in positive direction: " + hitInfo.point.ToString());
            }
        }
    }
}