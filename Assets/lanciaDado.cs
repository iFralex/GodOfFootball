using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lanciaDado : MonoBehaviour
{
    public Rigidbody dado;
    public Rigidbody[] dadi;
    public ParticleSystem effetto;
    public int risultato = -1;
    public int[] risultati = { -1, -1 };
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public Animator animazione, animazioneDoppio;

    float velRot(float n)
    {
        return Random.Range(-n, n);
    }

    void Start() => risultati[0] = risultati[1] = risultato = -1;

    void PreparaLancio(Quaternion? rotazione = null, Quaternion? rotazione2 = null)
    {
        if (rotazione == null)
            rotazione = Random.rotation;
        if (rotazione2 == null)
            rotazione2 = Random.rotation;
        eventSystem.gameObject.SetActive(false);
        risultato = risultati[0] = risultati[1] = -1;
        dado.transform.rotation = (Quaternion)rotazione;
        dadi[0].transform.rotation = (Quaternion)rotazione;
        dadi[1].transform.rotation = (Quaternion)rotazione2;
        GetComponent<CameraFit>().CambiaModalità(RenderMode.ScreenSpaceCamera);
    }

    void FineLancio()
    {
        if (PhotonNetwork.connected)
            GetComponent<managerOnline>().cardSpAnim.SetBool("en", false);
        else
            GetComponent<managerOffline>().cardSpAnim.SetBool("en", false);
        GetComponent<CameraFit>().CambiaModalità(RenderMode.ScreenSpaceOverlay);
        eventSystem.gameObject.SetActive(true);
        risultato = -1;
        risultati[0] = risultati[1] = -1;
    }

    [PunRPC]
    public void Lancia(Vector3? forza = null)
    {
        if (!PhotonNetwork.connected)
            forza = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        Vector3 f = (Vector3)forza;
        PreparaLancio(Quaternion.Euler(f.x * 3000, f.y / 4 * 1000, f.z * 1000 + 39));
        StartCoroutine(EffettuaLancio(f));
    }

    IEnumerator EffettuaLancio(Vector3 forza)
    {
        if (!PhotonNetwork.connected)
            forza = new Vector3(velRot(10f), velRot(10f), velRot(10f));
        animazione.SetBool("entra", true);
        yield return new WaitForSeconds(2);
        dado.AddTorque(forza, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => dado.angularVelocity != Vector3.zero);
        effetto.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            int r = System.Convert.ToInt32(hit.collider.name);
            animazione.SetInteger("risultato", r);
            yield return new WaitForSeconds(.5f);
            animazione.SetInteger("risultato", 0);
            yield return new WaitForSeconds(.5f);
            risultato = r;
        }
        yield return new WaitForSeconds(1);
        animazione.SetBool("entra", false);
        yield return new WaitForSeconds(1);
        FineLancio();
    }

    [PunRPC]
    public void LanciaDoppio(Vector3? forza1 = null, Vector3? forza2 = null)
    {
        if (!PhotonNetwork.connected)
        {
            forza1 = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            forza2 = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }
        Vector3[] forze = { (Vector3)forza1, (Vector3)forza2 };
        //print("forze =  a: " + forze[0] + "  b: " + forze[1]);
        PreparaLancio(Quaternion.Euler(forze[0].x * 3000, forze[0].y / 4 * 1000, forze[0].z * 1000 + 39), Quaternion.Euler(forze[1].x * 3000, forze[1].y / 4 * 1000, forze[1].z * 1000 + 39));
        StartCoroutine(EffettuaLancioDoppio(forze));
    }

    IEnumerator EffettuaLancioDoppio(Vector3[] forze = null)
    {
        if (forze == null)
        {
            forze = new Vector3[] { new Vector3(velRot(10f), velRot(10f), velRot(10f)), new Vector3(velRot(10f), velRot(10f), velRot(10f)) };
            print("a");
        }
        animazioneDoppio.SetBool("entra", true);
        yield return new WaitForSeconds(2);
        float n = 10f;
        for (int i = 0; i < dadi.Length; i++)
            dadi[i].AddTorque(forze[i], ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => dadi[0].angularVelocity != Vector3.zero && dadi[1].angularVelocity != Vector3.zero);
        foreach (Rigidbody d in dadi)
            d.GetComponentInChildren<ParticleSystem>().Play();
        int[] r = new int[2];
        for (int i = 0; i < dadi.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(dadi[i].transform.position.x, dadi[i].transform.position.y, transform.position.z), transform.forward, out hit, 100))
            {
                r[i] = System.Convert.ToInt32(hit.collider.name);
                dadi[i].GetComponent<Animator>().SetInteger("risultato", r[i]);
            }
        }
        yield return new WaitForSeconds(.5f);
        dadi[0].GetComponent<Animator>().SetInteger("risultato", 0);
        dadi[1].GetComponent<Animator>().SetInteger("risultato", 0);
        yield return new WaitForSeconds(.5f);
        risultati = r;

        yield return new WaitForSeconds(1);
        animazioneDoppio.SetBool("entra", false);
        yield return new WaitForSeconds(1);
        FineLancio();
    }
}