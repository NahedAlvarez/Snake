using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

     enum Direction
     {
            up,
            down,
            right,
            left
     }

    Direction dir;
    public float frameRate = 0.2f;
    public float step = 0.16f;


    public Vector2 horizontalRange;
    public Vector2 verticalRange;

    public List<Transform> Tail = new List<Transform>();

    public GameObject tailPrefab;

    private void Start()
    {
        InvokeRepeating("Move",frameRate,frameRate);
        BaseFireOk();
    }

    void BaseFireOk()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("Connected FireBase");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    void Move()
    {
        lastPos = transform.position;
        Vector3 nextPos = Vector3.zero;
        if (dir == Direction.up)
           nextPos = Vector3.up;
        else if (dir == Direction.down)
            nextPos = Vector3.down;
        else if (dir == Direction.right)
            nextPos = Vector3.right;
        else if (dir == Direction.left)
            nextPos = Vector3.left;
        nextPos *= step;
        transform.position += nextPos;
        MoveTail();
    }

    Vector3 lastPos;
    void MoveTail()
    {
        for (int i = 0; i < Tail.Count;i++)
        {
            Vector3 temp = Tail[i].position;
            Tail[i].position = lastPos;
            lastPos = temp;
        }
    }


    private void Update()
    {

        MoveSnake();

    }


    void MoveSnake()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) &&  dir != Direction.down)
        {
            dir = Direction.up;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Direction.up)
        {
            dir = Direction.down;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Direction.right)
        {
            dir = Direction.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Direction.left)
        {
            dir = Direction.right;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Foot"))
        {
           Tail.Add( Instantiate(tailPrefab,Tail[Tail.Count-1].position,Quaternion.identity).transform);
            col.transform.position = new Vector2(Random.Range(horizontalRange.x,horizontalRange.y),Random.Range(verticalRange.x,verticalRange.y));
        }
        if (col.CompareTag("Block"))
        {
            print("You lose");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }



}
