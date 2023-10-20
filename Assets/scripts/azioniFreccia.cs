using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class azioniFreccia : MonoBehaviour
{
    public enum Azione { passaggio, dribling, tiro, cross};
    public Azione tipo;
    public giocatore[] destinatario;
}
