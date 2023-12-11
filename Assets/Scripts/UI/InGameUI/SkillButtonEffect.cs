using UnityEngine;

public class SkillButtonEffect : MonoBehaviour
{
    public ParticleSystem oneButtonUseParticle;
    public GameObject particle1;
    // Start is called before the first frame update
    public void DieEffectOn()
    {
        var Go = Instantiate(particle1, transform.position,Quaternion.identity);
        //Go.Play();
        Go.transform.SetParent(transform);
        Debug.Log(Go.transform.position);

        //Destroy(Go.gameObject, 1f);
        
    }
}
