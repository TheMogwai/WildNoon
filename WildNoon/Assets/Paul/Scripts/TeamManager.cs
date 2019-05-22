using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Pour créé un nouveau character : Copier/Coller un bouton dans "Characters Parent", bien penser à indiquer au button et a l'event trigger du bouton quel est la place du nouveau character dans la liste des characters (rappel : le premier = 0, le second = 1...)
// Créer ensuite un nouveau scriptable object, y référencer les stats souhaitées, cela fait, penser à référencer le scriptable object dans le script "Unit Stats Button" sur le nouveau bouton créé
// Pour finir placez le model 3d sur la scène, placez ensuite une nouvelle caméra de la façon souhaitée, créez une nouvelle "render Texture" que vous appliquez à la nouvelle caméra, référencer cette texture dans "l'albedo" d'un nouveau material. Et enfin référencez ce material dans une nouvelle case du tableau "cameraRendererMaterial" que vous aurez créé (Attention à ce la place que prend votre material correspond à la place du nouveau character dans la liste des characters.

public class TeamManager : MonoBehaviour
{


    public GameObject CharactersParents;
    public GameObject CharactersInfoDisplay;
    public GameObject ReadyToPlayButton;

    [Header("Character Name/Description")]
    public Text m_characterName;
    public Text m_characterDescription;

    [Header("Art Display")]
    public Image CharacterModel;
    public Material[] cameraRendererMaterial;

    [Header("Stats Display")]
    public Image m_courage;
    public Image m_damage;
    public Image m_range;
    public Image m_mobility;
    public Image m_heatlh;
    public Image m_armor;

    [Header("Spell Art Display")]
    public Image m_spell_1;
    public Image m_spell_2;
    public Image m_spell_3;
    public Image m_spell_4;

    [Header("Spell Description Display")]
    public Text m_spell_1_Description;                           //Zone à changer si on passe en TextMeshPro;
    public Text m_spell_2_Description;
    public Text m_spell_3_Description;
    public Text m_spell_4_Description;


    [Space]
    [Header("Team Building Teams")]
    public GameObject TeamBuildTeam_1;
    public GameObject TeamBuildTeam_2;
    [Header("Team Building BackGround")]
    public GameObject BackGroundTeam_1;
    public GameObject BackGroundTeam_2;
    [Header("Team Building")]
    public int[] m_nbrCharacterToChoose_Team_1;
    public int[] m_nbrCharacterToChoose_Team_2;


    [Space]
    [Space]
    [Header("Debug")]
    public bool m_displaySomethingAtFirst;

    int m_turnCount;


    #region Private Array Var

    Image[] SlotsTeam_1;
    Image[] SlotsTeam_2;
    UnitStatsButtons[] character;


    Image[] StatsDisplay;
    Image[] SpellArtDisplay;
    Text[] SpellDescription;

    GameObject[] m_Team_1 = new GameObject[4];
    GameObject[] m_Team_2 = new GameObject[4];

    #endregion

    int charaNbrInTheList;

    bool m_characterPressed;

    int m_countCharaTeam_1;
    int m_countCharaTeam_2;

    GameObject m_unit_Spawer;


