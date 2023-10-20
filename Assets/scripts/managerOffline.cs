using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Placement;
using GoogleMobileAds.Api;
using GoogleMobileAds;

public class managerOffline : MonoBehaviour
{
    public enum Spiegazioni { ammonizioneRosso, ammonizioneGiallo, fuori, rigore, passaggio, cross, tiro, calcioDangolo, dribling, doppioDribling, punizione, colpoDiTesta };
    public giocatore[] playersCompagni, playersAvver;
    public RectTransform palla, pannello;
    public Transform squadraAmica, squadraNemicaTra;
    giocatore _proprietarioPalla;
    
    public giocatore proprietarioPalla
    {
        get { return _proprietarioPalla; }

        set
        {
            _proprietarioPalla = value;
            ProprietarioPallaAggiornato();
        }
    }
    public List<azioniFreccia> listaFrecce, listaFrecceAvver;
    public Squadra miaSquadra, squadraNemica;
    lanciaDado ld;
    int _goal, _goalAvver;
    public int goal
    {
        get { return _goal; }
        set
        {
            _goal = value;
            goalMieiT.text = "Goal: " + value.ToString();
        }
    }
    public int goalAvver
    {
        get { return _goalAvver; }
        set
        {
            _goalAvver = value;
            goalAvverT.text = "Goal: " + value.ToString();
        }
    }
    bool driblingEffettuato, driblingNemicoEffettuato, aereo = true, aereoNemico = true, timerAttivo, partitaConclusa;
    public Coroutine timerC;
    public Squadra[] squadre;
    public static string nomeSquadra = "", nomeSquadraNemica = "";
    public AudioSource coro;

    [Header("Maglie e sesso")]
    public List<Sprite> visiUomo;
    public List<Sprite> visiDonna;
    public static bool maschi;

    [Header("Doppio lancio")]
    public RectTransform lineeGrigliaX;
    public RectTransform lineeGrigliaY;
    List<RectTransform> grigliaX = new List<RectTransform>(0), grigliaY = new List<RectTransform>(0);
    
    [Header("Tiro cross")]
    public Animator scegliLato;

    [Header("Goal")]
    public Animator goalAnim;
    public ParticleSystem goalPS;
    public Text goalMieiT;
    public Text goalAvverT;
    public Text ruoloMioT;
    public Text ruoloAvvT;

    [Header("Parata")]
    public Image parataIm;

    [Header("Card spiegazione")]
    public Text cardSpT;
    public Image cardSpIm;
    public Sprite[] cards;
    public Animator cardSpAnim;

    [Header("Timer")]
    public RectTransform timerIm;
    public Text timerPartitaT;

    [Header("Fine")]
    public Animator fineAnim;
    public Text titoloFineT;
    public Text statoMiaSquadraFineT;
    public Text statoAvverSquadraFineT;
    public Text nomeSquadraMiaFineT;
    public Text nomeSquadraAvverFineT;
    public Text goalMieiFineT;
    public Text goalAvverFineT;

    public giocatore playerDestinatario(azioniFreccia freccia)
    {
        for (int i = 0; i < freccia.destinatario.Length; i++)
            if (freccia.destinatario[i] != proprietarioPalla)
                return freccia.destinatario[i];
        return null;
    }

    public giocatore playerDestinatarioSpecifico(giocatore.Ruoli r, giocatore[] pl, giocatore.Posizioni p = giocatore.Posizioni.nulla)
    {
        foreach (giocatore g in pl)
            if (g.ruolo == r && g.posizione == p)
                return g;
        return null;
    }

    public giocatore playerPiùVicino(Vector2 origine, giocatore[] _players)
    {
        float distanza = 50000;
        giocatore a = null;
        for (int i = 0; i < _players.Length; i++)
            if (_players[i].ruolo != giocatore.Ruoli.portiere)
                if (Vector2.Distance(_players[i].GetComponent<RectTransform>().position, origine) < distanza)
                {
                    a = _players[i];
                    distanza = Vector2.Distance(_players[i].GetComponent<RectTransform>().position, origine);
                }
        return a;
    }

    azioniFreccia scegliFrecciaCasuale(giocatore player)
    {
        return player.frecceDisponibili[Random.Range(0, player.frecceDisponibili.Length)];
    }

    public bool playerÈCompagno(giocatore _player)
    {
        for (int i = 0; i < playersAvver.Length; i++)
            if (playersAvver[i] == _player)
                return false;
        return true;
    }

    public giocatore[] playersContrari(giocatore player, bool b = true)
    {
        if (b)
            if (playerÈCompagno(player))
                return playersAvver;
            else
                return playersCompagni;
        else
        {
            if (playerÈCompagno(player))
                return playersCompagni;
            else
                return playersAvver;
        }
    }

