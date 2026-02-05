using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Movement : MonoBehaviour
{
    [Header("Parameters")]
    public Unit_AI _unitAI;
    public NavMeshAgent _agent;
    public float _moveSpeed;
    [SerializeField] private Transform[] _unitPositions;
    [SerializeField] private List<Character_Movement> _unitCharacters;

    [Header("Current Status")]
    public GameObject _target;
    public float _desiredDistance;
    public bool _isMoving = false;

    [Header("Debugging")]
    public GameObject debug_TestTarget;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        CharacterSetup();
    }

    private void CharacterSetup()
    {
        for (int i = 0; i < _unitCharacters.Count; i++)
        {
            _unitCharacters[i]._targetPosition = _unitPositions[i];
            _unitCharacters[i]._parentAI = _unitAI;
            _unitCharacters[i]._moveEnabled = false;
        }
    }

    private void Update()
    {
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        if (!_isMoving) return;
        if (_agent.pathPending) return;

        if (_agent.remainingDistance <= _desiredDistance) //(movingToDestination == true && Vector3.Distance(transform.position, currentDestination.position) <= desiredDistance)
        {
            //Debug.Log("Unit " + gameObject.transform.parent.name + " stopping at: " + Vector3.Distance(transform.position, _target.transform.position) + " while desired distance is set to: " + _desiredDistance);
            //Debug.Log("Unit " + gameObject.transform.parent.name + " stopping at: " + _agent.remainingDistance + " while desired distance is set to: " + _desiredDistance);
            _agent.isStopped = true;
            _isMoving = false;
        }
    }

    public void Move_Start(GameObject destination, float distance)
    {
        _desiredDistance = distance;
        _target = destination;
        //Debug.Log("Target is " + Vector3.Distance(transform.position, _target.transform.position));
        if (_agent.isStopped == true)
        {
            _agent.isStopped = false;
        }
        //if (Debug.isDebugBuild) Debug.Log("Moving unit " + _unitAI.GetUnitName());
        _agent.SetDestination(_target.transform.position);
        _isMoving = true;
        for (int i = 0; i < _unitCharacters.Count; i++)
        {
            _unitCharacters[i]._moveEnabled = true;
        }
    }

    public void Move_End() //For manually stopping unit.
    {
        _agent.isStopped = true;
        _isMoving = false;
    }

    #region Debugging

    [ContextMenu("Test Movement")]
    public void Debug_MovementStart()
    {
        Vector3 tempVector = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        debug_TestTarget.transform.position = tempVector;
        Move_Start(debug_TestTarget, 0.1f);
    }

    #endregion
}
