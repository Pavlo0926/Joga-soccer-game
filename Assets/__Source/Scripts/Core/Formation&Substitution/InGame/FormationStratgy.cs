 using System.Collections;
using System.Collections.Generic;
 using UnityEngine;
 using DG.Tweening;

public class FormationStratgy : MonoBehaviour
{/*
    //public RectTransform 
    public Transform AtkparentObject;
    public Transform DefparentObject;
    public Transform BalparentObject;

    public GameObject AtkformationObject;
    public GameObject DefformationObject;
    public GameObject BalformationObject;

    public RectTransform AttackPanel, DefencePanel, BalancePanel;

    public GameObject[] Active_Btn_sprite;

    public static FormationStratgy instance;


    // private AssignFormationData m_formationdata = null;
    // private AssignFormationData formationdata { get { if (!m_formationdata) m_formationdata = AssignFormationData.Instance; return m_formationdata; } }

    // public List<FormationData> formationsAttack = new List<FormationData>();
    // public List<FormationData> formationsDefence = new List<FormationData>();
    // public List<FormationData> formationsBalance = new List<FormationData>();

    // Use this for initialization
    void Awake()
    {
        if (!instance)
            instance = this;

        AssignFormationDefence();
        AssignFormationAttack();
        AssignFormationBalanced();
        Active_Btn_sprite[0].SetActive(true);   //def
        Active_Btn_sprite[1].SetActive(false);  //attack			
        Active_Btn_sprite[2].SetActive(false);  //bal	
    }
    void Start()
    {
        // WebServicesHandler.SharedInstance.GetAllFormation(PlayerPrefs.GetString("user_id"));

    }
    public void AssignFormationAttack()
    {
        // SelectBtn.SetActive(false);
        // attack.Select();
        // attack.OnSelect(null);
        Active_Btn_sprite[0].SetActive(false);  //def
        Active_Btn_sprite[1].SetActive(true);  //attack			
        Active_Btn_sprite[2].SetActive(false);  //bal

        for (int i = 0; i < AtkparentObject.childCount; i++)
        {
            AtkparentObject.GetChild(i).gameObject.SetActive(false);//used the pooled object ->FST
        }
        DefencePanel.DOAnchorPos(new Vector2(-1200, 0), 0.25f, true);
        AttackPanel.DOAnchorPos(new Vector2(0, 0), 0.25f, true);
        BalancePanel.DOAnchorPos(new Vector2(1200, 0), 0.25f);

        for (int i = 0; i < formationdata.formationsAttack.Count; i++) // AS for new server //As per MVP task list
        {
            GameObject gm;
            if (AtkparentObject.childCount <= i)//add to pool if we dont have gm spawned already ->FST
                gm = Instantiate(AtkformationObject, Vector3.zero, Quaternion.identity) as GameObject;
            else gm = AtkparentObject.GetChild(i).gameObject;//used the pooled object ->FST
            gm.SetActive(true);
            gm.name = "Formation" + i;
            gm.transform.SetParent(AtkparentObject);
            gm.transform.localScale = Vector3.one;
            RectTransform gmrect = gm.GetComponent<RectTransform>();
            gmrect.anchoredPosition3D = Vector3.zero;
            FormationSelection gmformation = gm.GetComponent<FormationSelection>();
            gmformation.formation_index = formationdata.formationsAttack[i].Index_no;
            gmformation.formationName = formationdata.formationsAttack[i].formationName;
            gmformation.formationid = (formationdata.formationsAttack[i].id);
            gmformation.point = formationdata.formationsAttack[i].point;
            gmformation.pointType = formationdata.formationsAttack[i].pointType;
            gmformation.activeFormation = formationdata.formationsAttack[i].isActive;
            gmformation.statusPurchase = formationdata.formationsAttack[i].purchaseStatus;
            gmformation.formationType = formationdata.formationsAttack[i].formationType;
            gmformation.myDisk = new List<PositionOfDisk>();
            gmformation.myUIDisk = new List<UIPositionOfDisk>();
            for (int j = 0; j < formationdata.formationsAttack[i].posDisc.Count; j++)
            {
                PositionOfDisk POD = new PositionOfDisk();
                POD.name = formationdata.formationsAttack[i].posDisc[j].Name;
                POD.myPosX = formationdata.formationsAttack[i].posDisc[j].posX;
                POD.myPosY = formationdata.formationsAttack[i].posDisc[j].posY;
                gmformation.myDisk.Add(POD);

                //As per MVP task list
                UIPositionOfDisk UPD = new UIPositionOfDisk();
                UPD.name = formationdata.formationsAttack[i].posUIDisc[j].Name;
                UPD.myPosX = formationdata.formationsAttack[i].posUIDisc[j].posX;
                UPD.myPosY = formationdata.formationsAttack[i].posUIDisc[j].posY;
                //UPD.myPosZ = formationsAttack [i].posUIDisc [j].posZ;
                gmformation.myUIDisk.Add(UPD);
            }
        }
    }
    public void AssignFormationDefence()
    {
        Active_Btn_sprite[0].SetActive(true);   //def
        Active_Btn_sprite[1].SetActive(false);  //attack			
        Active_Btn_sprite[2].SetActive(false);  //bal
                                                // SelectBtn.SetActive(false);
                                                // defence.Select();
                                                // defence.OnSelect(null);

        for (int i = 0; i < DefparentObject.childCount; i++)
            DefparentObject.GetChild(i).gameObject.SetActive(false);//used the pooled object ->FST

        DefencePanel.DOAnchorPos(new Vector2(0, 0), 0.25f, true);
        AttackPanel.DOAnchorPos(new Vector2(1200, 0), 0.25f, true);
        BalancePanel.DOAnchorPos(new Vector2(1200, 0), 0.25f);
        for (int i = 0; i < formationdata.formationsDefence.Count; i++) // AS for new server //As per MVP task list
        {
            GameObject gm;
            if (DefparentObject.childCount <= i)//add to pool if we dont have gm spawned already ->FST
                gm = Instantiate(DefformationObject, Vector3.zero, Quaternion.identity) as GameObject;
            else gm = DefparentObject.GetChild(i).gameObject;//used the pooled object ->FST
            gm.SetActive(true);
            gm.name = "Formation" + i;
            gm.transform.SetParent(DefparentObject);
            gm.transform.localScale = Vector3.one;
            RectTransform gmrect = gm.GetComponent<RectTransform>();
            gmrect.anchoredPosition3D = Vector3.zero;

            FormationSelection gmformation = gm.GetComponent<FormationSelection>();
            gmformation.formation_index = formationdata.formationsDefence[i].Index_no;
            gmformation.formationName = formationdata.formationsDefence[i].formationName;
            gmformation.formationid = (formationdata.formationsDefence[i].id);
            gmformation.point = formationdata.formationsDefence[i].point;
            gmformation.pointType = formationdata.formationsDefence[i].pointType;
            gmformation.activeFormation = formationdata.formationsDefence[i].isActive;
            gmformation.statusPurchase = formationdata.formationsDefence[i].purchaseStatus;
            gmformation.formationType = formationdata.formationsDefence[i].formationType;
            gmformation.myDisk = new List<PositionOfDisk>();
            gmformation.myUIDisk = new List<UIPositionOfDisk>();
            for (int j = 0; j < formationdata.formationsDefence[i].posDisc.Count; j++)
            {
                PositionOfDisk POD = new PositionOfDisk();
                POD.name = formationdata.formationsDefence[i].posDisc[j].Name;
                POD.myPosX = formationdata.formationsDefence[i].posDisc[j].posX;
                POD.myPosY = formationdata.formationsDefence[i].posDisc[j].posY;
                gmformation.myDisk.Add(POD);
                //As per MVP task list
                UIPositionOfDisk UPD = new UIPositionOfDisk();
                UPD.name = formationdata.formationsDefence[i].posUIDisc[j].Name;
                UPD.myPosX = formationdata.formationsDefence[i].posUIDisc[j].posX;
                UPD.myPosY = formationdata.formationsDefence[i].posUIDisc[j].posY;
                //UPD.myPosZ = formationsDefence [i].posUIDisc [j].posZ;
                gmformation.myUIDisk.Add(UPD);
            }


        }

    }

    public void AssignFormationBalanced()
    {
        Active_Btn_sprite[0].SetActive(false);  //def
        Active_Btn_sprite[1].SetActive(false);  //attack			
        Active_Btn_sprite[2].SetActive(true);   //bal
                                                // SelectBtn.SetActive(false);
                                                // balance.Select();
                                                // balance.OnSelect(null);
        for (int i = 0; i < BalparentObject.childCount; i++)
            BalparentObject.GetChild(i).gameObject.SetActive(false);//used the pooled object ->FST
        DefencePanel.DOAnchorPos(new Vector2(-1200, 0), 0.25f, true);
        AttackPanel.DOAnchorPos(new Vector2(1200, 0), 0.25f, true);
        BalancePanel.DOAnchorPos(new Vector2(0, 0), 0.25f);
        for (int i = 0; i < formationdata.formationsBalance.Count; i++) // AS for new server //As per MVP task list
        {
            GameObject gm;
            if (BalparentObject.childCount <= i)//add to pool if we dont have gm spawned already ->FST
                gm = Instantiate(BalformationObject, Vector3.zero, Quaternion.identity) as GameObject;
            else gm = BalparentObject.GetChild(i).gameObject;//used the pooled object ->FST
            gm.SetActive(true);
            gm.name = "Formation" + i;
            gm.transform.SetParent(BalparentObject);
            gm.transform.localScale = Vector3.one;
            RectTransform gmrect = gm.GetComponent<RectTransform>();
            gmrect.anchoredPosition3D = Vector3.zero;

            FormationSelection gmformation = gm.GetComponent<FormationSelection>();
            gmformation.formation_index = formationdata.formationsBalance[i].Index_no;
            gmformation.formationName = formationdata.formationsBalance[i].formationName;
            gmformation.formationid = (formationdata.formationsBalance[i].id);
            gmformation.point = formationdata.formationsBalance[i].point;
            gmformation.pointType = formationdata.formationsBalance[i].pointType;
            gmformation.activeFormation = formationdata.formationsBalance[i].isActive;
            gmformation.statusPurchase = formationdata.formationsBalance[i].purchaseStatus;
            gmformation.formationType = formationdata.formationsBalance[i].formationType;
            gmformation.myDisk = new List<PositionOfDisk>();
            gmformation.myUIDisk = new List<UIPositionOfDisk>();

            for (int j = 0; j < formationdata.formationsBalance[i].posDisc.Count; j++)
            {
                PositionOfDisk POD = new PositionOfDisk();
                POD.name = formationdata.formationsBalance[i].posDisc[j].Name;
                POD.myPosX = formationdata.formationsBalance[i].posDisc[j].posX;
                POD.myPosY = formationdata.formationsBalance[i].posDisc[j].posY;
                gmformation.myDisk.Add(POD);

                //As per MVP task list
                UIPositionOfDisk UPD = new UIPositionOfDisk();
                UPD.name = formationdata.formationsBalance[i].posUIDisc[j].Name;
                UPD.myPosX = formationdata.formationsBalance[i].posUIDisc[j].posX;
                UPD.myPosY = formationdata.formationsBalance[i].posUIDisc[j].posY;
                //UPD.myPosZ = formationsBalance [i].posUIDisc [j].posZ;
                gmformation.myUIDisk.Add(UPD);
            }
        }
    }*/
}