using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class shield_hit : MonoBehaviour
{
    public GameObject shieldRipples;
    private VisualEffect shieldRippleVFX;

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Bullet")
        {
            var ripples = Instantiate(shieldRipples, transform) as GameObject;
            shieldRippleVFX = ripples.GetComponent<VisualEffect>();
            shieldRippleVFX.SetVector3("SphereCenter", collision.GetContact(0).point);

            Destroy(ripples, 2);
        }
    }





}