    void Start()
    {
        //GoogleMobileAds.Api.MobileAds.Instance.GetAd<InterstitialAdGameObject>("ad o").LoadAd();
        for (int i = 0; i < squadre.Length; i++)
            if (squadre[i].nome == nomeSquadra)
                miaSquadra = squadre[i];

        for (int i = 0; i < squadre.Length; i++)
            if (squadre[i].nome == nomeSquadraNemica)
                squadraNemica = squadre[i];
        
        for (int i = 0; i < lineeGrigliaX.childCount; i++)
            grigliaX.Add(lineeGrigliaX.GetChild(i).GetComponent<RectTransform>());

        for (int i = 0; i < lineeGrigliaY.childCount; i++)
            grigliaY.Add(lineeGrigliaY.GetChild(i).GetComponent<RectTransform>());

        ld = GetComponent<lanciaDado>();
        List<Sprite> visi = new List<Sprite>(0), visiAvv = new List<Sprite>(0);
        if (maschi)
            for (int i = 0; i < visiUomo.Count; i++)
            {
                visi.Add(visiUomo[i]);
                visiAvv.Add(visiUomo[i]);
            }
        else
            for (int i = 0; i < visiUomo.Count; i++)
            {
                visi.Add(visiDonna[i]);
                visiAvv.Add(visiDonna[i]);
            }

        for (int i = 0; i < playersCompagni.Length; i++)
        {
            playersCompagni[i] = squadraAmica.GetChild(i).GetComponent<giocatore>();
            playersAvver[i] = squadraNemicaTra.GetChild(i).GetComponent<giocatore>();
        }

        for (int i = 0; i < playersCompagni.Length; i++)
        {
            int n = Random.Range(0, visi.Count);
            playersCompagni[i].GetComponent<Image>().sprite = visi[n];
            visi.RemoveAt(n);
            if (playersCompagni[i].ruolo != giocatore.Ruoli.portiere)
                playersCompagni[i].transform.GetChild(0).GetComponent<Image>().sprite = miaSquadra.maglia;
        }

        for (int i = 0; i < playersAvver.Length; i++)
        {
            int n = Random.Range(0, visiAvv.Count);
            playersAvver[i].GetComponent<Image>().sprite = visiAvv[n];
            visiAvv.RemoveAt(n);
            if (playersAvver[i].ruolo != giocatore.Ruoli.portiere)
                playersAvver[i].transform.GetChild(0).GetComponent<Image>().sprite = squadraNemica.maglia;
        }

        for (int i = 0; i < playersCompagni.Length; i++)
            playersCompagni[i].squadra = miaSquadra;

        for (int i = 0; i < playersAvver.Length; i++)
            playersAvver[i].squadra = squadraNemica;

        if (miaSquadra.coro != null)
        {
            coro.clip = miaSquadra.coro;
            coro.Play();
        }
        StartCoroutine(GetComponent<lanciaMoneta>().LanciaMoneta(.07f, .001f, 0));
    }

    public void PosizionaPalla(giocatore _player)
    {
        float n = 75;
        if (_player.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceCamera)
            n = 0.6f;
        if (!playerÈCompagno(_player))
            n *= -1;
        
        Vector3 _pos = _player.GetComponent<RectTransform>().position;
        palla.position = new Vector3(_pos.x, _pos.y - n, _pos.z);
    }

    public void AttivaAzione(azioniFreccia freccia)
    {/*
#if UNITY_EDITOR
        if (freccia.tipo == azioniFreccia.Azione.cross)
            freccia.tipo = azioniFreccia.Azione.passaggio;
#endif*/
        ld.eventSystem.enabled = false;
        if (timerAttivo)
        {
            StopCoroutine(timerC);
            timerIm.parent.gameObject.SetActive(false);
            timerAttivo = false;
        }

        switch (freccia.tipo)
        {
            case azioniFreccia.Azione.passaggio:
                StartCoroutine(OttieniRisultatoPassaggio(freccia));
                break;
            case azioniFreccia.Azione.dribling:
                StartCoroutine(OttieniRisultatoDribling(freccia));
                break;
            case azioniFreccia.Azione.cross:
                StartCoroutine(OttieniRisultatoCross(freccia));
                break;
            case azioniFreccia.Azione.tiro:
                if (playerÈCompagno(proprietarioPalla))
                {
                    if (driblingEffettuato)
                        StartCoroutine(DoppioDribling());
                    else
                        AttivaTiroCross();
                }
                else
                {
                    if (driblingNemicoEffettuato)
                        StartCoroutine(DoppioDribling());
                    else
                        AttivaTiroCross();
                }
                break;
        }
    }

