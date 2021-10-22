// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System;
// using UnityEngine.EventSystems;

// namespace UnityEngine.UI.Extensions
// {
//     [RequireComponent(typeof(Button))]
//     public class TutorialPanelScript : MonoBehaviour, IScrollSnap
//     {
//         public int _currentPage;
//         public int _previousPage;
//         public int startPage;
//         private int _startingPage = 0;
//         private int _screens = 1;

//         internal float _scrollStartPosition;
//         internal float _childSize;
//         private float _childPos, _maskSize;
//         internal Vector2 _childAnchorPoint;
//         internal ScrollRect _scroll_rect;
//         private Transform _listContainerTransform;
//         internal Vector3 _lerp_target;
//         private Vector3 _lerpTarget;
//         internal Vector3[] PageAnchor;
//         internal bool _lerp;
//         private int _pages;

//         private float _listContainerMinPosition;

//         private float _listContainerMaxPosition;

//         private float _listContainerSize;

//         private RectTransform _listContainerRectTransform;

//         private Vector2 _listContainerCachedSize;

//         private float _itemSize;
//         public GameObject[] childPanel;
//         private Vector3[] _pageAnchorPositions;


//         public void NextScreen()
//         {

//             {
//                 int targetPage = _pages - 1;
//                 _lerp = true;

//                 _lerpTarget = _pageAnchorPositions[CurrentPage() + 1];

//                 PageChanged(targetPage);
//             }
//         }
//         private void PageChanged(int currentPage)
//         {
//             _startingPage = currentPage;



//         }

//         public int CurrentPage()
//         {
//             float pos;
//             pos = _listContainerMaxPosition - _listContainerTransform.localPosition.x;
//             pos = Mathf.Clamp(pos, 0, _listContainerSize);
//             float page = pos / _itemSize;
//             return Mathf.Clamp(Mathf.RoundToInt(page), 0, _pages);

//         }

//         //     //ublic GameObject parent;
//         //     public ScrollRect parent;
//         //     public Vector3 position;
//         //    // public Scrollbar Target;
//         //    // public Button TheOtherButton;
//         //    public Button next;
//         //    public GameObject[] panel;
//         //    // public float Step = 0f;
//         //  void Awake()
//         //  {
//         //     // Target.value = 0f;

//         //     // Target.transform.position = Vector2.zero;
//         //  }
//         //  void Start()
//         //  {
//         //    // parent.GetComponent<Transform>().position= Vector2.zero;
//         //     // position  = parent.GetComponent<Transform>().position.ToString();
//         //     // Target.value = 0f;
//         //     // Step= 0.3325f;
//         //  }

//         //     public void Increment() 
//         //     {
//         //       //  if (Target == null ) throw new Exception("Setup ScrollbarIncrementer first!");
//         //         for(int i =0;i<panel.Length;i++ )
//         //         {
//         //         panel[i].transform.localPosition = parent.transform.localPosition;
//         //         }
//         //         // Target.value = Mathf.Clamp(Target.value + Step, 0, 1);
//         //         // if(Target.value != 1)
//         //         // next.interactable = false;
//         //         // else 
//         //         // next.interactable = true;
//         //         //next.interactable = Target.value  < 1;
//         //        // TheOtherButton.interactable = true;
//         //     }

//         //     public void Decrement()
//         //     {
//         //         // if (Target == null ) throw new Exception("Setup ScrollbarIncrementer first!");
//         //         // Target.value = Mathf.Clamp(Target.value - Step, 0, 1);
//         //       //  GetComponent<Button>().interactable = Target.value != 0;
//         //        // TheOtherButton.interactable = true;
//         //     }
//     }
// }