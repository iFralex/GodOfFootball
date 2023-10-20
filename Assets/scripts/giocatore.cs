using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giocatore : MonoBehaviour
{
    public enum Ruoli { portiere, difensore, terzino, centrocampista, trequartista, punta }
    public enum Posizioni { nulla, destro, sinistro };
    //public enum Squadre { bianconeri, roma};
    public Ruoli ruolo;
    public Posizioni posizione;
    public Squadra squadra;
    public azioniFreccia[] frecceDisponibili;
    public managerOffline mo;
    public managerOnline mon;
    public int falli;

    public void AttivaFrecce()
    {
        void DisattivaFrecce(List<azioniFreccia> lf1, List<azioniFreccia> lf2)
        {
            for (int o = lf1.Count - 1; o >= 0; o--)
                if (lf1[o] != null)
                    lf1[o].gameObject.SetActive(false);
                else
                    lf1.RemoveAt(o);

            for (int o = lf2.Count - 1; o >= 0; o--)
                if (lf2[o] != null)
                    lf2[o].gameObject.SetActive(false);
                else
                    lf2.RemoveAt(o);
        }

        if (mo != null)
            DisattivaFrecce(mo.listaFrecce, mo.listaFrecceAvver);
        else
            DisattivaFrecce(mon.listaFrecce, mon.listaFrecceAvver);

        for (int i = 0; i < frecceDisponibili.Length; i++)
                frecceDisponibili[i].gameObject.SetActive(true);
    }

    public void Fallo(bool a = false)
    {
        falli++;
        if (falli == 1)
            transform.Find("cartellino").gameObject.SetActive(true);
        else if (a)
        {
            int o = 0;

            if (mo != null)
            {
                if (mo.playerÈCompagno(this))
                {
                    for (int i = 0; i < mo.listaFrecce.Count; i++)
                        for (int u = 0; u < mo.listaFrecce[i].destinatario.Length; u++)
                            if (mo.listaFrecce[i].destinatario[u] == this)
                            {
                                int o1 = 0;
                                giocatore[] s = new giocatore[mo.listaFrecce[i].destinatario.Length - 1];
                                for (int e = 0; e < s.Length; e++)
                                    if (mo.listaFrecce[i].destinatario[e] != this)
                                    {
                                        s[o1] = mo.listaFrecce[i].destinatario[e];
                                        o1++;
                                    }
                                mo.listaFrecce[i].destinatario = s;
                                if (mo.listaFrecce[i].destinatario.Length == 0)
                                {
                                    mo.listaFrecce.Remove(mo.listaFrecce[i]);
                                    Destroy(mo.listaFrecce[i].gameObject);
                                }
                            }

                    for (int i1 = 0; i1 < frecceDisponibili.Length; i1++)
                        for (int i = 0; i < mo.playersCompagni.Length; i++)
                            for (int u = 0; u < mo.playersCompagni[i].frecceDisponibili.Length; u++)
                            {
                                //print(mo.playersCompagni[i].frecceDisponibili[u] + "    " + mo.playersCompagni[i].name);
                                if (mo.playersCompagni[i] != this && mo.playersCompagni[i].frecceDisponibili[u] == frecceDisponibili[i1])
                                {
                                    print(u + "    " + mo.playersCompagni[i].name);
                                    int o1 = 0;
                                    azioniFreccia[] s = new azioniFreccia[mo.playersCompagni[i].frecceDisponibili.Length - 1];
                                    for (int e = 0; e < s.Length + 1; e++)
                                        if (mo.playersCompagni[i].frecceDisponibili[e] != frecceDisponibili[i1])
                                        {
                                            s[o1] = mo.playersCompagni[i].frecceDisponibili[e];
                                            o1++;
                                        }
                                    mo.playersCompagni[i].frecceDisponibili = s;
                                }
                            }

                    giocatore[] g = new giocatore[mo.playersCompagni.Length - 1];
                    for (int i = 0; i < mo.playersCompagni.Length; i++)
                        if (mo.playersCompagni[i] != this)
                        {
                            g[o] = mo.playersCompagni[i];
                            o++;
                        }
                    mo.playersCompagni = g;
                }
                else
                {
                    for (int i = 0; i < mo.listaFrecceAvver.Count; i++)
                        for (int u = 0; u < mo.listaFrecceAvver[i].destinatario.Length; u++)
                            if (mo.listaFrecceAvver[i].destinatario[u] == this)
                            {
                                int o1 = 0;
                                giocatore[] s = new giocatore[mo.listaFrecce[i].destinatario.Length - 1];
                                for (int e = 0; e < s.Length; e++)
                                    if (mo.listaFrecceAvver[i].destinatario[e] != this)
                                    {
                                        s[o1] = mo.listaFrecceAvver[i].destinatario[e];
                                        o1++;
                                    }
                                mo.listaFrecceAvver[i].destinatario = s;
                                if (mo.listaFrecceAvver[i].destinatario.Length == 0)
                                {
                                    mo.listaFrecceAvver.Remove(mo.listaFrecceAvver[i]);
                                }
                            }

                    for (int i1 = 0; i1 < frecceDisponibili.Length; i1++)
                        for (int i = 0; i < mo.playersAvver.Length; i++)
                            for (int u = 0; u < mo.playersAvver[i].frecceDisponibili.Length; u++)
                            {
                                //print(mo.playersAvver[i].frecceDisponibili[u] + "    " + mo.playersAvver[i].name);
                                if (mo.playersAvver[i] != this && mo.playersAvver[i].frecceDisponibili[u] == frecceDisponibili[i1])
                                {
                                    print(u + "    " + mo.playersAvver[i].name);
                                    int o1 = 0;
                                    azioniFreccia[] s = new azioniFreccia[mo.playersAvver[i].frecceDisponibili.Length - 1];
                                    for (int e = 0; e < s.Length + 1; e++)
                                        if (mo.playersAvver[i].frecceDisponibili[e] != frecceDisponibili[i1])
                                        {
                                            s[o1] = mo.playersAvver[i].frecceDisponibili[e];
                                            o1++;
                                        }
                                    mo.playersAvver[i].frecceDisponibili = s;
                                }
                            }

                    giocatore[] g = new giocatore[mo.playersAvver.Length - 1];
                    print("dentro");
                    for (int i = 0; i < mo.playersAvver.Length; i++)
                        if (mo.playersAvver[i] != this)
                        {
                            g[o] = mo.playersAvver[i];
                            o++;
                        }
                    mo.playersAvver = g;
                }
            }
            else
            {
                if (mon.playerÈCompagno(this))
                {
                    for (int i = 0; i < mon.listaFrecce.Count; i++)
                        for (int u = 0; u < mon.listaFrecce[i].destinatario.Length; u++)
                            if (mon.listaFrecce[i].destinatario[u] == this)
                            {
                                int o1 = 0;
                                giocatore[] s = new giocatore[mon.listaFrecce[i].destinatario.Length - 1];
                                for (int e = 0; e < s.Length; e++)
                                    if (mon.listaFrecce[i].destinatario[e] != this)
                                    {
                                        s[o1] = mon.listaFrecce[i].destinatario[e];
                                        o1++;
                                    }
                                mon.listaFrecce[i].destinatario = s;
                                if (mon.listaFrecce[i].destinatario.Length == 0)
                                {
                                    mon.listaFrecce.Remove(mon.listaFrecce[i]);
                                    Destroy(mon.listaFrecce[i].gameObject);
                                }
                            }

                    for (int i1 = 0; i1 < frecceDisponibili.Length; i1++)
                        for (int i = 0; i < mon.playersCompagni.Length; i++)
                            for (int u = 0; u < mon.playersCompagni[i].frecceDisponibili.Length; u++)
                            {
                                //print(mo.playersCompagni[i].frecceDisponibili[u] + "    " + mo.playersCompagni[i].name);
                                if (mon.playersCompagni[i] != this && mon.playersCompagni[i].frecceDisponibili[u] == frecceDisponibili[i1])
                                {
                                    print(u + "    " + mon.playersCompagni[i].name);
                                    int o1 = 0;
                                    azioniFreccia[] s = new azioniFreccia[mon.playersCompagni[i].frecceDisponibili.Length - 1];
                                    for (int e = 0; e < s.Length + 1; e++)
                                        if (mon.playersCompagni[i].frecceDisponibili[e] != frecceDisponibili[i1])
                                        {
                                            s[o1] = mon.playersCompagni[i].frecceDisponibili[e];
                                            o1++;
                                        }
                                    mon.playersCompagni[i].frecceDisponibili = s;
                                }
                            }

                    giocatore[] g = new giocatore[mon.playersCompagni.Length - 1];
                    for (int i = 0; i < mon.playersCompagni.Length; i++)
                        if (mon.playersCompagni[i] != this)
                        {
                            g[o] = mon.playersCompagni[i];
                            o++;
                        }
                    mon.playersCompagni = g;
                }
                else
                {
                    for (int i = 0; i < mon.listaFrecceAvver.Count; i++)
                        for (int u = 0; u < mon.listaFrecceAvver[i].destinatario.Length; u++)
                            if (mon.listaFrecceAvver[i].destinatario[u] == this)
                            {
                                int o1 = 0;
                                giocatore[] s = new giocatore[mon.listaFrecce[i].destinatario.Length - 1];
                                for (int e = 0; e < s.Length; e++)
                                    if (mon.listaFrecceAvver[i].destinatario[e] != this)
                                    {
                                        s[o1] = mon.listaFrecceAvver[i].destinatario[e];
                                        o1++;
                                    }
                                mon.listaFrecceAvver[i].destinatario = s;
                                if (mon.listaFrecceAvver[i].destinatario.Length == 0)
                                    mon.listaFrecceAvver.Remove(mon.listaFrecceAvver[i]);
                            }

                    for (int i1 = 0; i1 < frecceDisponibili.Length; i1++)
                        for (int i = 0; i < mon.playersAvver.Length; i++)
                            for (int u = 0; u < mon.playersAvver[i].frecceDisponibili.Length; u++)
                            {
                                //print(mo.playersAvver[i].frecceDisponibili[u] + "    " + mo.playersAvver[i].name);
                                if (mon.playersAvver[i] != this && mon.playersAvver[i].frecceDisponibili[u] == frecceDisponibili[i1])
                                {
                                    print(u + "    " + mon.playersAvver[i].name);
                                    int o1 = 0;
                                    azioniFreccia[] s = new azioniFreccia[mon.playersAvver[i].frecceDisponibili.Length - 1];
                                    for (int e = 0; e < s.Length + 1; e++)
                                        if (mon.playersAvver[i].frecceDisponibili[e] != frecceDisponibili[i1])
                                        {
                                            s[o1] = mon.playersAvver[i].frecceDisponibili[e];
                                            o1++;
                                        }
                                    mon.playersAvver[i].frecceDisponibili = s;
                                }
                            }

                    giocatore[] g = new giocatore[mon.playersAvver.Length - 1];
                    print("dentro");
                    for (int i = 0; i < mon.playersAvver.Length; i++)
                        if (mon.playersAvver[i] != this)
                        {
                            g[o] = mon.playersAvver[i];
                            o++;
                        }
                    mon.playersAvver = g;
                }
            }

            for (int i = 0; i < frecceDisponibili.Length; i++)
                frecceDisponibili[i].gameObject.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}