    private void Awake()
    {



        SlotsTeam_1 = TeamBuildTeam_1.GetComponentsInChildren<Image>();
        SlotsTeam_2 = TeamBuildTeam_2.GetComponentsInChildren<Image>();
        character = CharactersParents.GetComponentsInChildren<UnitStatsButtons>();
        m_unit_Spawer = FindObjectOfType<Unit_Spawer>().gameObject;
        StatsDisplay = new Image[6] { m_courage, m_heatlh, m_armor, m_damage, m_range, m_mobility };
        SpellArtDisplay = new Image[4] { m_spell_1, m_spell_2, m_spell_3, m_spell_4 };
        SpellDescription = new Text[4] { m_spell_1_Description, m_spell_2_Description, m_spell_3_Description, m_spell_4_Description};

        m_countCharaTeam_1 = 0;
        m_countCharaTeam_2 = 0;

        if (m_displaySomethingAtFirst)
        {
            #region Display Chara

            #region Camera Texture

            CharacterModel.material = cameraRendererMaterial[0];

            #endregion

            #region Character Description

            m_characterName.text = character[0].stats.m_name;
            m_characterDescription.text = character[0].stats.m_punshLine;

            #endregion

            #region Character Stats

            int[] CharacterStats = new int[6] { character[0].stats.m_courage, character[0].stats.m_heatlh, character[0].stats.m_armor, character[0].stats.m_damage, character[0].stats.m_range, character[0].stats.m_mobility,  };
            int[] CharacterStatsMax = new int[6] { 20, 100, 100, 20, 10, 5 };
            for (int i = 0, l = StatsDisplay.Length; i < l; ++i)
            {

                StatsDisplay[i].fillAmount = Mathf.InverseLerp(0, CharacterStatsMax[i], CharacterStats[i]);
            }

            #endregion

            #region Character Spell

            Spells[] CharacterSpell = new Spells[4] { character[0].stats.firstSpell, character[0].stats.secondSpell, character[0].stats.thirdSpell, character[0].stats.FourthSpell };
            for (int a = 0, f = SpellArtDisplay.Length; a < f; ++a)
            {
                SpellArtDisplay[a].sprite = CharacterSpell[a].artwork;
                SpellDescription[a].text = CharacterSpell[a].spell_Description;
            }

            #endregion

            #endregion
        }


        CheckTotalChoose(m_nbrCharacterToChoose_Team_1);
        CheckTotalChoose(m_nbrCharacterToChoose_Team_2);


    }

    #region Debug Array Turn To Choose
    int nbrToChooseToBig;

    void CheckTotalChoose(int[] nbr)
    {

        for (int i = 0, l = nbr.Length; i < l; ++i)
        {
            nbrToChooseToBig += nbr[i];
        }
        if(nbrToChooseToBig > 4)
        {
            Debug.LogError("Too Many Character Can Be Choosen in array : " + nbr);
        }
        nbrToChooseToBig = 0;
    }

    #endregion


    public void OnCharactersPressed(int nbrInTheList)
    {
        charaNbrInTheList = nbrInTheList;
        if (m_turnCount < m_nbrCharacterToChoose_Team_1.Length || m_turnCount < m_nbrCharacterToChoose_Team_2.Length)
        {
            if (CheckWhoseTurnIsIt() == 0 || CheckWhoseTurnIsIt() == 2)
            {
                DisplayCharacterPressed(m_countCharaTeam_1 , SlotsTeam_1);
            }
            else if(CheckWhoseTurnIsIt() == 1)
            {
                DisplayCharacterPressed(m_countCharaTeam_2, SlotsTeam_2);
            }
        }
    }

    public void OnCharacterSelected()
    {
        if (m_turnCount < m_nbrCharacterToChoose_Team_1.Length || m_turnCount < m_nbrCharacterToChoose_Team_2.Length)
        {
            if (CheckWhoseTurnIsIt() == 0)
            {
                if (m_characterPressed)
                {
                    if (m_countCharaTeam_1 < SlotsTeam_1.Length)
                    {
                        SlotsTeam_1[m_countCharaTeam_1].color = Color.red;                              //Pour afficher le fait que le perso a été choisi
                        m_Team_1[m_countCharaTeam_1] = character[charaNbrInTheList].gameObject;

                        m_unit_Spawer.GetComponent<Unit_Spawer>().Team1[m_countCharaTeam_1] = character[charaNbrInTheList].gameObject;

                    }
                    m_characterPressed = false;
                    m_countCharaTeam_1++;
                    m_nbrCharacterToChoose_Team_1[m_turnCount]--;
                }
                if (CheckWhoseTurnIsIt() == 1)
                {
                    InverseBackGround();
                }
            }
            else if (CheckWhoseTurnIsIt() == 1)
            {
                if (m_characterPressed)
                {
                    if (m_countCharaTeam_2 < SlotsTeam_2.Length)
                    {
                        SlotsTeam_2[m_countCharaTeam_2].color = Color.red;                              //Pour afficher le fait que le perso a été choisi
                        m_Team_2[m_countCharaTeam_2] = character[charaNbrInTheList].gameObject;

                        m_unit_Spawer.GetComponent<Unit_Spawer>().Team2[m_countCharaTeam_2] = character[charaNbrInTheList].gameObject;

                    }
                    m_characterPressed = false;
                    m_countCharaTeam_2++;
                    m_nbrCharacterToChoose_Team_2[m_turnCount]--;
                }
                if (CheckWhoseTurnIsIt() == 2)
                {
                    m_turnCount++;
                    InverseBackGround();
                    if (m_turnCount == m_nbrCharacterToChoose_Team_1.Length || m_turnCount == m_nbrCharacterToChoose_Team_2.Length)
                    {
                        OnReadyToPlay();
                    }
                }
            }
        }
    }

