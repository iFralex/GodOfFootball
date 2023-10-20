using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class provaSquadre : MonoBehaviour
{
    static int squadraAtt;
    
    void Start()
    {
        StartCoroutine(Prova());
    }

    IEnumerator Prova()
    {
        yield return null;
        /*transform.GetChild(squadraAtt + 1).GetComponent<Button>().onClick.Invoke();
        transform.GetChild(squadraAtt + 2).GetComponent<Button>().onClick.Invoke();
        squadraAtt += 2;*/
        transform.GetChild(squadraAtt).GetComponent<Button>().onClick.Invoke();
        transform.GetChild(squadraAtt + 1).GetComponent<Button>().onClick.Invoke();
        squadraAtt += 2;
    }
}
