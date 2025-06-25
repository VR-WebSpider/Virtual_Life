using Photon.Pun;
using UnityEngine;

public class AddPhotonView : MonoBehaviour
{
    void Awake()
    {
        if (GetComponent<PhotonView>() == null)
        {
            gameObject.AddComponent<PhotonView>();
        }
    }
}
