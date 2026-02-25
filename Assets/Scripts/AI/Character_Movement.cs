using UnityEngine;
using UnityEngine.AI;

public class Character_Movement : MonoBehaviour
{
    public Unit_AI _parentAI;
    private NavMeshAgent _agent;
    public Transform _targetPosition;
    public bool _moveEnabled;
    public Animator _animator;

    [Header("Death")]
    public bool isDead;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void ToggleMovement()
    {
        _moveEnabled = !_moveEnabled;
    }

    private void Update()
    {
        if (isDead) return;

        UpdateMovement();
    }

    public void UpdateMovement()
    {
        if (_moveEnabled == true)
        {
            _agent.SetDestination(_targetPosition.transform.position);
            if (_animator != null) _animator.SetFloat("Speed", 1f);
        }
        else
        {
            if (_animator != null) _animator.SetFloat("Speed", 0f);
        }

        //if (_moveEnabled && transform.position == _targetPosition.position && !_agent.pathPending) //&& _agent.hasPath != true
        if (_moveEnabled &&_agent.hasPath != true && !_agent.pathPending) //&& _agent.hasPath != true
            {
            Debug.Log(gameObject.name + " has arrived.");
            _moveEnabled = false;
        }
    }

    public void KillCharacter()
    {
        if (_moveEnabled) _moveEnabled = false;
        if (_agent.isActiveAndEnabled) _agent.isStopped = true;
        _agent.enabled = false;

        gameObject.SetActive(false);
    }
}
