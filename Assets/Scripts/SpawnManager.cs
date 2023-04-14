using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnManager : MonoBehaviour
    {
        public event System.Action OnBallCreated;
        public event System.Action OnBallDeleted;

        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Color[] _colorOptions; // Any number of desired colors can be selected from the inspector.
        private int _colorOptionsLenght;                // The property is not called every time when choosing random colors

        private Camera _mainCamera;                     // To get mousePosition in world space
        private List<Ball> _balls = new List<Ball>();
        private string _ballName;

        private float _ballRadius;                      // Getting radius, so that the balls do not overlap (This is my extra feature)

        private void Start()
        {
            _mainCamera = Camera.main;
            _ballRadius = _ballPrefab.GetComponent<CircleCollider2D>().radius;  // The knob sprite contains unseen spaces,
                                                                                // so getting the radius over the sprite is misleading.
                                                          // That's why I get radius over collider. No other reason I included collider.

            _colorOptions = new Color[2];       // In this case we only want to have 2 different colors (blue and red)
            _colorOptions[0] = Color.red;       // So that's why I hard coded the indices directly
            _colorOptions[1] = Color.blue;      // Will be determined by a random number of 0 or 1
            _colorOptionsLenght = _colorOptions.Length;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePositionInWorldSpace = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePositionInWorldSpace.z = 0;
                SpawnBallAtMouseAndGiveName(mousePositionInWorldSpace, "Colored Ball");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //TODO: This should remove all red balls, right?
                RemoveAllRedBall();
            }
        }

        public void RemoveAllRedBall()
        {
            // When deleting an element of list it shifts all subsequent elements to the left
            // For this reason I prefer to use backward for loop instead of foreach
            for (int i = _balls.Count - 1; i >= 0; i--)
            {
                Ball currentBall = _balls[i];
                if (currentBall.Color == Color.red)
                {
                    RemoveBall(currentBall);
                }
            }
        }

        public void RemoveBall(Ball ball)
        {
            //TODO: Implement a way to remove a ball from the scene and the list.
            // just deletes the determined ball
            _balls.Remove(ball);
            Destroy(ball.gameObject);

            if (OnBallDeleted != null)
            {
                OnBallDeleted();
            }
        }

        private void SpawnBallAtMouseAndGiveName(Vector3 mousePosition, string name)
        {
            //TODO: Spawn a random color of ball at the position of the mouse click and play spawn sound.
            if (IsAvailablePos(mousePosition))
            {
                _ballName = name;
                InstantiateBall(mousePosition);

                //OPTIONAL: Use events for playing sounds.
                if (OnBallCreated != null)
                {
                    OnBallCreated();
                }
            }
        }

        private Ball InstantiateBall(Vector3 position)
        {
            //GameObject ballGameObject = Instantiate(_ballPrefab, position, Quaternion.identity);
            //Ball ball = ballGameObject.GetComponent<Ball>(); //TODO: What's a better way to do this?

            int randomNumber = Random.Range(0, _colorOptionsLenght);
            Color ballColor = _colorOptions[randomNumber];

            Ball newBall = Instantiate(_ballPrefab, position, Quaternion.identity);
            newBall.SetName(_ballName);
            newBall.Color = ballColor;
            _balls.Add(newBall);

            return newBall;
        }

        // Checks whether the ball to be created is overlaps other balls 
        public bool IsAvailablePos(Vector3 newBallPos)
        {
            foreach (Ball b in _balls)
            {
                // I've used to SqrMagnitude not Distance func. because square root operation is fairly expensive.
                // We don't need the actual distance.
                // I did squaredDistance on the left side of the inequality so this is why squaring the sum of the radius of the two balls. 
                if (Vector3.SqrMagnitude(b.transform.position - newBallPos) < Mathf.Pow(_ballRadius + _ballRadius, 2))
                {
                    return false;
                }

                //if (Vector3.Distance(b.transform.position ,newBallPos) < _ballRadius + _ballRadius)
                //{
                //    return false;
                //}
            }
            return true;
        }
    }
}