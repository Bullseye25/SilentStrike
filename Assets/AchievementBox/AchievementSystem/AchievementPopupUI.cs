using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource source;

    public void Initialize(Achievement achievement)
    {
        descriptionText.text = achievement.description;
        icon.sprite = achievement.icon;
        AchivementComplete();
    }

    public void AchivementComplete()
    {
        if (animator != null)
        {
            animator.Play("Show");
            source.Play();
        }
    }
}
