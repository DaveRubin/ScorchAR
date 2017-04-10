using UnityEngine;

public class Explosion : MonoBehaviour {

    float maxLife = 3;
    float life = 3;
    float force = 5;

    // Update is called once per frame
    void Awake() {
        transform.position = Vector3.zero;
    }

    public void SetDamage(float damage,float lifeDuration = 3) {
        maxLife = life = lifeDuration;
        force = damage;
    }

    void Update() {

        life -= Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 5, life/maxLife);

        if (life < 0 ) {
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Get damage according to explosion life remaining
    /// </summary>
    /// <returns></returns>
    public float GetDamage() {
        return (life/maxLife) * force;
    }
}