    void OnReadyToPlay()
    {
        ReadyToPlayButton.GetComponent<Button>().interactable = true;
    }

    void InverseBackGround()
    {
        BackGroundTeam_1.SetActive(!BackGroundTeam_1.activeSelf);
        BackGroundTeam_2.SetActive(!BackGroundTeam_2.activeSelf);
    }

    int CheckWhoseTurnIsIt()
    {
        if (m_nbrCharacterToChoose_Team_1[m_turnCount] > 0)
        {
            return 0;
        }
        else if (m_nbrCharacterToChoose_Team_2[m_turnCount] > 0)
        {
            return 1;
        }
        return 2;
    }

    void DisplayCharacterPressed(int m_countCharaTeam, Image[] Slots)
    {
        if (m_countCharaTeam < Slots.Length)
        {
            Slots[m_countCharaTeam].sprite = character[charaNbrInTheList].GetComponent<Image>().sprite;
            m_characterPressed = true;
        }
    }

    public void OnMouseTrigger(int nbrInTheList)
    {

        charaNbrInTheList = nbrInTheList;

        #region Camera Texture

        CharacterModel.material = cameraRendererMaterial[nbrInTheList];

        #endregion

        #region Character Description

        m_characterName.text = character[nbrInTheList].stats.m_name;
        m_characterDescription.text = character[nbrInTheList].stats.m_punshLine;

        #endregion

        #region Character Stats

        int[] CharacterStats = new int[6] { character[nbrInTheList].stats.m_courage, character[nbrInTheList].stats.m_heatlh, character[nbrInTheList].stats.m_armor, character[nbrInTheList].stats.m_damage, character[nbrInTheList].stats.m_range, character[nbrInTheList].stats.m_mobility, };
        int[] CharacterStatsMax = new int[6] { 20, 100, 100, 20, 10, 5 };
        for (int i = 0, l= StatsDisplay.Length; i < l; ++i)
        {
            StatsDisplay[i].fillAmount = Mathf.InverseLerp(0, CharacterStatsMax[i], CharacterStats[i]);
        }

        #endregion

        #region Character Spell

        Spells[] CharacterSpell = new Spells[4] { character[nbrInTheList].stats.firstSpell, character[nbrInTheList].stats.secondSpell, character[nbrInTheList].stats.thirdSpell, character[nbrInTheList].stats.FourthSpell };
        for (int a = 0, f = SpellArtDisplay.Length; a < f; ++a)
        {
            SpellArtDisplay[a].sprite = CharacterSpell[a].artwork;
            SpellDescription[a].text = CharacterSpell[a].spell_Description;
        }

        #endregion

    }

    public void ReadyToPlay()
    {
        m_unit_Spawer.GetComponent<Unit_Spawer>().OnConvertArray(m_Team_1,m_Team_2);
        StartCoroutine(Gitanerie());
    }

    IEnumerator Gitanerie()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
        DontDestroyOnLoad(m_unit_Spawer);
    }


}
