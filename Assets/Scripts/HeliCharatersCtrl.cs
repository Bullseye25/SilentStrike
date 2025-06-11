using DG.Tweening;
using UnityEngine;

public class HeliCharatersCtrl : MonoBehaviour
{
    public DOTweenAnimation DOTweenAnimation;
    public GameObject AIChracter;
    public float health=100;
    public GameObject RigCharacter;

    public void ApplyDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                DOTweenAnimation.DOKill();
                this.gameObject.layer = 22;

                HudMenuManager.instance.EnemyKilled();

                Instantiate(RigCharacter, new Vector3(this.transform.position.x+Random.Range(-1f, 1f),this.transform.position.y+1,this.transform.position.z+Random.Range(-1f, 1f)), Quaternion.identity);
                this.gameObject.SetActive(false);

            }
        }

    }
   
    public void EnableAiCharater()
    {
        AIChracter.transform.position = transform.position;
        AIChracter.SetActive(true);
        this.gameObject.SetActive(false);
    }
   
}
