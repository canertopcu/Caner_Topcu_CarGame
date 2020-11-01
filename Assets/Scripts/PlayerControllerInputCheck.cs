using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInputCheck : MonoBehaviour
{

    public List<Command> commandList;
    public float speed = 1;
    public float rotSpeed = 1;
    GameManager gameManager;
    bool isLeftPressed = false;
    bool isRightPressed = false;
    bool isStartMove = false;
    public Transform startTransform;
    public bool isReplay = false;
    public int playIndex = 0;
    Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        commandList = new List<Command>();
        gameManager = GameManager.Instance;
        Init(false); 
    }


    public void Init(bool isReplaying)
    {
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
        isReplay = isReplaying;
        isStartMove = false;
        gameManager.SetState(GameState.Pause);

        playIndex = 0;
        lastPos = startTransform.position;
    }


    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameState.Pause)
        {
            if (Input.anyKey)
            {
                gameManager.SetState(GameState.Play);

                isStartMove = true;
                timer = Time.fixedUnscaledTime;
            }

        }



        if (isStartMove)
        {
            if (!isReplay)
            {
                Debug.DrawLine(transform.position, lastPos, Color.blue, Mathf.Infinity);
                lastPos = transform.position;

                if (Input.GetKeyDown(KeyCode.A))
                {
                    Command c = new Command();
                    c.commandType = CommandType.Forward;
                    c.time = Time.fixedUnscaledTime - timer;
                    commandList.Add(c);

                    isLeftPressed = true;
                    timer = Time.fixedUnscaledTime;
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    isLeftPressed = false;

                    Command c = new Command();
                    c.commandType = CommandType.Left;
                    c.time = Time.fixedUnscaledTime - timer;
                    commandList.Add(c);

                    timer = Time.fixedUnscaledTime;

                }

                if (Input.GetKeyDown(KeyCode.D))
                {

                    Command c = new Command();
                    c.commandType = CommandType.Forward;
                    c.time = Time.fixedUnscaledTime - timer;
                    commandList.Add(c);

                    isRightPressed = true;
                    timer = Time.fixedUnscaledTime;

                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    isRightPressed = false;

                    Command c = new Command();
                    c.commandType = CommandType.Right;
                    c.time = Time.fixedUnscaledTime - timer;
                    commandList.Add(c);

                    timer = Time.fixedUnscaledTime;

                } 
            } 
            else
            {
                Debug.DrawLine(transform.position, lastPos, Color.red, Mathf.Infinity);
                lastPos = transform.position;
                
            }
            
        }
     
        if (isStartMove)
        {
            if (!isReplay)
            {
                if (isLeftPressed && !isRightPressed)
                {
                    transform.Rotate(Vector3.up * -rotSpeed * Time.deltaTime);
                }
                else if (!isLeftPressed && isRightPressed)
                {
                    transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

                }
                else { }
            }
            else {
                float diff = Time.fixedUnscaledTime - timer;
                if (commandList[playIndex].time > diff)
                {
                    if (commandList[playIndex].commandType == CommandType.Left)
                    {
                        transform.Rotate(Vector3.up * -rotSpeed * Time.deltaTime);
                    }
                    else if (commandList[playIndex].commandType == CommandType.Right)
                    {
                        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
                    }
                    else { }
                }
                else
                {
                    if (playIndex < commandList.Count - 1)
                    {
                        playIndex++;
                    }
                    timer = Time.fixedUnscaledTime;
                }
            }
            transform.position += transform.forward * speed * Time.deltaTime;

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EndPos"))
        {
            Debug.LogError("Success");
            if (!isReplay)
            {
                Command c = new Command();

                if (isLeftPressed)
                    c.commandType = CommandType.Left;
                else if (isLeftPressed)
                    c.commandType = CommandType.Right;
                else
                    c.commandType = CommandType.Forward;

                c.time = Time.unscaledTime - timer;
                commandList.Add(c);
            }

            Init(true);

        }
        else
        {
            Debug.LogError("Faill");
            commandList.Clear();
            Init(false);
        }
    }

}



public enum CommandType { Left, Right, Forward }

[System.Serializable]
public class Command
{
    public float time;
    public CommandType commandType;
}