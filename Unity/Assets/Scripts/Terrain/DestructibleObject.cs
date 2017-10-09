using System.Collections;
using DllTest;
using UnityEngine;


public class DestructibleObject : MonoBehaviour {

    public float health = 20;
    public GameObject explosion;
//    public GameObject destroyedObject;

    void OnTriggerEnter(Collider collider) {
        //Debug.Log("ENTERED!!");
        StartCoroutine(Blink(0.075f,0.2f));
        ProjectileControl projectile = collider.gameObject.GetComponent<ProjectileControl>();
        if (projectile != null) {
            GameObject.Destroy(projectile);
            collider.GetComponent<Renderer>().enabled = false;

            health -= 5;
            if ( health <= 0 )
            {
                GameObject newExplosion = (GameObject)Instantiate(explosion, new Vector3(this.transform.position.x,this.transform.position.y + 1.5f, this.transform.position.z), this.transform.rotation);
                Destroy(newExplosion,1.5f);

                //GameObject destroyedObject = Instantiate(destroyedObject, this.transform.position, this.transform.rotation) as GameObject;
                GameObject.Destroy(gameObject);
            }
        }

    }

    IEnumerator Blink(float blinkTime, float duration)
    {
        while (duration > 0f) {
            duration -= Time.deltaTime;

            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;

            yield return new WaitForSeconds(blinkTime);
        }
        GetComponent<Renderer>().enabled = true;
    }
}