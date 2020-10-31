using UnityEngine;
using System.Linq;
using Rnd = UnityEngine.Random;

public class trickOrTreatScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombModule bombModule;
    public Material[] costumes;
    public Renderer costumeRender;
    public Renderer closeDoorRender;
    public Renderer openDoorRender;

    private int _costumes;
    private int _stages;
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
        moduleId = moduleIdCounter++;
        bombModule.OnActivate += Activate;
        StartPart2();
        PickCostume();
        door = true;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (door == true)
        {
            closeDoor.gameObject.SetActive(true);
            openDoor.gameObject.SetActive(false);
        }
        else
        {
            closeDoor.gameObject.SetActive(false);
            openDoor.gameObject.SetActive(true);
            Debug.LogFormat("[Totally Accurate Minecraft Simulator #{0}] Costume is {1}. {2}", moduleId, costumes[_costumeIndex], _stages);
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

    void BugFix()
    {
        door = true;
    }

    void StartPart2()
    {
        _stagesSolve = Rnd.Range(10, 20);
        audio = GetComponent<KMAudio>();
        trick.OnInteract += delegate { Trick(); return true; };
        treat.OnInteract += delegate { Treat(); return true; };
        table.OnInteract += delegate { Table(); return true; };
        doorknob.OnInteract += delegate { Doorknob(); return false; };
        PickCostume();

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

    void Trick()
    {
        if (_costumeIndex == 11 )
        {
            _stages = (_stages + 1);
            door = true;
        }
        else
        {
            bombModule.HandleStrike();
            door = true;
        }
    }

    void Treat()
    {
        if (_costumeIndex <= 10)
        {
            _stages = (_stages + 1);
            door = true;
        }
        else
        {
            bombModule.HandleStrike();
            door = true;
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
        }
    }

    void Doorknob()
    {
        door = false;
        Update();
        trick.OnInteract += delegate { Trick(); return false; };
        treat.OnInteract += delegate { Treat(); return false; };
        table.OnInteract += delegate { Table(); return false; };
        BugFix();
    }

    
}
