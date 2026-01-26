using UnityEngine;
using UnityEngine.AI;

public class Character_Movement : MonoBehaviour
{
    public Unit_AI _parentAI;
    private NavMeshAgent _agent;
    public Transform _targetPosition;
    public bool _moveEnabled;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void ToggleMovement()
    {
        _moveEnabled = !_moveEnabled;
    }

    private void Update()
    {
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        if (_moveEnabled == true)
        {
            _agent.SetDestination(_targetPosition.transform.position);
        }

        if (_moveEnabled  && transform.position == _targetPosition.position && !_agent.pathPending) //&& _agent.hasPath != true
        {
            Debug.Log(gameObject.name + " has arrived.");
            _moveEnabled = false;
        }
    }
}
