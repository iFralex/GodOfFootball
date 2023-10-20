using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class traduciUI : MonoBehaviour
{
    void Awake() => GetComponent<Text>().text = traduzioni.traduci(GetComponent<Text>().text);
}

static public class traduzioni// : MonoBehaviour
{
    public static Dictionary<string, string> traduzione = new Dictionary<string, string>
    {
        { "Palla rubata", "Stole ball"},
        { "In porta", "On goal"},
        { "Parata", "Safe"},
        { "Rinvio 1", "Return 1"},
        { "Rinvio 2", "Return 2"},
        { "Deviata", "Deviate"},
        { "Passaggio", "Passage"},
        { "Cross", "Cross"},
        { "Ammonizione Giallo", "Yellow card"},
        { "Ammonizione Rosso", "Red card"},
        { "Rigore", "Penalty kick"},
        { "Tiro", "Shot"},
        { "Fuori", "Outside"},
        { "Calcio d'angolo", "Corner"},
        { "Dribbling", "Dribbling"},
        { "Doppio dribbling", "Double dribbling"},
        { "Punizione", "Free kick"},
        { "Colpo di testa", "Head Shot"},
        { "Scegli entro", "Choose within"},
        { "Attaccante", "Striker"},
        { "Difensore", "Defender"},
        { "Testa", "Heads"},
        { "Croce", "Tails"},
        { "Vinto", "Win"},
        { "Perso", "Lose"},
        { "Scegli un lato della porta", "Choose one side of the door"},
        { "Scegli testa", "Choose heads"},
        { "Scegli croce", "Choose tails"},
        { "Mio risultato", "My result"},
        { "Risultato avversario", "Opponent's result"},
        { "Hai vinto!!!", "You won!!!"},
        { "Hai perso", "You lost"},
        { "Parità", "Parity"},
        { "Vincitore", "Winner"},
        { "Perdente", "Loser"},
        { "Fine", "End"},
        { "Nome utente", "User name:"},
        { "Squadra:", "Team:"},
        { "Punti:", "Points:"},
        { "Vincite:", "Won:"},
        { "Pareggi:", "Draws:"},
        { "GIOCA:", "PLAY:"},
        { "Prossimamente tornei online gratuiti con fantastici premi", "Coming soon free online tournaments great prizes"},
        { "OSP!\nNon hai punti sufficienti per partecipare.\nGioca in singleplayer per guadagnare almeno 3 punti per giocare in multiplayer", "OPS!\nYou do not have enough points to participate.\nPlay singleplayer to earn at least 3 points for playing multiplayer." }
    };

    public static string traduci(string s)
    {
        if (Application.systemLanguage == SystemLanguage.English)
            return traduzione[s];
        else
            return s;
    }
}