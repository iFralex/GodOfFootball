using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lanciaMoneta : MonoBehaviour
{
    public List<Sprite> monetaIc;
    public Image moneta, pannelloLanMon;
    public Text testaOCroceT, sceltoTestaT, sceltoCroceT;
    managerOffline mo => GetComponent<managerOffline>();

    public void RuotaMoneta(int n) => StartCoroutine(LanciaMoneta(.07f, .001f, Random.Range(0, 2), true, n));

    public IEnumerator LanciaMoneta(float differenza, float durata, int n, bool a = false, int testaOCroce = 1)
    {
        mo.pannello.gameObject.SetActive(true);
        if (a)
        {
            pannelloLanMon.gameObject.SetActive(true);
            if (testaOCroce == 1)
            {
                moneta.transform.parent.GetComponent<Animator>().SetBool("testa", true);
                sceltoTestaT.text = traduzioni.traduci("Testa");
            }
            else
            {
                moneta.transform.parent.GetComponent<Animator>().SetBool("croce", true);
                sceltoCroceT.text = traduzioni.traduci("Croce");
            }
            yield return new WaitForSeconds(2);
        }

        for (int i = 0; i < 10 + n; i++)
        {
            if (i < 5)
                differenza += 0.05f;
            else
                differenza -= 0.05f;

            float size = 1;
            while (size > 0.08)
            {
                size -= differenza;
                moneta.rectTransform.localScale = new Vector3(size, 1, 1);
                yield return new WaitForSeconds(durata);
            }

            if (moneta.sprite == monetaIc[1])
                moneta.sprite = monetaIc[0];
            else
                moneta.sprite = monetaIc[1];

            while (size < 1f)
            {
                size += differenza;
                moneta.rectTransform.localScale = new Vector3(size, 1, 1);
                yield return new WaitForSeconds(durata);
            }
        }

        if (a)
        {
            yield return new WaitForSeconds(1);
            moneta.transform.parent.GetComponent<Animator>().SetBool("entra", false);
            if (testaOCroce == 1)
                moneta.transform.parent.GetComponent<Animator>().SetBool("testa", false);
            else
                moneta.transform.parent.GetComponent<Animator>().SetBool("croce", false);
            yield return new WaitForSeconds(.25f);
            if (testaOCroce != n)
            {
                testaOCroceT.text = traduzioni.traduci("Vinto");
                testaOCroceT.color = Color.green;
                mo.proprietarioPalla = mo.playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, mo.playersCompagni);
            }
            else
            {
                testaOCroceT.text = traduzioni.traduci("Perso");
                testaOCroceT.color = Color.red;
                mo.proprietarioPalla = mo.playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, mo.playersAvver);
            }
            testaOCroceT.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            mo.proprietarioPalla.AttivaFrecce();
            mo.PosizionaPalla(mo.proprietarioPalla);
            pannelloLanMon.gameObject.SetActive(false);
            mo.pannello.gameObject.SetActive(false);
            mo.timerC = mo.StartCoroutine(mo.TimerAzione());
            mo.StartCoroutine(mo.TimerPartita());
            mo.ruoloMioT.gameObject.SetActive(true);
            mo.ruoloAvvT.gameObject.SetActive(true);
        }
    }
}