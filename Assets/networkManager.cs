using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Placement;
using GoogleMobileAds.Api;

public class networkManager : Photon.PunBehaviour
{
    public Animator caricamento;
    public UnityEngine.EventSystems.EventSystem evs;
    public UnityEngine.UI.Text causa;
    public InterstitialAdGameObject ads;
    bool finitoAds;

    void Start() => MobileAds.Initialize(a => { });

    public void Connetti()
    {/*
        if (PlayerPrefs.HasKey("punti"))
        {
            int p = PlayerPrefs.GetInt("punti");
            print("p");
            if (p > 2)
                PlayerPrefs.SetInt("punti", p - 3);
            else
            {
                caricamento.transform.parent.gameObject.SetActive(false);
                evs.enabled = true;
                StartCoroutine(Debug("OPS! You do not have enough points to participate. Play singleplayer to earn at least three points for playing multiplayer"));
                return;
            }
        }
        else
        {
            caricamento.transform.parent.gameObject.SetActive(false);
            evs.enabled = true;
            StartCoroutine(Debug("OPS! You do not have enough points to participate. Play singleplayer to earn at least three points for playing multiplayer"));
            return;
        }*/
        PhotonNetwork.ConnectUsingSettings("1");
        caricamento.transform.parent.gameObject.SetActive(true);
        CaricaAd();
    }

    public void CaricaAd()
    {
        ads = MobileAds.Instance.GetAd<InterstitialAdGameObject>("ad");
        ads.LoadAd();
    }

    public void AdCaricato(string s)
    {
        if (ads != null)
            ads.ShowIfLoaded();
        Time.timeScale = 1;
    }

    public void AdChiuso() => finitoAds = true;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("partita", new RoomOptions() { MaxPlayers = 2 }, new TypedLobby() { Name = "lobby", Type = LobbyType.Default });
    }

    public override void OnJoinedRoom()
    {
        //print(managerOnline.nomeSquadra);
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "squadra", managerOnline.nomeSquadra }, { "pronto", false } });
        if (PhotonNetwork.room.PlayerCount == 2)
            StartCoroutine(CaricaScena());
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        caricamento.transform.parent.gameObject.SetActive(false);
        evs.enabled = true;
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        caricamento.transform.parent.gameObject.SetActive(false);
        evs.enabled = true;
        StartCoroutine(Debug((string)codeAndMsg[1]));
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        caricamento.transform.parent.gameObject.SetActive(false);
        evs.enabled = true;
        StartCoroutine(Debug(cause.ToString()));
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        caricamento.transform.parent.gameObject.SetActive(false);
        evs.enabled = true;
        StartCoroutine(Debug((string)codeAndMsg[1]));
    }

    IEnumerator Debug(string s)
    {
        causa.gameObject.SetActive(true);
        causa.text = s;
        yield return new WaitForSeconds(2);
        causa.gameObject.SetActive(false);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        print(managerOnline.nomeSquadra);
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "squadra", managerOnline.nomeSquadra } });
        StartCoroutine(CaricaScena());
    }

    IEnumerator CaricaScena()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => !finitoAds);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}