using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AssignFormationData : MonoBehaviour
{
    //public Text captionsymbol;
    public GameObject parentObject;
    public GameObject AtkparentObject;
    public GameObject BalparentObject;

    public GameObject formationObject;
    public GameObject AtkformationObject;
    public GameObject BalformationObject;

    public GameObject backButton;
    public GameObject formationButton;
    public GameObject stadiumButton;
    public GameObject SwipeButton;
    public GameObject captainButton;
    public GameObject playerUpgradeButton;
    public GameObject onlineformationplay_Btn;

    public GameObject MVP_BG_panel; // as per MVP task.

    public Button[] myMainDiscPos;

    public List<GameObject> ActiveDiscs;
    public List<GameObject> InActiveDiscs;

    public Transform inActiveObjectParent;

    public GameObject inActiveObjectPrefab;

    public float initialvalueTime = 20f;
    public float initialvalueForce = 3.5f;
    public float initialvalueAIM = 10f;

    public GameObject onlineForamtionPanel;

    public RectTransform DefPanel, AtkPanel, BalPanel;

    public GameObject GS_RevitalizationPanel;
    public Button attack, defence, balance;
    public GameObject SelectBtn;

    public Sprite[] Active_Btn_sprite;

    //Socket formtaion disable --@sud
 //*************************//
  //   public List<FormationData> formationsAttack { get; set; } = new List<FormationData>();
   //  public List<FormationData> formationsDefence { get; set; } = new List<FormationData>();
  //   public List<FormationData> formationsBalance { get; set; } = new List<FormationData>();

    //*************************//

   // bool isAssignFormationInGame;//unused?? ->FST
    //vectors used for anchoring made once only ->FST
    private readonly Vector2 r_VecA = new Vector2(-1500, 0);
    private readonly Vector2 r_VecB = new Vector2(1500, 0);
    private readonly Vector2 r_VecZero = new Vector2(0, 0);


    #region Getters

    private Image m_AttackImage = null;
    private Image AttackImage { get { if (!m_AttackImage) if (attack) m_AttackImage = attack.GetComponent<Image>(); return m_AttackImage; } }

    private Image m_DefenceImage = null;
    private Image DefenseImage { get { if (!m_DefenceImage) if (defence) m_DefenceImage = defence.GetComponent<Image>(); return m_DefenceImage; } }

    private Image m_BalanceImage = null;
    private Image BalanceImage { get { if (!m_BalanceImage) if (balance) m_BalanceImage = balance.GetComponent<Image>(); return m_BalanceImage; } }

    private ScrollRect m_AttackScroll = null;
    private ScrollRect AttackScroll { get { if (!m_AttackScroll) if (AtkPanel) m_AttackScroll = AtkPanel.GetComponent<ScrollRect>(); return m_AttackScroll; } }

    private ScrollRect m_DefenseScroll = null;
    private ScrollRect DefenseScroll { get { if (!m_DefenseScroll) if (DefPanel) m_DefenseScroll = DefPanel.GetComponent<ScrollRect>(); return m_DefenseScroll; } }

    private ScrollRect m_BalanceScroll = null;
    private ScrollRect BalanceScroll { get { if (!m_BalanceScroll) if (BalPanel) m_BalanceScroll = BalPanel.GetComponent<ScrollRect>(); return m_BalanceScroll; } }

    //we will cache these when we create them, as we reuse them, instead of destroy we will pool the objects ->FST
 // commented --@sud
    // private List<GameObject> m_SpawnedAttackObs = new List<GameObject>();

    // private List<GameObject> SpawnedAttackObs
    // {
    //     get
    //     {
    //         if (m_SpawnedAttackObs == null || m_SpawnedAttackObs.Count < 1)
    //         {
    //             if (formationsAttack.Count > 0)
    //             {
    //                 for (int i = 0; i < formationsAttack.Count; i++) // AS for new server //As per MVP task list
    //                 {
    //                     GameObject gm = Instantiate(AtkformationObject, Vector3.zero, Quaternion.identity) as GameObject;
    //                     //	gm.GetComponent <Image> ().sprite = formationsAttack [i].formationImgURL;
    //                     gm.name = "Formation" + i;
    //                     gm.transform.SetParent(AtkparentObject.transform);   // more suitable than parent = transform. ->FST
    //                     gm.transform.localScale = Vector3.one;
    //                     gm.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    //                     m_SpawnedAttackObs.Add(gm);
    //                    // Debug.Log(gm + " added");
    //                 }
    //             }
    //         }
    //         return m_SpawnedAttackObs;
    //     }
    // }

    // private List<GameObject> m_SpawnedDefenseObs = new List<GameObject>();
    // private List<GameObject> SpawnedDefenseObs
    // {
    //     get
    //     {
    //         if (m_SpawnedDefenseObs == null || m_SpawnedDefenseObs.Count < 1)
    //         {
    //             if (formationsDefence.Count > 0)
    //             {
    //                 for (int i = 0; i < formationsDefence.Count; i++) // AS for new server //As per MVP task list
    //                 {
    //                     GameObject gm = Instantiate(formationObject, Vector3.zero, Quaternion.identity) as GameObject;
    //                     //	gm.GetComponent <Image> ().sprite = formationsDefence [i].formationImgURL;
    //                     gm.name = "Formation" + i;
    //                     gm.transform.SetParent(parentObject.transform);            // more suitable than parent = transform. ->FST
    //                     gm.transform.localScale = Vector3.one;
    //                     gm.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    //                     m_SpawnedDefenseObs.Add(gm);
    //                    // Debug.Log(gm + " added");
    //                 }
    //             }
    //         }
    //         return m_SpawnedDefenseObs;
    //     }
    // }
    // private List<GameObject> m_SpawnedBalanceObs = new List<GameObject>();
    // private List<GameObject> SpawnedBalanceObs
    // {
    //     get
    //     {
    //         if (m_SpawnedBalanceObs == null || m_SpawnedBalanceObs.Count < 1)
    //         {
    //             if(formationsBalance.Count > 0)
    //             {
    //                 for (int i = 0; i < formationsBalance.Count; i++) // AS for new server //As per MVP task list
    //                 {
    //                     GameObject gm = Instantiate(BalformationObject, Vector3.zero, Quaternion.identity) as GameObject;
    //                     //	gm.GetComponent <Image> ().sprite = formationsBalance [i].formationImgURL;
    //                     gm.name = "Formation" + i;
    //                     gm.transform.SetParent(BalparentObject.transform);     // more suitable than parent = transform. ->FST
    //                     gm.transform.localScale = Vector3.one;
    //                     gm.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    //                     m_SpawnedBalanceObs.Add(gm);
    //                     // Debug.Log(gm + " added");
    //                 }
    //             }
    //         }
    //         return m_SpawnedBalanceObs;
    //     }
    // }
    #endregion

    public static AssignFormationData Instance { get; private set; } = null;//Capitalised appropriately ->FST

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else Destroy(this);
    }
    void Start()
    {
        SelectBtn.SetActive(false);
        AssignFormationDefence();

    }
    // To changes position of panel and color of buttons
    private void ActiveSprite(string active)
    {
        switch (active)
        {
            case "attack":
                AttackImage.sprite = Active_Btn_sprite[0];
                DefenseImage.sprite = Active_Btn_sprite[1];
                BalanceImage.sprite = Active_Btn_sprite[1];
                AttackScroll.normalizedPosition = new Vector2(0.5f, 0.5f);
                AtkparentObject.transform.position = transform.parent.position;
                DefPanel.DOAnchorPos(r_VecA, 0.25f, true);
                AtkPanel.DOAnchorPos(r_VecZero, 0.25f, true);
                BalPanel.DOAnchorPos(r_VecB, 0.25f, true);
                break;

            case "defence":
                AttackImage.sprite = Active_Btn_sprite[1];
                DefenseImage.sprite = Active_Btn_sprite[0];
                BalanceImage.sprite = Active_Btn_sprite[1];
                DefenseScroll.normalizedPosition = new Vector2(0.5f, 0.5f);
                parentObject.transform.position = transform.parent.position;
                DefPanel.DOAnchorPos(r_VecZero, 0.25f, true);
                AtkPanel.DOAnchorPos(r_VecB, 0.25f, true);
                BalPanel.DOAnchorPos(r_VecB, 0.25f, true);
                //parentObject.transform.position = new Vector3(0,0,0);
                break;

            case "balance":
                AttackImage.sprite = Active_Btn_sprite[1];
                DefenseImage.sprite = Active_Btn_sprite[1];
                BalanceImage.sprite = Active_Btn_sprite[0];
                BalanceScroll.normalizedPosition = new Vector2(0.5f, 0.5f);
                BalparentObject.transform.position = transform.parent.position;
                DefPanel.DOAnchorPos(r_VecA, 0.25f, true);
                AtkPanel.DOAnchorPos(r_VecB, 0.25f, true);
                BalPanel.DOAnchorPos(r_VecZero, 0.25f, true);
                break;
        }
    }

    public void AssignFormationAttack()
    {
        // were destroying...replaced with cache after first creation and enable/disbale ->FST
        // commented --@sud
        // for (int i = 0; i < SpawnedAttackObs.Count; i++)
        //     if (SpawnedAttackObs[i].activeSelf)
        //         SpawnedAttackObs[i].SetActive(false);

        SelectBtn.SetActive(false);
        ActiveSprite("attack");

        SetAttackPosition();

        // AtkPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0.5f, 0.5f);
        // AtkparentObject.transform.position = AtkPanel.transform.position;
        // Debug.Log("Assign Formation---->> I'm here");
    }

    public void SetAttackPosition()
    {
        //error prevention ->FST
        // commented --@sud
        /*         if (!(formationsAttack.Count > 0))
                 {
                     //Warn dev and return ->FST
                    Debug.LogWarning("FST Warning Notification -> list not populated @ AssignFormationData.cs > formationsAttack");
                     return;
                 }

                Debug.Log("list populated!!! @ AssignFormationData.cs > formationsAttack"); // just to get an idea of which order this is coming to repair above ->FST

        // commented --@sud
                 for (int i = 0; i < SpawnedAttackObs.Count; i++) // AS for new server //As per MVP task list
                 {
                     // were destroying...replaced with cache after first creation and enable/disbale ->FST
                     if (!SpawnedAttackObs[i].activeSelf)
                         SpawnedAttackObs[i].SetActive(true);

                     //CACHE FIRST!!! ->FST
                     FormationSelection fs = SpawnedAttackObs[i].GetComponent<FormationSelection>();
                     fs.formation_index = formationsAttack[i].Index_no;
                     fs.formationName = formationsAttack[i].formationName;
                     fs.formationid = (formationsAttack[i].id);
                     fs.point = formationsAttack[i].point;
                     fs.pointType = formationsAttack[i].pointType;
                     fs.activeFormation = formationsAttack[i].isActive;
                    fs.statusPurchase = formationsAttack[i].purchaseStatus;
                     fs.formationType = formationsAttack[i].formationType;
                     fs.myDisk = new List<PositionOfDisk>();
                     fs.myUIDisk = new List<UIPositionOfDisk>();
                            //     // Debug.Log("Formation Index Number ---------->" + formationsAttack[i].Index_no);
                     // Debug.Log("Formation Name ---------->" + formationsAttack[i].formationName);
                     // Debug.Log("Disks Count---------->" + formationsAttack[i].posDisc.Count);


                     for (int j = 0; j < formationsAttack[i].posDisc.Count; j++)
                     {
                         //  Debug.Log("Disk Position Number-->> " + j);

                        //Simplify construction ->FST
                        PositionOfDisk POD = new PositionOfDisk
                        {
                             name = formationsAttack[i].posDisc[j].Name,
                             myPosX = formationsAttack[i].posDisc[j].posX,
                             myPosY = formationsAttack[i].posDisc[j].posY
                         };

                         //USe cached ->FST
                         fs.myDisk.Add(POD);

                         //As per MVP task list
                         //Simplify construction ->FST
                         UIPositionOfDisk UPD = new UIPositionOfDisk
                         {
                             name = formationsAttack[i].posUIDisc[j].Name,
                             myPosX = formationsAttack[i].posUIDisc[j].posX,
                             myPosY = formationsAttack[i].posUIDisc[j].posY
                         };
                         //UPD.myPosZ = formationsAttack [i].posUIDisc [j].posZ;
                         //Use Cached ->FST
                         fs.myUIDisk.Add(UPD);

                //         // Debug.Log("Disk UI Position Number " + j);
                     }
                 }*/

        // int playerformation =  FST_SettingsManager.Formation;

        // //As per MVP task list
        //     if (playerformation < formationsAttack[0].Index_no)
        //     {
        //        FST_SettingsManager.Formation = formationsAttack[0].Index_no);

        //         for (int j = 0; j < 5; j++)
        //             Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsAttack[0].posUIDisc[j].posX) / 1.77f, float.Parse(formationsAttack[0].posUIDisc[j].posY) / 1.77f);
        //     }
        //     else
        //     {
        //         for (int i = 0; i < formationsAttack.Count; i++)
        //             if (playerformation == formationsAttack[i].Index_no)
        //                 for (int j = 0; j < formationsAttack[i].posDisc.Count; j++)
        //                     Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsAttack[i].posUIDisc[j].posX) / 1.77f, float.Parse(formationsAttack[i].posUIDisc[j].posY) / 1.77f);
        //     }

    }

    public void AssignFormationDefence()
    {
        // were destroying...replaced with cache after first creation and enable/disbale ->FST
        // commented --@sud
        // for (int i = 0; i < SpawnedDefenseObs.Count; i++)
        //     if (SpawnedDefenseObs[i].activeSelf)
        //         SpawnedDefenseObs[i].SetActive(false);

        SelectBtn.SetActive(false);
        ActiveSprite("defence");
        SetDefPosition();
    }


    public void SetDefPosition()
    {
        //Good chance this will fail in online sessions as the two list must be exactly equal at all times! Note to self.. Fix this. ->FST

        //error prevention ->FST
 // commented --@sud
        // if (!(formationsDefence.Count > 0))
        // {
        //     //Warn dev and return ->FST
        //   //  Debug.LogWarning("FST Warning Notification -> list not populated @ AssignFormationData.cs > formationsDefence");
        //     return;
        // }

      //  Debug.Log("list populated!!! @ AssignFormationData.cs > formationsDefence"); // just to get an idea of which order this is coming to repair above ->FST
// commented --@sud
        // for (int i = 0; i < SpawnedDefenseObs.Count; i++) // AS for new server //As per MVP task list
        // {
        //     // were destroying...replaced with cache after first creation and enable/disbale ->FST
        //     if (!SpawnedDefenseObs[i].activeSelf)
        //         SpawnedDefenseObs[i].SetActive(true);

        //     //CACHE VARS THAT ARE USED OVER AND OVER!!!!  ->FST
        //     FormationSelection fs = SpawnedDefenseObs[i].GetComponent<FormationSelection>();

        //     //THEN USED CACHED VAR!!! ->FST
        //     fs.formation_index = formationsDefence[i].Index_no;
        //     fs.formationName = formationsDefence[i].formationName;
        //     fs.formationid = (formationsDefence[i].id);
        //     fs.point = formationsDefence[i].point;
        //     fs.pointType = formationsDefence[i].pointType;
        //     fs.activeFormation = formationsDefence[i].isActive;
        //     fs.statusPurchase = formationsDefence[i].purchaseStatus;
        //     fs.formationType = formationsDefence[i].formationType;
        //     fs.myDisk = new List<PositionOfDisk>();
        //     fs.myUIDisk = new List<UIPositionOfDisk>();
        //     // Debug.Log("Formation Index Number ---------->" + formationsDefence[i].Index_no);
        //     // Debug.Log("Formation Name ---------->" + formationsDefence[i].formationName);
        //     // Debug.Log("Disks Count---------->" + formationsDefence[i].posDisc.Count);

        //     for (int j = 0; j < formationsDefence[i].posDisc.Count; j++)
        //     {
        //         //  Debug.Log("Disk Position Number-->> " + j);
        //         //Simplfied construction ->FST
        //         PositionOfDisk POD = new PositionOfDisk
        //         {
        //             name = formationsDefence[i].posDisc[j].Name,
        //             myPosX = formationsDefence[i].posDisc[j].posX,
        //             myPosY = formationsDefence[i].posDisc[j].posY
        //         };

        //         //USE CACHED VAR!!! ->FST
        //         fs.myDisk.Add(POD);

        //         //As per MVP task list

        //         //Simplfied construction ->FST
        //         UIPositionOfDisk UPD = new UIPositionOfDisk
        //         {
        //             name = formationsDefence[i].posUIDisc[j].Name,
        //             myPosX = formationsDefence[i].posUIDisc[j].posX,
        //             myPosY = formationsDefence[i].posUIDisc[j].posY
        //         };
        //         //UPD.myPosZ = formationsDefence [i].posUIDisc [j].posZ;

        //         //USE CACHED VAR!!! ->FST
        //         fs.myUIDisk.Add(UPD);
        //         // Debug.Log("Disk UI Position Number " + j);
        //     }
        // }

        // //As per MVP task list

        // //THIS BLOCK BELOW IS A RECIPE FOR DISASTER! Note to self.. Fix this. ->FST

        // //  yield return new WaitForSeconds(0.5f);
        // if (PlayerPrefs.HasKey("PlayerFormation"))
        // {
        //     if (PlayerPrefs.GetInt("PlayerFormation") < formationsDefence[0].Index_no)
        //     {
        //         //PlayerPrefs.SetInt ("PlayerFormation", formationsDefence [0].id);
        //         PlayerPrefs.SetInt("PlayerFormation", formationsDefence[0].Index_no);
        //         for (int j = 0; j < 5; j++)
        //             Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsDefence[0].posUIDisc[j].posX) / 1.77f, float.Parse(formationsDefence[0].posUIDisc[j].posY) / 1.77f);
        //     }
        //     else
        //     {
        //         for (int i = 0; i < formationsDefence.Count; i++)
        //             if (PlayerPrefs.GetInt("PlayerFormation") == formationsDefence[i].Index_no)
        //                 for (int j = 0; j < formationsDefence[i].posDisc.Count; j++)
        //                     Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsDefence[i].posUIDisc[j].posX) / 1.77f, float.Parse(formationsDefence[i].posUIDisc[j].posY) / 1.77f);
        //     }
        // }
        // else
        // {
        //     PlayerPrefs.SetInt("PlayerFormation", formationsDefence[0].Index_no);
        //     for (int j = 0; j < formationsDefence[0].posDisc.Count; j++)
        //         Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsDefence[0].posUIDisc[j].posX) / 1.77f, float.Parse(formationsDefence[0].posUIDisc[j].posY) / 1.77f);
        // }
    //    Debug.Log("SetDefPosition()");

        UIController.Instance.DeactiveLoading2Panel(0.5f);

    }
    public void AssignFormationBalanced()
    {
        // were destroying...replaced with cache after first creation and enable/disbale ->FST
        // commented --@sud
        // for (int i = 0; i < SpawnedBalanceObs.Count; i++)
        //     if (SpawnedBalanceObs[i].activeSelf)
        //         SpawnedBalanceObs[i].SetActive(false);

        SelectBtn.SetActive(false);
        ActiveSprite("balance");
        SetBalancePosition();

        // Debug.Log("Assign Formation---->> I'm here");
    }
    public void SetBalancePosition()
    {
        //error prevention ->FST
        // commented --@sud
        // if (!(formationsBalance.Count > 0))
        // {
        //     //Warn dev and return ->FST
        //   //  Debug.LogWarning("FST Warning Notification -> list not populated @ AssignFormationData.cs > formationsBalance");
        //     return;
        // }

        //we made it lets find out when ->FST
     //   Debug.Log("list populated!!! @ AssignFormationData.cs > formationsBalance"); // just to get an idea of which order this is coming to repair above ->FST
// commented --@sud

        // for (int i = 0; i < SpawnedBalanceObs.Count; i++) // AS for new server //As per MVP task list //replaced with pooling list ->FST
        // {
        //     // were destroying...replaced with cache after first creation and enable/disbale ->FST
        //     if (!SpawnedBalanceObs[i].activeSelf)
        //         SpawnedBalanceObs[i].SetActive(true);

        //     //CACHE VARS THAT ARE USED OVER AND OVER!!!!  ->FST
        //     FormationSelection fs = SpawnedBalanceObs[i].GetComponent<FormationSelection>();

        //     //THEN USED CACHED VAR!!! ->FST
        //     fs.formation_index = formationsBalance[i].Index_no;
        //     fs.formationName = formationsBalance[i].formationName;
        //     fs.formationid = (formationsBalance[i].id);
        //     fs.point = formationsBalance[i].point;
        //     fs.pointType = formationsBalance[i].pointType;
        //     fs.activeFormation = formationsBalance[i].isActive;
        //     fs.statusPurchase = formationsBalance[i].purchaseStatus;
        //     fs.formationType = formationsBalance[i].formationType;
        //     fs.myDisk = new List<PositionOfDisk>();
        //     fs.myUIDisk = new List<UIPositionOfDisk>();
        //     // Debug.Log("Formation Index Number ---------->" + formationsBalance[i].Index_no);
        //     // Debug.Log("Formation Name ---------->" + formationsBalance[i].formationName);
        //     // Debug.Log("Disks Count---------->" + formationsBalance[i].posDisc.Count);

        //     for (int j = 0; j < formationsBalance[i].posDisc.Count; j++)
        //     {
        //         //  Debug.Log("Disk Position Number-->> " + j);
        //         //Simplfied construction ->FST
        //         PositionOfDisk POD = new PositionOfDisk
        //         {
        //             name = formationsBalance[i].posDisc[j].Name,
        //             myPosX = formationsBalance[i].posDisc[j].posX,
        //             myPosY = formationsBalance[i].posDisc[j].posY
        //         };

        //         //USE CACHED VAR!!! ->FST
        //         fs.myDisk.Add(POD);

        //         //As per MVP task list

        //         //Simplfied construction ->FST
        //         UIPositionOfDisk UPD = new UIPositionOfDisk
        //         {
        //             name = formationsBalance[i].posUIDisc[j].Name,
        //             myPosX = formationsBalance[i].posUIDisc[j].posX,
        //             myPosY = formationsBalance[i].posUIDisc[j].posY
        //         };
        //         //UPD.myPosZ = formationsBalance [i].posUIDisc [j].posZ;
        //         //USE CACHED VAR!!! ->FST
        //         fs.myUIDisk.Add(UPD);
        //         // Debug.Log("Disk UI Position Number " + j);
        //     }
        // }

        // //As per MVP task list
        // //THIS BLOCK BELOW IS A RECIPE FOR DISASTER! Note to self.. Fix this. ->FST

        // if (PlayerPrefs.HasKey("PlayerFormation"))
        // {
        //     if (PlayerPrefs.GetInt("PlayerFormation") < formationsBalance[0].Index_no)
        //     {
        //         //PlayerPrefs.SetInt ("PlayerFormation", formationsBalance [0].id);
        //         PlayerPrefs.SetInt("PlayerFormation", formationsBalance[0].Index_no);
        //         for (int j = 0; j < 5; j++)
        //             Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsBalance[0].posUIDisc[j].posX) / 1.77f, float.Parse(formationsBalance[0].posUIDisc[j].posY) / 1.77f);
        //     }
        //     else
        //     {
        //         for (int i = 0; i < formationsBalance.Count; i++)
        //             if (PlayerPrefs.GetInt("PlayerFormation") == formationsBalance[i].Index_no)
        //                 for (int j = 0; j < formationsBalance[i].posDisc.Count; j++)
        //                    Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsBalance[i].posUIDisc[j].posX) / 1.77f, float.Parse(formationsBalance[i].posUIDisc[j].posY) / 1.77f);
        //     }
        // }
        // else
        // {
        //     PlayerPrefs.SetInt("PlayerFormation", formationsBalance[0].Index_no);
        //     for (int j = 0; j < formationsBalance[0].posDisc.Count; j++)
        //         Instance.myMainDiscPos[j].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(float.Parse(formationsBalance[0].posUIDisc[j].posX) / 1.77f, float.Parse(formationsBalance[0].posUIDisc[j].posY) / 1.77f);
        // }
    }

    public void ButtonCallEvent(string name)
    {
        string buttonname = name.ToLower();
        switch (buttonname)
        {
            case "defbtn":
                Debug.Log("Defence");
                AssignFormationDefence();
                break;
            case "atkbtn":
                Debug.Log("attack");
                AssignFormationAttack();
                break;
            case "balbtn":
                Debug.Log("balance");
                AssignFormationBalanced();
                break;
        }
    }

    // public void AssignPlayerSubstitutionValueInGame()
    // {

    //     // Debug.Log("Now here");

    //     WebServicesHandler.SharedInstance.ActivePlayerList.Clear();
    //     WebServicesHandler.SharedInstance.InActivePlayerList.Clear();

    //     for (int i = 0; i < WebServicesHandler.SharedInstance.statsValue.discList.Count; i++)
    //     {

    //         if (WebServicesHandler.SharedInstance.statsValue.discList[i].mStatus == "Active")
    //         {
    //             WebServicesHandler.SharedInstance.ActivePlayerList.Add(WebServicesHandler.SharedInstance.statsValue.discList[i]);
    //         }

    //         if (WebServicesHandler.SharedInstance.statsValue.discList[i].mStatus == "InActive")
    //         {
    //             WebServicesHandler.SharedInstance.InActivePlayerList.Add(WebServicesHandler.SharedInstance.statsValue.discList[i]);
    //         }
    //     }

    //     if (SceneManager.GetActiveScene().name == "InGame" && !isAssignFormationInGame)
    //     {
    //         isAssignFormationInGame = true;
    //         AssignFormationAttack();
    //     }

    //     for (int i = 0; i < WebServicesHandler.SharedInstance.ActivePlayerList.Count; i++)
    //     {
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().idDisc = WebServicesHandler.SharedInstance.ActivePlayerList[i].mDiskId;
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().diskName = WebServicesHandler.SharedInstance.ActivePlayerList[i].mDiskName;
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().forceData = WebServicesHandler.SharedInstance.ActivePlayerList[i].mForceData;
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().aimData = WebServicesHandler.SharedInstance.ActivePlayerList[i].mAim;
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().timeData = WebServicesHandler.SharedInstance.ActivePlayerList[i].mTime;
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().staminaData = WebServicesHandler.SharedInstance.ActivePlayerList[i].mStamina;
    //         ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().statusData = WebServicesHandler.SharedInstance.ActivePlayerList[i].mStatus;
    //ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().idDisc = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_id;
    //ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().diskName = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_name;
    //ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().forceData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force;
    //ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().aimData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim;
    //ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().timeData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer;
    //// ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().staminaData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>()
    //ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().statusData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_status;

    //  }

    //Comment for MVP task
    //if (inActiveObjectParent.childCount == 1)
    //{

    //    for (int j = 0; j < WebServicesHandler.SharedInstance.InActivePlayerList.Count; j++)
    //    {
    //        GameObject gm = Instantiate(inActiveObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    //        gm.name = "SwipePlayer" + j;
    //        gm.transform.parent = inActiveObjectParent;
    //        gm.transform.localScale = Vector3.one;
    //        gm.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().idDisc = WebServicesHandler.SharedInstance.InActivePlayerList[j].mDiskId;
    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().diskName = WebServicesHandler.SharedInstance.InActivePlayerList[j].mDiskName;
    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().forceData = WebServicesHandler.SharedInstance.InActivePlayerList[j].mForceData;
    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().aimData = WebServicesHandler.SharedInstance.InActivePlayerList[j].mAim;
    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().timeData = WebServicesHandler.SharedInstance.InActivePlayerList[j].mTime;
    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().staminaData = WebServicesHandler.SharedInstance.InActivePlayerList[j].mStamina;
    //        gm.GetComponentInChildren<MM_InActivePlayerStatminaValue>().statusData = WebServicesHandler.SharedInstance.InActivePlayerList[j].mStatus;

    //        InActiveDiscs.Add(gm);

    //        if (SceneManager.GetActiveScene().name == "InGame")
    //        {
    //            //	BumpStaminaManager.instance.SwapPlayer_timerList.Add (gm);
    //            // for disable player stats,bumpstamiona eneble it when player stats enable
    //        }
    //    }
    //}
    //else
    //{

    //    for (int i = 0; i < InActiveDiscs.Count; i++)
    //    {
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().idDisc = WebServicesHandler.SharedInstance.InActivePlayerList[i].mDiskId;
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().diskName = WebServicesHandler.SharedInstance.InActivePlayerList[i].mDiskName;
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().forceData = WebServicesHandler.SharedInstance.InActivePlayerList[i].mForceData;
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().aimData = WebServicesHandler.SharedInstance.InActivePlayerList[i].mAim;
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().timeData = WebServicesHandler.SharedInstance.InActivePlayerList[i].mTime;
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().staminaData = WebServicesHandler.SharedInstance.InActivePlayerList[i].mStamina;
    //        InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().statusData = WebServicesHandler.SharedInstance.InActivePlayerList[i].mStatus;
    //    }

    //}
    //}

    // int SwipePlayerid, OtherPlayerid;
    //Comment for MVP task
    //public void SwipePlayer()
    //{
    //    SwipePlayerid = 0;
    //    OtherPlayerid = 0;

    //    for (int i = 0; i < ActiveDiscs.Count; i++)
    //    {
    //        //Debug.Log (ActiveDiscs [i].GetComponent <MM_ActivePlayerStatminaValue> ().diskName);
    //        if (ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().isSelectedDisc)
    //        {
    //            SwipePlayerid = i;
    //            ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().isSelectedDisc = false;
    //            ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>().PlayerCardDetailsObject.SetActive(false);
    //        }
    //    }
    //    for (int i = 0; i < InActiveDiscs.Count; i++)
    //    {
    //        if (InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().isSelectedDisc)
    //        {
    //            OtherPlayerid = i;
    //            InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().isSelectedDisc = false;
    //            InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().PlayerCardDetailsObject.SetActive(false);
    //        }
    //    }

    //    playerSwipping(SwipePlayerid, OtherPlayerid);
    //    NonePlayerTap();

    //}
    //Comment for MVP task
    /*
        public void playerSwipping(int player1, int player2)
        {
            if (SceneManager.GetActiveScene().name == "InGame")
            {
                float tempTimer = GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().obj.GetComponent<MM_ActivePlayerStatminaValue>().player_timer_act;
                //GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().obj.GetComponent <MM_ActivePlayerStatminaValue> ().player_timer_act = BumpStaminaManager.instance.SwapPlayer_timerList [player2].GetComponent <MM_InActivePlayerStatminaValue> ().player_timer_Inact;
                // for disable player stats,bumpstamiona eneble it when player stats enable
                //	BumpStaminaManager.instance.SwapPlayer_timerList [player2].GetComponent <MM_InActivePlayerStatminaValue> ().player_timer_Inact = tempTimer;
                // for disable player stats,bumpstamiona eneble it when player stats enable
            }

            float id = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().idDisc;
            string diskName = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().diskName;
            float force = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().forceData;
            float aim = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().aimData;
            float time = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().timeData;
            float stamina = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().staminaData;
            string status = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().statusData;

            //Comment for MVP task
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().idDisc = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().idDisc;
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().diskName = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().diskName;
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().forceData = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().forceData;
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().aimData = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().aimData;
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().timeData = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().timeData;
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().staminaData = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().staminaData;
            //ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().statusData = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().statusData;
            //Comment for MVP task
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().idDisc = id;
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().diskName = diskName;
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().forceData = force;
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().aimData = aim;
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().timeData = time;
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().staminaData = stamina;
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().statusData = status;

            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mDiskId = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().idDisc;
            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mDiskName = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().diskName;
            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mForceData = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().forceData;
            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mAim = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().aimData;
            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mTime = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().timeData;
            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mStamina = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().staminaData;
            WebServicesHandler.SharedInstance.ActivePlayerList[player1].mStatus = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().statusData;
            //Comment for MVP task
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mDiskId = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().idDisc;
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mDiskName = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().diskName;
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mForceData = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().forceData;
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mAim = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().aimData;
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mTime = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().timeData;
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mStamina = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().staminaData;
            //WebServicesHandler.SharedInstance.InActivePlayerList[player2].mStatus = InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().statusData;

            if (SceneManager.GetActiveScene().name == "InGame")
            {
                GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().mDisc_id = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().idDisc;
                GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().discName = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().diskName;
                GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().Force = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().forceData;
                GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().mDisc_aim = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().aimData;
                GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().timePlayer = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().timeData;
                GlobalGameManager.SharedInstance.allPlayer[player1].GetComponent<playerController>().mDisc_status = ActiveDiscs[player1].GetComponent<MM_ActivePlayerStatminaValue>().statusData;
            }

            WebServicesHandler.SharedInstance.updateDisk(
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mForceData,
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mTime,
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mStamina,
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mAim,
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mDiskName,
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mDiskId,
                WebServicesHandler.SharedInstance.ActivePlayerList[player1].mStatus
            );

            WebServicesHandler.SharedInstance.updateDisk(
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mForceData,
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mTime,
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mStamina,
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mAim,
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mDiskName,
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mDiskId,
                WebServicesHandler.SharedInstance.InActivePlayerList[player2].mStatus
            );
            //Comment for MVP task
            //InActiveDiscs[player2].GetComponent<MM_InActivePlayerStatminaValue>().AssignValueofDisk();
           // -----------------------------------------

            //AssignSubDiscDetailsShow.instance.assignValue ();
            // for disable Assignsubdetails eneble it when Assignsubdetails enable
            SwipeButton.SetActive(false);
        }
        */

    public void DeselectAllButton()
    {
        NonePlayerTap();
        for (int i = 0; i < ActiveDiscs.Count; i++)
        {
            MM_ActivePlayerStatminaValue val = ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>(); //quick ref ->FST
            val.isSelectedDisc = false; //use quick ref ->FST
            val.PlayerCardDetailsObject.SetActive(false); //use quick ref ->FST
        }
        //Comment for MVP task
   /*     for (int i = 0; i < InActiveDiscs.Count; i++)
        {
           InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().isSelectedDisc = false;
           InActiveDiscs[i].GetComponent<MM_InActivePlayerStatminaValue>().PlayerCardDetailsObject.SetActive(false);
        }*/
    }

    //For Revatilazation panel 

    // public void ActiveGS_RevatioliztionPanel ()
    // {
    // 	GS_RevitalizationPanel.SetActive (true);
    // }

    // public void DeActiveGS_RevatioliztionPanel ()
    // {
    // 	GS_RevitalizationPanel.SetActive (false);
    // }

    public void FirstPlayerTap()
    {
        // Assign_Captain();
        formationButton.SetActive(false);
        stadiumButton.SetActive(false);
        SwipeButton.SetActive(false);
        captainButton.SetActive(true);
        playerUpgradeButton.SetActive(true);
    }

    public void SecondPlayerTap()
    {
        formationButton.SetActive(false);
        stadiumButton.SetActive(false);
        SwipeButton.SetActive(true);
        captainButton.SetActive(false);
        playerUpgradeButton.SetActive(false);
    }

    public void NonePlayerTap()
    {
        formationButton.SetActive(true);
        SwipeButton.SetActive(false);
        captainButton.SetActive(false);
        playerUpgradeButton.SetActive(false);
      //  if (GameManager.Instance.IsShopFormation && !GameManager.Instance.IsRematchOneNOne)
      //      stadiumButton.SetActive(true);
      //  else
        //    stadiumButton.SetActive(false);
    }

    public void Assign_Captain()
    {
        for (int i = 0; i < Instance.ActiveDiscs.Count; i++)
        {
            MM_ActivePlayerStatminaValue val = Instance.ActiveDiscs[i].GetComponent<MM_ActivePlayerStatminaValue>(); //quick ref ->FST
            if (val.isSelectedDisc)//Use quick ref ->FST
            {
                Instance.FirstPlayerTap();
                val.Captain.gameObject.SetActive(true);//Use quick ref ->FST
            }
            //val.enabled = true;//Use quick ref ->FST
        }
    }
}