using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class SendGameResult : MonoBehaviour
{
    public void Send()
    {
        STT.Instance.Send();
    }
}
