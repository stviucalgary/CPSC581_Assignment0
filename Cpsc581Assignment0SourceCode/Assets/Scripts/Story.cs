using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour {
    public float MovementValue = 1;
    public GameObject TextBoxGameObject;
    public GameObject TextGameObject;
    public GameObject CharacterTalkingGameObject;
    public GameObject CombatMenuGameObject;
    public GameObject PokemonMusicGameObject;
    public GameObject XGonMusicGameObject;
    public GameObject BazookaGameObject;
    public GameObject GunGameObject;
    public GameObject PickUpGameObject;


    private GUIText Text;
    private GUITexture TextBox;
    private GUITexture CharacterTalking;
    private GUITexture CombatMenu;
    private AudioSource PokemonMusic;
    private AudioSource XGonMusic;
    private TextTyper TextTyper;
    private PickupSpawner PickUp;

    public bool HasPickUpCrate;
    public bool VillianDead;

    private int stage;
    private int SBStextIndex;
    private int BStextIndex;
    private int FStextIndex;
    private float timeElapsed;

    private string[] SBSDialog = new string[7];
    private string[] BSDialog = new string[7];
    private string[] FSDialog = new string[7];



    // Use this for initialization
    void Start () {
        //int private vars
        timeElapsed = 0.0f;
        stage = 0;
        SBStextIndex = 0;
        BStextIndex = 0;


        //Get all needed compents
        Text = TextGameObject.GetComponent<GUIText>();
        TextTyper = TextGameObject.GetComponent<TextTyper>();
        TextBox = TextBoxGameObject.GetComponent<GUITexture>();
        CharacterTalking = CharacterTalkingGameObject.GetComponent<GUITexture>();
        CombatMenu = CombatMenuGameObject.GetComponent<GUITexture>();
        PickUp = PickUpGameObject.GetComponent<PickupSpawner>();


        PokemonMusic = PokemonMusicGameObject.GetComponent<AudioSource>();
        XGonMusic = XGonMusicGameObject.GetComponent<AudioSource>();


        //Turn off the text ui
        Text.enabled = false;
        TextBox.enabled = false;
        CharacterTalking.enabled = false;

        //init dialog
        SBSDialog[0] = "Ha! I have finally found you Mr. PotaTeo";
        SBSDialog[1] = "I am Captain Indecisive-SlowTalker";
        SBSDialog[2] = "And I have decided to take over this world ";
        SBSDialog[3] = "And for no reason at all";
        SBSDialog[4] = "I have chosen you to be my mortal enemy";
        SBSDialog[5] = "We will fight in turn based combat";
        SBSDialog[6] = "To see what will happen to your planet!";

        BSDialog[0] = "My turn!";
        BSDialog[1] = "Hmmmm.... what should I do?";
        BSDialog[2] = "I could do this move";
        BSDialog[3] = "Or this one!";
        BSDialog[4] = "Nah";
        BSDialog[5] = "What about this!";
        BSDialog[6] = "Ugh...... too many choices";

        FSDialog[0] = "What are you doing?";
        FSDialog[1] = "You can't move, it is my turn!";
        FSDialog[2] = "NO STOP!";
        FSDialog[3] = "Come on, that is not fair";
        FSDialog[4] = "Seriously, that's cheating";
        FSDialog[5] = "Staaaahpppppppppppp";
        FSDialog[6] = "Come on, wait your turn";

        //set gameobjects to be inactive
        BazookaGameObject.gameObject.SetActive(false);
        GunGameObject.gameObject.SetActive(false);

        //init public variables
        HasPickUpCrate = false;
        VillianDead = false;
    }

    // Update is called once per frame
    void Update () {
        if (stage == 2 && Input.GetButtonDown("Fire1"))
        {
            stage++;
            timeElapsed = 0f;
            PickUp.DeliverPickup();
        }

        //Main switch statements that controls which stage to play
        timeElapsed += Time.deltaTime;
        switch (stage)
        {
            case 0:
                //Beginning movement sequence
                if ((2.5f - timeElapsed) < 0)
                {
                    MovementValue = 0;
                    PokemonMusic.enabled = true;
                    stage++;
                }
                break;
            case 1:
                StartBattleSequence();
                break;
            case 2:
                BattleSequence();
                break;
            case 3:
                FinalSequence();
                break;
            default:
                break;
        }


    }

    private void StartBattleSequence()
    {
        //if all dialog is done go to next stage
        if (SBStextIndex > 7)
        {
            stage++;
            return;
        }
        //turn on text ui
        Text.enabled = true;
        TextBox.enabled = true;
        CharacterTalking.enabled = true;

        if (!TextTyper.IsTyping)
        {
            //increase dialog index
            SBStextIndex++;

            //makes sure dialog is within dialog index range
            if (SBSDialog.Length < SBStextIndex)
                return;

            //type dialog
            TextTyper.DisplayText(SBSDialog[SBStextIndex-1]);
        }
    }

    private void BattleSequence()
    {
        //put the combat menu on
        CombatMenu.enabled = true;

        //loop dialog
        if (BStextIndex == 7)
        {
            BStextIndex = 1;
        }

        if (!TextTyper.IsTyping)
        {
            //increase dialog index
            BStextIndex++;

            //makes sure dialog is within dialog index range
            if (BSDialog.Length < BStextIndex)
                return;

            //type dialog
            TextTyper.DisplayText(BSDialog[BStextIndex - 1]);
        }
    }

    private void FinalSequence()
    {
        //put the combat menu off
        CombatMenu.enabled = false;
        TextTyper.letterPause = 0f;

        //Movement sequence
        if (IsBetween<float>(timeElapsed, 0f, 2f))
            MovementValue = -1;
        else if (IsBetween<float>(timeElapsed, 2f, 2.5f))
            MovementValue = 1;
        else
            MovementValue = 0;

        //loop dialog
        if (FStextIndex == 7)
            FStextIndex = 2;

        if (!TextTyper.IsTyping)
        {
            //increase dialog index
            FStextIndex++;

            //makes sure dialog is within dialog index range
            if (FSDialog.Length < FStextIndex)
                return;

            //type dialog
            TextTyper.DisplayText(FSDialog[FStextIndex - 1]);
        }

        if (HasPickUpCrate)
        {
            BazookaGameObject.gameObject.SetActive(true);
            GunGameObject.gameObject.SetActive(true);

            PokemonMusic.enabled = false;
            XGonMusic.enabled = true;
        }

        if (VillianDead)
        {
            Text.enabled = false;
            TextBox.enabled = false;
            CharacterTalking.enabled = false;
        }

    }
    //https://stackoverflow.com/questions/5023213/is-there-a-between-function-in-c
    public bool IsBetween<T>( T item, T start, T end)
    {
        return Comparer<T>.Default.Compare(item, start) >= 0
            && Comparer<T>.Default.Compare(item, end) <= 0;
    }
}