    IEnumerator OttieniRisultatoPassaggio(azioniFreccia freccia)
    {
        print(ld.risultato + "   " + ld.risultati[0] + "   " + ld.risultati[1]);
        yield return new WaitWhile(() => !(ld.risultato == -1 && ld.risultati[0] == -1 && ld.risultati[1] == -1));
        print("passaggio  |    proprietario: " + proprietarioPalla.name + "   destinatario: " + playerDestinatario(freccia).name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   tempo: " + Time.time);
        CardSpiegazione(Spiegazioni.passaggio);
        CardSpiegazione("+...");
        ld.Lancia();
        yield return new WaitWhile(() => ld.risultato == -1);

        void _Passaggio()
        {
            CardSpiegazione(Spiegazioni.passaggio);
            giocatore g = playerDestinatario(freccia);
            if (g == null)
                proprietarioPalla = playerPiùVicino(proprietarioPalla.transform.position, playersContrari(proprietarioPalla, false));
            else
                proprietarioPalla = g;
            PosizionaPalla(proprietarioPalla);
        }

        void _RubaPalla()
        {
            CardSpiegazione(traduzioni.traduci("Palla rubata"));
            giocatore player = playerDestinatario(freccia);
            Vector2 puntoMedio = (player.GetComponent<RectTransform>().position + proprietarioPalla.GetComponent<RectTransform>().position) / 2;
            puntoMedio = ((Vector2)player.GetComponent<RectTransform>().position + puntoMedio) / 2;
            proprietarioPalla = playerPiùVicino(puntoMedio, playersContrari(proprietarioPalla));
            PosizionaPalla(proprietarioPalla);
            if (playerÈCompagno(proprietarioPalla))
            {
                driblingEffettuato = true;
                aereo = false;
            }
            else
            {
                driblingNemicoEffettuato = true;
                aereo = false;
            }
            print("passaggio fallito   |    proprietario: " + proprietarioPalla.name + "   compagno?: " + playerÈCompagno(proprietarioPalla));
        }

        switch (ld.risultato)
        {
            case 1:
                _RubaPalla();
                proprietarioPalla.AttivaFrecce();
                break;
            default:
                _Passaggio();
                proprietarioPalla.AttivaFrecce();
                break;
        }
        timerC = StartCoroutine(TimerAzione(2));
    }
    
    IEnumerator OttieniRisultatoDribling(azioniFreccia freccia)
    {
        print("dribling  |    proprietario: " + proprietarioPalla.name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   tempo: " + Time.time);
        if (playerÈCompagno(proprietarioPalla))
            driblingEffettuato = true;
        else
            driblingNemicoEffettuato = true;
        CardSpiegazione(Spiegazioni.dribling);
        CardSpiegazione("+...");
        int f = 0;
        if (playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, playersContrari(proprietarioPalla)) != null)
        {
            ld.LanciaDoppio();
            yield return new WaitWhile(() => ld.risultati[0] == -1 && ld.risultati[1] == -1);
        }
        else
        { 
            _Dribling();
            proprietarioPalla.AttivaFrecce();
            driblingEffettuato = true;
            timerC = StartCoroutine(TimerAzione(2));
            yield break;
        }
        
        void _Dribling()
        {
            CardSpiegazione(Spiegazioni.dribling);
            proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.punta, playersContrari(proprietarioPalla, false));
            PosizionaPalla(proprietarioPalla);
            print("dribling passaggio  |    proprietario: " + proprietarioPalla.name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   tempo: " + Time.time);
        }

        void _RubaPalla()
        {
            CardSpiegazione(Spiegazioni.dribling);
            CardSpiegazione(traduzioni.traduci("Palla rubata"));
            proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, playersContrari(proprietarioPalla));
            PosizionaPalla(proprietarioPalla);
            print("dribling palla rubata  |    proprietario: " + proprietarioPalla.name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   tempo: " + Time.time);
        }

        void _Fallo()
        {
            giocatore g = playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, playersContrari(proprietarioPalla));
            if (g != null)
                g.Fallo(true);
            print(g);
            if (g != null)
            {
                if (g.falli < 2)
                    f = g.falli;
                else
                {
                    f = 2;
                    print("passaggio dribling");
                    CardSpiegazione(Spiegazioni.ammonizioneRosso);
                    _Dribling();
                }
            }
            else
            {
                f = 2;
                print("passaggio dribling");
                CardSpiegazione(Spiegazioni.ammonizioneRosso);
                _Dribling();
            }
            proprietarioPalla.AttivaFrecce();
            PosizionaPalla(proprietarioPalla);
            print("dribling fallo  |    proprietario: " + proprietarioPalla.name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   falli: " + f + "   tempo: " + Time.time);
        }
        
        if (playerÈCompagno(proprietarioPalla))
        {
            if (ld.risultati[0] > ld.risultati[1])
            {
                _Dribling();
                proprietarioPalla.AttivaFrecce();
                driblingEffettuato = true;
            }
            else if (ld.risultati[0] < ld.risultati[1])
            {
                _RubaPalla();
                proprietarioPalla.AttivaFrecce();
            }
            else
            {
                _Fallo();
                yield return new WaitWhile(() => ld.risultati[0] != -1);
                if (f < 2)
                {
                    StartCoroutine(OttieniRisultatoDribling(freccia));
                    yield break;
                }
            }
        }
        else
        {
            if (ld.risultati[0] < ld.risultati[1])
            {
                _Dribling();
                proprietarioPalla.AttivaFrecce();
                driblingNemicoEffettuato = true;
            }
            else if (ld.risultati[0] > ld.risultati[1])
            {
                _RubaPalla();
                proprietarioPalla.AttivaFrecce();
            }
            else
            {
                _Fallo();
                yield return new WaitWhile(() => ld.risultati[0] != -1);
                if (f < 2)
                {
                    StartCoroutine(OttieniRisultatoDribling(freccia));
                    yield break;
                }
            }
        }
        print("finito");
        timerC = StartCoroutine(TimerAzione(2));
    }

    IEnumerator OttieniRisultatoCross(azioniFreccia freccia)
    {
        print("cross   |    proprietario: " + proprietarioPalla.name + "   destinatario: " + playerDestinatario(freccia).name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   tempo: " + Time.time);
        if (playerÈCompagno(proprietarioPalla))
        {
            aereo = true;
            driblingEffettuato = false;
        }
        else
        {
            aereoNemico = true;
            driblingNemicoEffettuato = false;
        }
        CardSpiegazione(Spiegazioni.cross);
        CardSpiegazione("+...");
        print("cross");
        ld.Lancia();
        yield return new WaitWhile(() => ld.risultato == -1);

        void _Passaggio()
        {
            CardSpiegazione(Spiegazioni.cross);
            proprietarioPalla = playerDestinatario(freccia);
            PosizionaPalla(proprietarioPalla);
        }

        void _RubaPalla()
        {
            CardSpiegazione(traduzioni.traduci("Palla rubata"));
            float distanza = 50000;
            giocatore a = proprietarioPalla;
            giocatore player = playerDestinatario(freccia);
            Vector2 puntoMedio = (player.GetComponent<RectTransform>().position + proprietarioPalla.GetComponent<RectTransform>().position) / 2;
            puntoMedio = ((Vector2)player.GetComponent<RectTransform>().position + puntoMedio) / 2;
            giocatore[] _players = playersContrari(proprietarioPalla);
            for (int i = 0; i < _players.Length; i++)
                if (Vector2.Distance(_players[i].GetComponent<RectTransform>().position, puntoMedio) < distanza)
                {
                    a = _players[i];
                    distanza = Vector2.Distance(_players[i].GetComponent<RectTransform>().position, puntoMedio);
                }
            proprietarioPalla = a;
            PosizionaPalla(proprietarioPalla);
            print("cross palla rubbata  |    proprietario: " + proprietarioPalla.name + "   compagno?: " + playerÈCompagno(proprietarioPalla) + "   tempo: " + Time.time);
        }

        switch (ld.risultato)
        {
            case 3:
            case 4:
                _Passaggio();
                proprietarioPalla.AttivaFrecce();
                if (playerÈCompagno(proprietarioPalla))
                    driblingEffettuato = false;
                else
                    driblingNemicoEffettuato = false;
                timerC = StartCoroutine(TimerAzione(2, true));
                break;
            default:
                _RubaPalla();
                proprietarioPalla.AttivaFrecce();
                timerC = StartCoroutine(TimerAzione(2));
                break;
        }
    }

    public void TiroDopoCross(int n)
    {
        print(timerAttivo + "  " + Time.time);
        if (timerAttivo)
        {
            StopCoroutine(timerC);
            timerIm.parent.gameObject.SetActive(false);
            timerAttivo = false;
            //print("bloccato: " + Time.time);
        }
        
        List<int> num = new List<int>(0);

        if (n < 3)
            for (int i = 0; i < 3; i++)
                num.Add(i + 1);
        else if (n < 8)
            for (int i = 0; i < 3; i++)
                num.Add(i + 4);
        StartCoroutine(TiroCross(num));
    }

    IEnumerator TiroCross(List<int> n)
    {
        if (n.Count != 0)
        {
            if (playerÈCompagno(proprietarioPalla))
                if (aereo)
                    CardSpiegazione(Spiegazioni.colpoDiTesta);
                else
                    CardSpiegazione(Spiegazioni.tiro);
            else
            {
                if (aereoNemico)
                    CardSpiegazione(Spiegazioni.colpoDiTesta);
                else
                    CardSpiegazione(Spiegazioni.tiro);
            }
            CardSpiegazione("+...");
            scegliLato.SetBool("en", false);
            ld.Lancia();
            FindObjectOfType<CameraFit>().CambiaModalità(RenderMode.ScreenSpaceCamera);
            yield return new WaitWhile(() => ld.risultato == -1);

            int risp = ld.risultato;
            if (n.Contains(risp))
            {
                print("interno");
                yield return new WaitWhile(() => ld.risultato != -1);
                CardSpiegazione(Spiegazioni.tiro);
                CardSpiegazione(traduzioni.traduci("In porta"));
                ld.Lancia();
                yield return new WaitWhile(() => ld.risultato == -1);
                int risp2 = ld.risultato;
                print(risp2 + " " + risp);
                if (risp2 == risp)
                    StartCoroutine(Fuori(true));
                else if ((risp == 1 && risp2 == 6) || (risp2 == 6 && risp == 1) || risp2 - 1 == risp || risp2 + 1 == risp)
                    StartCoroutine(CalcioDangolo());
                else
                    StartCoroutine(Goal());
            }
            else
                StartCoroutine(Fuori());
        }
        else
        {
            scegliLato.SetBool("en", false);
            StartCoroutine(Fuori());
        }
        aereo = aereoNemico = true;
    }

    IEnumerator Fuori(bool parata = false)
    {
        yield return new WaitWhile(() => ld.risultato != -1);
        print("fuori");
        PosizionaPalla(playerDestinatarioSpecifico(giocatore.Ruoli.portiere, playersContrari(proprietarioPalla)));
        CardSpiegazione(Spiegazioni.fuori);
        if (parata)
        {
            parataIm.gameObject.SetActive(true);
            CardSpiegazione(traduzioni.traduci("Parata"));
            yield return new WaitForSeconds(2);
            parataIm.gameObject.SetActive(false);
        }
        CardSpiegazione(traduzioni.traduci("Rinvio 1"));
        ld.Lancia();
        yield return new WaitWhile(() => ld.risultato == -1);
        int ris1 = ld.risultato;

        yield return new WaitWhile(() => ld.risultato != -1);
        CardSpiegazione(traduzioni.traduci("Rinvio 2"));
        ld.Lancia();
        yield return new WaitWhile(() => ld.risultato == -1);
        int ris2 = ld.risultato;
        print(ris1 + "  " + ris2);

        Vector2 pos = new Vector2(grigliaX[ris1 - 1].position.x, grigliaY[ris2 - 1].position.y);
        giocatore[] g = new giocatore[playersAvver.Length + playersCompagni.Length];
        for (int i = 0; i < g.Length; i++)
            if (i < playersAvver.Length)
                g[i] = playersAvver[i];
            else
                g[i] = playersCompagni[i - playersAvver.Length];
        proprietarioPalla = playerPiùVicino(pos, g);
        PosizionaPalla(proprietarioPalla);
        proprietarioPalla.AttivaFrecce();
        timerC = StartCoroutine(TimerAzione(2));
    }

    IEnumerator CalcioDangolo()
    {
        void _Posiziona(giocatore _player)
        {
            proprietarioPalla = _player;
            PosizionaPalla(proprietarioPalla);
            proprietarioPalla.AttivaFrecce();
        }

        if (playerÈCompagno(proprietarioPalla))
            aereo = true;
        else
            aereoNemico = true;
        print("angolo");
        yield return new WaitWhile(() => ld.risultato != -1);
        parataIm.gameObject.SetActive(true);
        CardSpiegazione(Spiegazioni.calcioDangolo);
        CardSpiegazione(traduzioni.traduci("Deviata"));
        yield return new WaitForSeconds(2);
        parataIm.gameObject.SetActive(false);
        CardSpiegazione(Spiegazioni.calcioDangolo);
        ld.Lancia();
        yield return new WaitWhile(() => ld.risultato == -1);
        int ris = ld.risultato;
        giocatore[] _players = playersContrari(proprietarioPalla);
        if (ris == 1 || ris == 2)
            _Posiziona(playerDestinatarioSpecifico(giocatore.Ruoli.terzino, _players, giocatore.Posizioni.sinistro));
        else if (ris == 5 || ris == 6)
            _Posiziona(playerDestinatarioSpecifico(giocatore.Ruoli.terzino, _players, giocatore.Posizioni.destro));
        else
            AttivaTiroCross();

        if (ris != 3 && ris != 4)
            timerC = StartCoroutine(TimerAzione(2));
    }

    public IEnumerator Goal()
    {
        yield return new WaitForSeconds(2);
        print(proprietarioPalla.name + "  " + playerÈCompagno(proprietarioPalla));
        FindObjectOfType<CameraFit>().CambiaModalità(RenderMode.ScreenSpaceCamera);
        goalPS.Play();
        goalAnim.SetTrigger("en");
        if (playerÈCompagno(proprietarioPalla))
        {
            goal++;
            goalMieiT.text = "Goal: " + goal.ToString();
        }
        else
        {
            goalAvver++;
            goalAvverT.text = "Goal: " + goalAvver.ToString();
        }
        giocatore g = playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, playersContrari(proprietarioPalla));
        if (g == null)
            proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.centrocampista, playersContrari(proprietarioPalla), giocatore.Posizioni.destro);
        else
            proprietarioPalla = g;
        PosizionaPalla(proprietarioPalla);
        proprietarioPalla.AttivaFrecce();
        timerC = StartCoroutine(TimerAzione(2));
    }

