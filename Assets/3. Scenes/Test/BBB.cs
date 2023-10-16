using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BBB : MonoBehaviour
{
    Rigidbody2D rig;
    CircleCollider2D col;

    bool isInMillstone;

    LayerMask originLayer;

    private void OnMouseDown()
    {
        if (isInMillstone)
            return;
        transform.parent.GetComponent<AAA>().SelectObject(gameObject);
        Select();
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    public void ChangeLayer(bool isOrigin = false)
    {
        if (isOrigin)
            gameObject.layer = originLayer;
        else
            gameObject.layer = LayerMask.GetMask("MaterialObj");
    }

    public void Select()
    {
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        col.isTrigger = true;
        rig.gravityScale = 0;
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0;
    }

    public void ResetObj(Vector3 pos, Vector3 rot)
    {
        Select();

        Sequence sequence = DOTween.Sequence().SetRecyclable(true).SetAutoKill(false).Pause();

        sequence.Join(transform.DOLocalMove(pos, 0.2f))
            .Join(transform.DORotate(rot, 0.2f));

        sequence.Play();
    }

    public void ReleseObj()
    {
        rig.gravityScale = 3;
        col.isTrigger = false;
        var forceX = Random.Range(-2f, 2f);
        var forceY = Random.Range(-2f, 0f);
        rig.AddForce(new Vector2(forceX, forceY),ForceMode2D.Impulse);
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;     
    }

    public void InMillstone()
    {
        isInMillstone = true;
        Destroy(this);
    }
}
