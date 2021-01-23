using UnityEngine;
using System.Linq;
using Rnd = UnityEngine.Random;
using System.Collections;
using System;

public class trickOrTreatScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombModule bombModule;
    public Material[] costumes;
    public Renderer costumeRender;
    public Renderer closeDoorRender;
    public Renderer openDoorRender;

    private int _costumes;
    private int _stages;
    private int _dingDong;
    private float _doorDong;
    private float _doorDongGuest;
    private int _running = 1;
    private int _runningGuest = 0;
    private int _stagesSolve;
    private bool isActive;
    private bool door;
    private int _displayedCostume;
    private int _costumeIndex;

    public KMSelectable trick;
    public KMSelectable treat;
    public KMSelectable table;
    public KMSelectable doorknob;
    public GameObject openDoor;
    public GameObject closeDoor;

    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool _isSolved;

    // Use this for initialization
    void Start()
    {
        WaitDoor();
        moduleId = moduleIdCounter++;
        bombModule.OnActivate += Activate;
        StartPart2();
        door = true;
        _doorDong = 30;
        _running = 1;
        _runningGuest = 0;
    }

    void StartPart2()
    {
        _stagesSolve = Rnd.Range(10, 20);
        Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] {1}", moduleId, _stagesSolve);
        audio = GetComponent<KMAudio>();
        trick.OnInteract += delegate { Trick(); return true; };
        treat.OnInteract += delegate { Treat(); return true; };
        table.OnInteract += delegate { Table(); return true; };
        doorknob.OnInteract += delegate { Doorknob(); return false; };
    }

    // Update is called once per frame
    void Update()
    {
        if (_running == 1)
        {
            StartCoroutine (WaitDoor());
            _runningGuest = 0;
        }

        if (_runningGuest == 1)
        {
            StartCoroutine (GuestWait());
            _running = 2;
        }

        if (door == true)
        {
            closeDoor.gameObject.SetActive(true);
            openDoor.gameObject.SetActive(false);
        }
        else
        {
            closeDoor.gameObject.SetActive(false);
            openDoor.gameObject.SetActive(true);
            Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] Costume is {1}. {2}", moduleId, costumes[_costumeIndex], _stages + 1);
        }
        if (_stages >= _stagesSolve)
        {
            bombModule.HandlePass();
        }
    }

    

    void Activate()
    {
        isActive = true;
    }

    public IEnumerator WaitDoor()
    {
        if (_running != 2)
        {
            _running = 0;
            Ping();

            if (_doorDong >= 0)
            {
                _doorDong -= 1;
                Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] {1} Timer", moduleId, _doorDong);
            }
            if (_doorDong == 10)
            {
                Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] Ding Dong", moduleId);
                dingDong();
            }
            if (_doorDong == 0)
            {
                bombModule.HandleStrike();
                door = true;
                _dingDong = 0;
                _doorDong = 30;
            }
            yield return new WaitForSeconds(1.0f);
            _running = 1;
        }
    }

    public IEnumerator GuestWait()
    {

        if (_runningGuest != 2)
        {
            _running = 2;
            _runningGuest = 0;

            if (_doorDongGuest == 5)
            {
                PickCostume();
            }

            if (_doorDongGuest >= 0)
            {
                _doorDongGuest -= 1;
                Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] {1} Guest", moduleId, _doorDong);
            }

            if (_doorDongGuest == 0)
            {
                bombModule.HandleStrike();
                _dingDong = 0;
                _doorDong = 30;
                _doorDongGuest = 5;
            }
            yield return new WaitForSeconds(1.0f);
            _runningGuest = 1;
        }
    }

    void Trick()
    {
        if (_costumeIndex == 11)
        {
            _stages = (_stages + 1);
            door = true;
            _dingDong = 0;
            _doorDong = 30;
            WaitDoor();
        }
        else
        {
            bombModule.HandleStrike();
            door = true;
            _dingDong = 0;
            _doorDong = 30;
            WaitDoor();
        }
    }

    void Treat()
    {
        if (_costumeIndex <= 10)
        {
            _stages = (_stages + 1);
            door = true;
            _dingDong = 0;
            _doorDong = 30;
            WaitDoor();
        }
        else
        {
            bombModule.HandleStrike();
            door = true;
            _dingDong = 0;
            _doorDong = 30;
            WaitDoor();
        }
    }

    void Table()
    {
        if (_costumeIndex == 12)
        {
            bombModule.HandlePass();
            door = true;
        }
        else
        {
            bombModule.HandleStrike();
            door = true;
            _dingDong = 0;
            _doorDong = 30;
            WaitDoor();
        }
    }

    void Doorknob()
    {
        if (_dingDong == 1)
        {
            door = false;
            _doorDongGuest = 5;
            trick.OnInteract += delegate { Trick(); return false; };
            treat.OnInteract += delegate { Treat(); return false; };
            table.OnInteract += delegate { Table(); return false; };
        }
        else
        {
            bombModule.HandleStrike();
        }
    }

    void dingDong()
    {
        _dingDong = 1;
        audio.PlaySoundAtTransform("doorbell", transform);
        Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] Ding Dong", moduleId);
    }

    void PickCostume()
    {
        if (_stages >= _stagesSolve)
        {

        }
        else
        {
            _costumeIndex = Rnd.Range(0, 13);
            costumeRender.material = costumes[_costumeIndex];
            Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] Costume is {1}. {2}", moduleId, costumes[_costumeIndex], _stages);
        }
    }
    void Ping()
    {
        Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] Ping", moduleId);
    }

    
}
