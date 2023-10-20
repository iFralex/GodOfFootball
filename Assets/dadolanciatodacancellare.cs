using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dadolanciatodacancellare : MonoBehaviour
{
    public Vector3 vet;
    public float an;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(vai());
    }
    IEnumerator vai()
    {
        yield return new WaitForSeconds(4);
        transform.eulerAngles = new Vector3(Random.Range(-an, an), Random.Range(-an, an), Random.Range(-an, an));
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(vet, ForceMode.Impulse);
        //GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-an, an), Random.Range(-an, an), Random.Range(-an, an)), ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