    public void AttivaTiroCross()
    {
        if (playerÈCompagno(proprietarioPalla))
            if (aereo)
                CardSpiegazione(Spiegazioni.colpoDiTesta);
            else
                CardSpiegazione(Spiegazioni.tiro);
        else
        {
            if (aereoNemico)
                CardSpiegazione(Spiegazioni.colpoDiTesta);
            else
                CardSpiegazione(Spiegazioni.tiro);
        }
        FindObjectOfType<CameraFit>().CambiaModalità(RenderMode.ScreenSpaceOverlay);
        scegliLato.SetBool("en", true);
        timerC = StartCoroutine(TimerAzione(2, true));
    }

    public void CardSpiegazione(Spiegazioni sp)
    {
        switch (sp)
        {
            case Spiegazioni.passaggio:
                cardSpT.text = traduzioni.traduci("Passaggio");
                break;
            case Spiegazioni.cross:
                cardSpT.text = traduzioni.traduci("Cross");
                break;
            case Spiegazioni.ammonizioneGiallo:
                cardSpT.text = traduzioni.traduci("Ammonizione Giallo");
                break;
            case Spiegazioni.ammonizioneRosso:
                cardSpT.text = traduzioni.traduci("Ammonizione rosso");
                break;
            case Spiegazioni.rigore:
                cardSpT.text = traduzioni.traduci("Rigore");
                break;
            case Spiegazioni.tiro:
                cardSpT.text = traduzioni.traduci("Tiro");
                break;
            case Spiegazioni.fuori:
                cardSpT.text = traduzioni.traduci("Fuori");
                break;
            case Spiegazioni.calcioDangolo:
                cardSpT.text = traduzioni.traduci("Calcio d'angolo");
                break;
            case Spiegazioni.dribling:
                cardSpT.text = traduzioni.traduci("Dribbling");
                break;
            case Spiegazioni.doppioDribling:
                cardSpT.text = traduzioni.traduci("Doppio dribbling");
                break;
            case Spiegazioni.punizione:
                cardSpT.text = traduzioni.traduci("Punizione");
                break;
            case Spiegazioni.colpoDiTesta:
                cardSpT.text = traduzioni.traduci("Colpo di testa");
                break;
        }
        cardSpIm.sprite = cards[(int)sp];
        cardSpAnim.SetBool("en", true);
    }

