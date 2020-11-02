using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected GameManager gameManager;
    protected virtual void Awake() {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnStateChange += GameManager_OnStateChange;
    }

    private void OnDisable()
    {
        gameManager.OnStateChange -= GameManager_OnStateChange;
    }

    protected virtual void GameManager_OnStateChange()
    {
    }

    public abstract void Init(bool isReplying);

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnCollisionEnter(Collision collision) {

    }
}


[System.Serializable]
public struct LiveData
{
    public Vector3 position;
    public Quaternion rotation;

    public LiveData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
    }
}