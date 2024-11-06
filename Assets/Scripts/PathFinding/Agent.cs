using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace PathFinding
{
    public class Agent : MonoBehaviour
    {
        private PathFinding _pathfinding;
        private ProceduralMazeGeneration.ProceduralMazeGeneration _mazeGeneration;
        private bool _canTrace;
        private bool _isInTrace;
        private Coroutine _traceCoroutine;

        [Inject]
        public void Construct(PathFinding pathfinding,
            ProceduralMazeGeneration.ProceduralMazeGeneration mazeGeneration)
        {
            _pathfinding = pathfinding;
            _mazeGeneration = mazeGeneration;

            _mazeGeneration.OnMazeRegenerated += HandleMazeRegeneration;
        }

        private void OnDestroy()
        {
            if (_mazeGeneration != null)
                _mazeGeneration.OnMazeRegenerated -= HandleMazeRegeneration;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_canTrace)
                    Trace();
            }
        }

        private void HandleMazeRegeneration()
        {
            _canTrace = true;

            if (_isInTrace)
                StopTrace();

            transform.position = _mazeGeneration.Grid[0, 0].transform.position;
        }

        public void Trace()
        {
            _canTrace = false;
            var path = _pathfinding.FindPath(0, 0, _mazeGeneration.Grid.GetLength(0) - 1,
                _mazeGeneration.Grid.GetLength(1) - 1);
            var points = path.Select(p => p.transform).ToList();
            _traceCoroutine = StartCoroutine(Trace(points));
        }

        private void StopTrace()
        {
            StopCoroutine(_traceCoroutine);
            _traceCoroutine = null;
            _isInTrace = false;
        }

        private IEnumerator Trace(List<Transform> points)
        {
            _isInTrace = true;
            foreach (var point in points)
            {
                transform.position = point.position;
                yield return new WaitForSeconds(0.5f);
            }

            _isInTrace = false;
        }
    }
}