    public void CardSpiegazione(string s)
    {
        cardSpAnim.SetBool("en", true);
        if (s[0] != '+')
            cardSpT.text = s;
        else
        {
            string s1 = "";
            for (int i = 1; i < s.Length; i++)
                s1 += s[i] + "";
            cardSpT.text += s1;
        }
    }

    IEnumerator DoppioDribling()
    {
        pannello.gameObject.SetActive(true);
        if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro) != null)
        {
            print("doppio dribbling");
            CardSpiegazione(Spiegazioni.doppioDribling);
            CardSpiegazione("+...");
            ld.LanciaDoppio();
            yield return new WaitWhile(() => ld.risultati[0] == -1 && ld.risultati[1] == -1);
        }
        else
            if (playerÈCompagno(proprietarioPalla))
            {
                ld.risultati[0] = 4;
                ld.risultati[1] = 1;
            }
            else
            {
                ld.risultati[1] = 4;
                ld.risultati[0] = 1;
            }

        void _RubaPalla()
        {
            CardSpiegazione(Spiegazioni.doppioDribling);
            CardSpiegazione(traduzioni.traduci("Palla rubata"));
            if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.destro) == null)
            {
                if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro) != null)
                    proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
                else
                    proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.terzino, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
            }
            else
            {
                if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro) != null)
                    proprietarioPalla = proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), (giocatore.Posizioni)Random.Range(1, 3));
                else
                    proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.destro);
            }                
            PosizionaPalla(proprietarioPalla);
        }

        if (playerÈCompagno(proprietarioPalla))
        {
            print("proprietario di palla io");
            if (ld.risultati[0] > ld.risultati[1])
            {
                if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.destro) != null)
                {
                    if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro) != null)
                        yield return new WaitForSeconds(3);
                    print("primo dribling riuscito");
                    CardSpiegazione(Spiegazioni.doppioDribling);
                    CardSpiegazione("+ 2...");
                    ld.LanciaDoppio();
                    yield return new WaitWhile(() => ld.risultati[0] == -1 && ld.risultati[1] == -1);
                }
                else
                {
                    ld.risultati[0] = 4;
                    ld.risultati[1] = 1;
                }

                pannello.gameObject.SetActive(false);
                if (ld.risultati[0] > ld.risultati[1])
                {
                    print("secondo driblig riuscito");
                    aereo = false;
                    AttivaTiroCross();
                }
                else if (ld.risultati[0] < ld.risultati[1])
                {
                    print("palla rubata secondo dribbling");
                    _RubaPalla();
                    proprietarioPalla.AttivaFrecce();
                    timerC = StartCoroutine(TimerAzione(2));
                }
                else
                {
                    yield return new WaitForSeconds(3);
                    print("rigore");
                    giocatore pl = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.destro);
                    pl.Fallo(true);
                    CardSpiegazione(Spiegazioni.rigore);
                    CardSpiegazione("+...");
                    ld.Lancia();
                    yield return new WaitWhile(() => ld.risultato == -1);
                    int ris1 = ld.risultato;
                    CardSpiegazione(Spiegazioni.rigore);
                    if (ld.risultato == 1)
                    {
                        print("rigore fuori");
                        StartCoroutine(Fuori());
                    }
                    else
                    {
                        yield return new WaitForSeconds(3);
                        print("rigore");
                        CardSpiegazione(Spiegazioni.rigore);
                        CardSpiegazione("+...");
                        ld.Lancia();
                        yield return new WaitWhile(() => ld.risultato == -1);
                        if (ris1 == ld.risultato)
                        {
                            print("parata rigore");
                            StartCoroutine(Fuori(true));
                        }
                        else
                        {
                            print("goal rigore");
                            StartCoroutine(Goal());
                        }
                    }
                }
            }
            else if (ld.risultati[0] < ld.risultati[1])
            {
                print("palla rubata primo dribbling");
                _RubaPalla();
                proprietarioPalla.AttivaFrecce();
                driblingEffettuato = true;
                timerC = StartCoroutine(TimerAzione(2));
            }
            else
            {
                print("punizione");
                yield return new WaitForSeconds(3);
                playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro).Fallo(true);
                CardSpiegazione(Spiegazioni.punizione);
                CardSpiegazione("+...");
                ld.Lancia();
                yield return new WaitWhile(() => ld.risultato == -1);
                if (ld.risultato <= 2)
                {
                    print("al terzino punizione");
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("Palla rubata"));
                    proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.terzino, playersContrari(proprietarioPalla), giocatore.Posizioni.destro);
                    PosizionaPalla(proprietarioPalla);
                    proprietarioPalla.AttivaFrecce();
                    timerC = StartCoroutine(TimerAzione(2));
                }
                else if (ld.risultato == 3 || ld.risultato == 4)
                {
                    print("al difensore punizione");
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("Palla rubata"));
                    proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
                    giocatore g = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
                    if (g == null)
                        proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.terzino, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
                    else
                        g = proprietarioPalla;
                    PosizionaPalla(proprietarioPalla);
                    proprietarioPalla.AttivaFrecce();
                    timerC = StartCoroutine(TimerAzione(2));
                }
                else if (ld.risultato == 6)
                {
                    print("in porta punizione");
                    yield return new WaitForSeconds(3);
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("In porta"));
                    ld.Lancia();
                    yield return new WaitWhile(() => ld.risultato == -1);
                    if (ld.risultato == 6)
                    {
                        print("punizione parata");
                        StartCoroutine(Fuori(true));
                        PosizionaPalla(proprietarioPalla);
                    }
                    else
                    {
                        print("goal punizione");
                        StartCoroutine(Goal());
                    }
                }
                else
                {
                    print("punizione 5");
                    yield return new WaitWhile(() => ld.risultato != -1);
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("In porta"));
                    ld.Lancia();
                    yield return new WaitForSeconds(2);
                    yield return new WaitWhile(() => ld.risultato == -1);
                    if (ld.risultato == 5)
                    {
                        print("parata punizione 5");
                        StartCoroutine(Fuori(true));
                    }
                    else if (ld.risultato == 4 || ld.risultato == 6)
                    {
                        print("calcio d'angolo punizione 5");
                        StartCoroutine(CalcioDangolo());
                    }
                    else
                    {
                        print("goal punizione 5");
                        StartCoroutine(Goal());
                    }
                }
            }
        }
        else
        {
            print("palla all'avversario");
            if (ld.risultati[0] < ld.risultati[1])
            {
                if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.destro) != null)
                {
                    if (playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro) != null)
                        yield return new WaitForSeconds(3);
                    print("primo dribling riuscito");
                    CardSpiegazione(Spiegazioni.doppioDribling);
                    CardSpiegazione("+ 2...");
                    ld.LanciaDoppio();
                    yield return new WaitWhile(() => ld.risultati[0] == -1 && ld.risultati[1] == -1);
                }
                else
                {
                    ld.risultati[1] = 4;
                    ld.risultati[0] = 1;
                }
                pannello.gameObject.SetActive(false);

                if (ld.risultati[0] < ld.risultati[1])
                {
                    print("secondo driblig riuscito avversario");
                    aereoNemico = false;
                    AttivaTiroCross();
                }
                else if (ld.risultati[0] > ld.risultati[1])
                {
                    print("palla rubata secondo dribbling");
                    _RubaPalla();
                    proprietarioPalla.AttivaFrecce();
                    timerC = StartCoroutine(TimerAzione(2));
                }
                else
                {
                    yield return new WaitForSeconds(3);
                    print("rigore");
                    playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.destro).Fallo(true);
                    CardSpiegazione(Spiegazioni.rigore);
                    CardSpiegazione("+...");
                    ld.Lancia();
                    yield return new WaitWhile(() => ld.risultato == -1);
                    int ris1 = ld.risultato;
                    CardSpiegazione(Spiegazioni.rigore);
                    if (ld.risultato == 1)
                    {
                        print("rigore fuori");
                        StartCoroutine(Fuori());
                    }
                    else
                    {
                        yield return new WaitForSeconds(3);
                        print("rigore");
                        CardSpiegazione(Spiegazioni.rigore);
                        CardSpiegazione("+...");
                        ld.Lancia();
                        yield return new WaitWhile(() => ld.risultato == -1);
                        if (ris1 == ld.risultato)
                        {
                            print("parata rigore");
                            StartCoroutine(Fuori(true));
                        }
                        else
                        {
                            print("goal rigore");
                            StartCoroutine(Goal());
                        }
                    }
                }
            }
            else if (ld.risultati[0] > ld.risultati[1])
            {
                print("palla rubata primo dribbling");
                _RubaPalla();
                proprietarioPalla.AttivaFrecce();
                driblingNemicoEffettuato = true;
                timerC = StartCoroutine(TimerAzione(2));
            }
            else
            {
                print("punizione");
                yield return new WaitForSeconds(3);
                playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro).Fallo(true);
                CardSpiegazione(Spiegazioni.punizione);
                CardSpiegazione("+...");
                ld.Lancia();
                yield return new WaitWhile(() => ld.risultato == -1);
                if (ld.risultato <= 2)
                {
                    print("al terzino punizione");
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("Palla rubata"));
                    proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.terzino, playersContrari(proprietarioPalla), giocatore.Posizioni.destro);
                    PosizionaPalla(proprietarioPalla);
                    proprietarioPalla.AttivaFrecce();
                    timerC = StartCoroutine(TimerAzione(2));
                }
                else if (ld.risultato == 3 || ld.risultato == 4)
                {
                    print("al difensore punizione");
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("Palla rubata"));
                    giocatore g = playerDestinatarioSpecifico(giocatore.Ruoli.difensore, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
                    if (g == null)
                        proprietarioPalla = playerDestinatarioSpecifico(giocatore.Ruoli.terzino, playersContrari(proprietarioPalla), giocatore.Posizioni.sinistro);
                    else
                        g = proprietarioPalla;
                    PosizionaPalla(proprietarioPalla);
                    proprietarioPalla.AttivaFrecce();
                    timerC = StartCoroutine(TimerAzione(2));
                }
                else if (ld.risultato == 6)
                {
                    print("in porta punizione");
                    yield return new WaitForSeconds(3);
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("In porta"));
                    ld.Lancia();
                    yield return new WaitWhile(() => ld.risultato == -1);
                    if (ld.risultato == 6)
                    {
                        print("punizione parata");
                        StartCoroutine(Fuori(true));
                        PosizionaPalla(proprietarioPalla);
                    }
                    else
                    {
                        print("goal punizione");
                        StartCoroutine(Goal());
                    }
                }
                else
                {
                    print("punizione 5");
                    yield return new WaitWhile(() => ld.risultato != -1);
                    CardSpiegazione(Spiegazioni.punizione);
                    CardSpiegazione(traduzioni.traduci("In porta"));
                    ld.Lancia();
                    yield return new WaitForSeconds(2);
                    yield return new WaitWhile(() => ld.risultato == -1);
                    if (ld.risultato == 5)
                    {
                        print("parata punizione 5");
                        StartCoroutine(Fuori(true));
                    }
                    else if (ld.risultato == 4 || ld.risultato == 6)
                    {
                        print("calcio d'angolo punizione 5");
                        StartCoroutine(CalcioDangolo());
                    }
                    else
                    {
                        print("goal punizione 5");
                        StartCoroutine(Goal());
                    }
                }
            }
        }
    }

    public IEnumerator TimerAzione(float aspetta = 0, bool _scelta = false)
    {
        ld.eventSystem.enabled = true;
        pannello.gameObject.SetActive(false);
        if (partitaConclusa)
        {
            FinePartita();
            yield break;
        }
        if (playerÈCompagno(proprietarioPalla))
        {
            timerAttivo = true;
            yield return new WaitForSeconds(aspetta);
            timerIm.parent.gameObject.SetActive(true);
            //string st = traduzioni.traduci("Scegli entro") + ": 0";
            int secondi = 4, millisecondi = 0;
            while (secondi > 0)
            {
                yield return new WaitForSeconds(.01f);
                millisecondi--;
                if (millisecondi < 0)
                {
                    secondi--;
                    millisecondi = 59;
                }
                /*
                string s = ":";
                if (millisecondi < 10)
                    s = ":0";
                
                timerT.text = secondi.ToString() + s + millisecondi.ToString();*/
                timerIm.localScale = new Vector3((float)(secondi * 60 + millisecondi) / 240, 1, 1);
            }
            timerIm.parent.gameObject.SetActive(false);
            timerAttivo = false;
        }
        else
            yield return new WaitForSeconds(aspetta);
        print("finito" + Time.time);
        if (_scelta)
        {
            if (playerÈCompagno(proprietarioPalla))
                TiroDopoCross(10);
            else
                TiroDopoCross(Random.Range(0, 6));
        }
        else
        {
            if (playerÈCompagno(proprietarioPalla))
            {
                proprietarioPalla = playerPiùVicino(proprietarioPalla.transform.position, playersContrari(proprietarioPalla));
                PosizionaPalla(proprietarioPalla);
                proprietarioPalla.AttivaFrecce();
                driblingNemicoEffettuato = true;
                timerC = StartCoroutine(TimerAzione());
            }
            else
                AttivaAzione(scegliFrecciaCasuale(proprietarioPalla));
        }
    }

    public IEnumerator TimerPartita()
    {
        int minuti = 7, secondi = 0;
        for (; ; )
        {
            yield return new WaitForSeconds(1);
            secondi--;
            if (secondi < 0)
            {
                minuti--;
                secondi = 59;
            }

            if (minuti < 0)
                break;

            string s = ":";
            if (secondi < 10)
                s = ":0";

            timerPartitaT.text = "0" + minuti.ToString() + s + secondi.ToString();
        }
        partitaConclusa = true;
    }

    void ProprietarioPallaAggiornato()
    {
        if (!playerÈCompagno(proprietarioPalla))
        {
            ruoloAvvT.text = traduzioni.traduci("Attaccante");
            ruoloMioT.text = traduzioni.traduci("Difensore");
            return;
        }
        ruoloMioT.text = traduzioni.traduci("Attaccante");
        ruoloAvvT.text = traduzioni.traduci("Difensore");
    }

    public void FinePartita()
    {
        StopAllCoroutines();
        titoloFineT.transform.parent.gameObject.SetActive(true);
        fineAnim.SetTrigger("en");
        if (goal > goalAvver)
        {
            titoloFineT.text = traduzioni.traduci("Hai vinto!!!");
            statoMiaSquadraFineT.text = traduzioni.traduci("Vincitore");
            statoAvverSquadraFineT.text = traduzioni.traduci("Perdente");
            PlayerPrefs.SetInt("punti", PlayerPrefs.HasKey("punti") ? PlayerPrefs.GetInt("punti") + 3 : 3);
        }
        else if (goal < goalAvver)
        {
            titoloFineT.text = traduzioni.traduci("Hai perso");
            statoMiaSquadraFineT.text = traduzioni.traduci("Perdente");
            statoAvverSquadraFineT.text = traduzioni.traduci("Vincitore");
        }
        else
        {
            titoloFineT.text = traduzioni.traduci("Parità");
            statoMiaSquadraFineT.text = traduzioni.traduci("Parità");
            statoAvverSquadraFineT.text = traduzioni.traduci("Parità");
            PlayerPrefs.SetInt("punti", PlayerPrefs.HasKey("punti") ? PlayerPrefs.GetInt("punti") + 1 : 1);
        }

        nomeSquadraMiaFineT.text = miaSquadra.nome;
        nomeSquadraAvverFineT.text = squadraNemica.nome;
        goalMieiFineT.text = goal.ToString();
        goalAvverFineT.text = goalAvver.ToString();
    }

    public void TornaAlMenù()
    {
        nomeSquadra = nomeSquadraNemica = "";
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class Squadra
{
    public string nome;
    public Sprite maglia;
    public AudioClip coro;
}