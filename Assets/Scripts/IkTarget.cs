using UnityEngine;

public class IkTarget : MonoBehaviour
{
    public Vector3 offset;
    private Transform _bone;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _bone = _animator.GetBoneTransform(HumanBodyBones.Head);
    }

    private void LateUpdate()
    {
        _bone.LookAt(LevelsManager.instance.fpBody.transform.position);
        _bone.rotation = _bone.rotation * Quaternion.Euler(offset);
    }

   
}
