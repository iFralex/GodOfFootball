using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class navigazioneMenù : MonoBehaviour
{
    public string pallaSt, trigger1, trigger2;
    public UnityEngine.UI.Image palla, pannello, mascOFemmIm;
    public GameObject privacyPan, princ, match, tournament, leggiReg, scegliIta, scegliIng, scegliSpa, scegliGer, scegliArg, scegliBra, scegliCol, scegliUru, scegliNaz, leggiRegIta, leggiRegIng, leggiRegSpa, leggiRegTed, ranking, scegliTuaSquadra, scegliSquadraAvver, repilogoMultipplayer;
    public AudioSource suono;
    public Sprite maschiIc, femminaIc;
    bool multiplayer;

    [Header("multiplayer")]
    public InputField nomeUtente;
    public Text squadraT;
    public Text puntiT;
    public Text vinciteT;
    public Text pareggiT;

    void Start()
    {
        if (!PlayerPrefs.HasKey("primo accesso"))
        {
            PlayerPrefs.SetInt("primo accesso", 0);
            privacyPan.SetActive(true);
        }
    }

    public void ApriLink(string s) => Application.OpenURL(s);

    void Animazione(GameObject ob, string trigger, bool b)
    {
        ob.GetComponent<Animator>().SetBool(trigger, b);
    }

    IEnumerator Transizione(GameObject ob1, GameObject ob2)
    {
        suono.Play();
        pannello.gameObject.SetActive(true);
        Animazione(ob1, trigger1, true);
        yield return new WaitForSeconds(.5f);
        palla.gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger(pallaSt);
        yield return new WaitForSeconds(.25f);
        Animazione(ob1, trigger1, false);
        ob1.SetActive(false);
        ob2.SetActive(true);
        Animazione(ob2, trigger2, true);
        yield return new WaitForSeconds(.25f);
        palla.gameObject.SetActive(false);
        Animazione(ob2, trigger2, false);
        yield return new WaitForSeconds(.5f);
        pannello.gameObject.SetActive(false);
    }

    public void PrincMatch(bool b)
    {
        multiplayer = b;
        scegliTuaSquadra.SetActive(true);
        StartCoroutine(Transizione(princ, match));
    }

    public void MatchPrinc()
    {
        multiplayer = false;
        scegliTuaSquadra.SetActive(false);
        scegliSquadraAvver.SetActive(false);
        StartCoroutine(Transizione(match, princ));
    }

    public void PrincTournament()
    {
        StartCoroutine(Transizione(princ, tournament));
    }

    public void TournamentPrinc()
    {
        StartCoroutine(Transizione(tournament, princ));
    }

    public void PrincLeggiRegole()
    {
        StartCoroutine(Transizione(princ, leggiReg));
    }

    public void LeggiRegolePrinc()
    {
        StartCoroutine(Transizione(leggiReg, princ));
    }

    public void MatchScegliIta()
    {
        StartCoroutine(Transizione(match, scegliIta));
    }

    public void ScegliItaMatch()
    {
        StartCoroutine(Transizione(scegliIta, match));
    }

    public void MatchScegliIng()
    {
        StartCoroutine(Transizione(match, scegliIng));
    }

    public void ScegliIngMatch()
    {
        StartCoroutine(Transizione(scegliIng, match));
    }

    public void MatchScegliSpa()
    {
        StartCoroutine(Transizione(match, scegliSpa));
    }

    public void ScegliSpaMatch()
    {
        StartCoroutine(Transizione(scegliSpa, match));
    }

    public void MatchScegliGer()
    {
        StartCoroutine(Transizione(match, scegliGer));
    }

    public void ScegliGerMatch()
    {
        StartCoroutine(Transizione(scegliGer, match));
    }

    public void MatchScegliArg()
    {
        StartCoroutine(Transizione(match, scegliArg));
    }

    public void ScegliArgMatch()
    {
        StartCoroutine(Transizione(scegliArg, match));
    }

    public void MatchScegliBra()
    {
        StartCoroutine(Transizione(match, scegliBra));
    }

    public void ScegliBraMatch()
    {
        StartCoroutine(Transizione(scegliBra, match));
    }

    public void MatchScegliCol()
    {
        StartCoroutine(Transizione(match, scegliCol));
    }

    public void ScegliColMatch()
    {
        StartCoroutine(Transizione(scegliCol, match));
    }

    public void MatchScegliUru()
    {
        StartCoroutine(Transizione(match, scegliUru));
    }

    public void ScegliUruMatch()
    {
        StartCoroutine(Transizione(scegliUru, match));
    }

    public void MatchScegliNaz()
    {
        StartCoroutine(Transizione(match, scegliNaz));
    }

    public void ScegliNazMatch()
    {
        StartCoroutine(Transizione(scegliNaz, match));
    }

    public void LeggiRegoleLeggiRegoleIta()
    {
        StartCoroutine(Transizione(leggiReg, leggiRegIta));
    }

    public void LeggiRegoleItaLeggiRegole()
    {
        StartCoroutine(Transizione(leggiRegIta, leggiReg));
    }

    public void LeggiRegoleLeggiRegoleIng()
    {
        StartCoroutine(Transizione(leggiReg, leggiRegIng));
    }

    public void LeggiRegoleIngLeggiRegole()
    {
        StartCoroutine(Transizione(leggiRegIng, leggiReg));
    }

    public void LeggiRegoleLeggiRegoleSpa()
    {
        StartCoroutine(Transizione(leggiReg, leggiRegSpa));
    }

    public void LeggiRegoleSpaLeggiRegole()
    {
        StartCoroutine(Transizione(leggiRegSpa, leggiReg));
    }

    public void LeggiRegoleLeggiRegoleTed()
    {
        StartCoroutine(Transizione(leggiReg, leggiRegTed));
    }

    public void LeggiRegoleTedLeggiRegole()
    {
        StartCoroutine(Transizione(leggiRegTed, leggiReg));
    }

    public void SeasonRanking()
    {
        //StartCoroutine(Transizione(multiplayer, ranking));
    }

    public void RankingReason()
    {
        //StartCoroutine(Transizione(ranking, season));
    }

    public void MultipPrinc()
    {
        multiplayer = false;
        princ.SetActive(true);
        princ.GetComponent<Animator>().SetBool("en", true);
        StartCoroutine(a());
    }
    IEnumerator a()
    {
        yield return new WaitForSeconds(.1f);
        princ.GetComponent<Animator>().SetBool("en", false);
    }
    public void ScegliSquadra(string s)
    {
        if (multiplayer)
        {
            multiplayer = false;
            managerOnline.nomeSquadra = s;
            scegliIta.SetActive(false);
            scegliGer.SetActive(false);
            scegliIng.SetActive(false);
            scegliSpa.SetActive(false);
            scegliUru.SetActive(false);
            scegliNaz.SetActive(false);
            repilogoMultipplayer.SetActive(true);
            nomeUtente.text = PlayerPrefs.HasKey("nome utente") ? PlayerPrefs.GetString("nome utente") : "";
            squadraT.text = s;
            puntiT.text = PlayerPrefs.HasKey("punti") ? PlayerPrefs.GetInt("punti").ToString() : "0";
            vinciteT.text = PlayerPrefs.HasKey("vincite") ? PlayerPrefs.GetInt("vincite").ToString() : "0";
            pareggiT.text = PlayerPrefs.HasKey("pareggi") ? PlayerPrefs.GetInt("pareggi").ToString() : "0";

        }
        else
        { 
            if (managerOffline.nomeSquadra == "")
            {
                managerOffline.nomeSquadra = s;
                scegliTuaSquadra.SetActive(false);
                scegliSquadraAvver.SetActive(true);
                GameObject a = null;
                if (scegliIta.activeInHierarchy)
                    a = scegliIta;
                else if (scegliIng.activeInHierarchy)
                    a = scegliIng;
                else if (scegliSpa.activeInHierarchy)
                    a = scegliSpa;
                else if (scegliGer.activeInHierarchy)
                    a = scegliGer;
                else if (scegliArg.activeInHierarchy)
                    a = scegliArg;
                else if (scegliBra.activeInHierarchy)
                    a = scegliBra;
                else if (scegliCol.activeInHierarchy)
                    a = scegliCol;
                else if (scegliUru.activeInHierarchy)
                    a = scegliUru;
                else if (scegliNaz.activeInHierarchy)
                    a = scegliNaz;
                StartCoroutine(Transizione(a, match));
            }
            else
            {
                managerOffline.nomeSquadraNemica = s;
                //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
                SceneManager.LoadScene(2);
            }
        }
    }

    public void ScegliMaschiOFemmina(bool b)
    {
        if (b)
            mascOFemmIm.color = new Color(0, .5f, 1, 1);
        else
            mascOFemmIm.color = new Color(1, .5f, 1, 1);
        managerOffline.maschi = b;
    }

    public void SalvaNome(string s) => PlayerPrefs.SetString("nome utente", s);
}