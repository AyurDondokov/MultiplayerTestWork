using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour, IPunObservable
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.up;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
        if (stream.IsWriting)
        {
            stream.SendNext(direction);
        }
        else
        {
            direction = (Vector2)stream.ReceiveNext();
        }
    }

    public void SetStartValues( Vector2 dir) 
    {
        direction = dir;
    }
 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && !collision.GetComponent<PhotonView>().IsMine) 
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
