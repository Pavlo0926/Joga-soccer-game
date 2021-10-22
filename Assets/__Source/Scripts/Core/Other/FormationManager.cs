using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FormationManager : MonoBehaviour
{


	///*************************************************************************///
	/// Main Formation manager.
	/// You can define new formations here.
	/// To define new positions and formations, please do the following:
	///1. add +1 to formations counter.
	///2. define a new case in "getPositionInFormation" function.
	///3. for all 5 units, define an exact position on Screen. (you can copy a case and edit it's values)
	///4. Note. You always set the units position, as if they are on the left side of the field. 
	///The controllers automatically process the position of the units, if they belong to the right side of the field.
	///*************************************************************************///

	// Available Formations:
	/*
	1-2-2
	1-3-1
	1-2-1-1
	1-4-0
	1-1-1-1-1
	*/

	public static int formations = 5;
	//total number of available formations
	public static float fixedZ = -0.5f;
	//fixed Z position for all units on the selected formation
	public static float yFixer = 0f;
	//if you ever needed to translate all units up or down a little bit, you can do it by
	//tweeking this yFixer variable.
	//*****************************************************************************
	// Every unit reads it's position from this function.
	// Units give out their index and formation and get their exact position.
	//*****************************************************************************

// commented --@sud
	//public static List<DiskPosition> myDiscPos;

	// public static Vector3 getOnlinePositionInFormation (int _UnitIndex, int formation)
	// {
	// 	Vector3 output = Vector3.zero;
		// if (_UnitIndex == 0)
		// 	output = new Vector3 (float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posX.ToString ()), float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posY.ToString ()) + yFixer, fixedZ);
		// if (_UnitIndex == 1)
		// 	output = new Vector3 (float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posX.ToString ()), float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posY.ToString ()) + yFixer, fixedZ);
		// if (_UnitIndex == 2)
		// 	output = new Vector3 (float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posX.ToString ()), float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posY.ToString ()) + yFixer, fixedZ);
		// if (_UnitIndex == 3)
		// 	output = new Vector3 (float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posX.ToString ()), float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posY.ToString ()) + yFixer, fixedZ);
		// if (_UnitIndex == 4)
		// 	output = new Vector3 (float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posX.ToString ()), float.Parse (WebServicesHandler.SharedInstance.formations [formation].posDisc [_UnitIndex].posY.ToString ()) + yFixer, fixedZ);

		// return output;
	// }

	//	public static void getForamtionPositionForPlayer (int formationIndex)
	//	{
	//		for (int i = 0; i < WebServicesHandler.SharedInstance.formations.Count; i++) {
	//
	//			if (WebServicesHandler.SharedInstance.formations [i].id == formationIndex) {
	//
	//				for (int j = 0; j < WebServicesHandler.SharedInstance.formations [i].posDisc.Count; j++) {
	//					PositionOfDisk POD = new PositionOfDisk ();
	//
	//					POD.posX = WebServicesHandler.SharedInstance.formations [i].posDisc [j].posX;
	//					POD.posY = WebServicesHandler.SharedInstance.formations [i].posDisc [j].posY;
	//					myDiscPos.Add (POD);
	//				}
	//			}
	//		}
	//	}


	public static Vector3 GetPositionInFormation (int _formationIndex, int _UnitIndex)
	{
		Vector3 output = Vector3.zero;
//		_formationIndex = 0;
		switch (_formationIndex) {
		case 0:
			//2-0-2
			if (_UnitIndex == 0)
				output = new Vector3 (-13.63f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.13f, 3.77f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-9.13f, -6.89f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.49f, -0.13f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.49f, -3.06f + yFixer, fixedZ);
			break;

		case 1:
			//2-1-1
			if (_UnitIndex == 0)
				output = new Vector3 (-13.63f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.5f, 3.77f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-9.5f, -6.89f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-6.03f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.86f, -1.5f + yFixer, fixedZ);
			break;
			
		case 2:
			//2-3-0
			if (_UnitIndex == 0)
				output = new Vector3 (-10.91f, -4.3f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-10.91f, 1.28f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-5.83f, -1.49f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.83f, 5.12f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-5.83f, -8.29f + yFixer, fixedZ);
			break;
			
		case 3:
			//0-0-5
			if (_UnitIndex == 0)
				output = new Vector3 (-4.6f, -1.49f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-2.31f, 1.88f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.31f, -5.16f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.31f, 4.45f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.31f, -7.7f + yFixer, fixedZ);
			break;
			
		case 4:
			//0-2-2
			if (_UnitIndex == 0)
				output = new Vector3 (-11.48f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.53f, 2.55f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-9.53f, -5.26f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5f, 5.22f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-5f, -8.25f + yFixer, fixedZ);
			break;
		case 5:
			//1-1-2
			if (_UnitIndex == 0)
				output = new Vector3 (-13.82f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.61f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-5.78f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.31f, 2.13f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.31f, -5.35f + yFixer, fixedZ);
			break;
		case 6:
			//0-2-2
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-7.38f, 3.62f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-7.38f, -6.43f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-1.31f, 3.62f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-1.31f, -6.43f + yFixer, fixedZ);
			break;
		case 7:
			//0-0-4
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-3.29f, -0.14f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.29f, -3.09f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-1.31f, 2.23f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-1.31f, -5.4f + yFixer, fixedZ);
			break;
		case 8:
			//0-3-1
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-5.82f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-5.82f, 2.71f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.82f, -5.7f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-1.31f, -5.7f + yFixer, fixedZ);
			break;
		case 9:
			//2-2-0
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-10.21f, 4.77f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-10.21f, -7.85f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-4.52f, 1.66f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-4.52f, -4.5f + yFixer, fixedZ);
			break;
		case 10:
			//0-0-4
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-2.82f, 0.57f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.82f, -3.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.82f, 4.7f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.82f, -7.83f + yFixer, fixedZ);
			break;
		case 11:
			//0-3-1
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-10.57f, 1.5f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-7.72f, -4.92f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.87f, 4.07f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-1.27f, 5.6f + yFixer, fixedZ);
			break;
		case 12:
			//0-3-1-Target
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-8.22f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.89f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-8.22f, 3.24f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-8.22f, -6.27f + yFixer, fixedZ);
			break;
		case 13:
			//2-3-0
			if (_UnitIndex == 0)
				output = new Vector3 (-11.57f, -3.4f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.57f, -0.08f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.33f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.82f, 3.24f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-5.82f, -6.27f + yFixer, fixedZ);
			break;
		case 14:
			//0-2-2
			if (_UnitIndex == 0)
				output = new Vector3 (-13.2f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-6.16f, -0.91f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.67f, -3.83f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-6.16f, 3.24f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.67f, -6.27f + yFixer, fixedZ);
			break;
		case 15:
			//2-2-1
			if (_UnitIndex == 0)
				output = new Vector3 (-11.37f, 5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.37f, -8.33f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-6.77f, -4.91f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-6.77f, 1.65f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.41f, -1.5f + yFixer, fixedZ);
			break;
		case 16:
			//5-0-0
			if (_UnitIndex == 0)
				output = new Vector3 (-11.37f, 3.61f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.37f, -6.8f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-9.04f, -4.45f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-9.04f, 1.5f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-9.04f, -1.5f + yFixer, fixedZ);
			break;
		case 17:
			//1-2-1
			if (_UnitIndex == 0)
				output = new Vector3 (-13.35f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.04f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-5.55f, -5.63f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.55f, 2.51f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.43f, -1.5f + yFixer, fixedZ);
			break;
		case 18:
			//2-1-2
			if (_UnitIndex == 0)
				output = new Vector3 (-9.04f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.04f, 0.91f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-5.55f, -3.88f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.43f, -5.86f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.43f, -1.76f + yFixer, fixedZ);
			break;
		case 19:
			//2-0-3
			if (_UnitIndex == 0)
				output = new Vector3 (-12.55f, -2.86f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-12.55f, -0.45f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.43f, 4.2f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.43f, -7.6f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.43f, -1.76f + yFixer, fixedZ);
			break;
		case 20:
			//1-3-1
			if (_UnitIndex == 0)
				output = new Vector3 (-11.52f, -6.7f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.93f, 1.57f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.43f, 4.2f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-4.22f, -3.82f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-7.1f, -1.06f + yFixer, fixedZ);
			break;
		case 21:
			//4-1-0
			if (_UnitIndex == 0)
				output = new Vector3 (-10.59f, -5.01f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-10.33f, 1.21f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-4.52f, 1.82f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-13.03f, -1.91f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-7.57f, -1.53f + yFixer, fixedZ);
			break;
		case 22:
			//5-0-0
			if (_UnitIndex == 0)
				output = new Vector3 (-12.44f, -1.53f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-12.44f, 1.09f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-12.44f, -4.22f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-9.12f, -2.77f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-9.12f, 0f + yFixer, fixedZ);
			break;
		case 23:
			//0-4-0
			if (_UnitIndex == 0)
				output = new Vector3 (-13.17f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-8.5f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.7f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-8.5f, 1.02f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.7f, 1.02f + yFixer, fixedZ);
			break;
		case 24:
			//1-3-1
			if (_UnitIndex == 0)
				output = new Vector3 (-12.12f, -7.71f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-9.83f, -4.96f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-7.37f, -2.01f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-4.96f, 1.02f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.64f, 4.07f + yFixer, fixedZ);
			break;
		case 25:
			//4-0-0
			if (_UnitIndex == 0)
				output = new Vector3 (-13.05f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-8.47f, -3.72f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-8.47f, 0.91f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-8.47f, -7.43f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-8.47f, 4.37f + yFixer, fixedZ);
			break;
		case 26:
			//0-2-3
			if (_UnitIndex == 0)
				output = new Vector3 (-2.62f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-4.56f, -3.21f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-4.56f, 0.16f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-8.85f, -7.91f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-8.85f, 4.54f + yFixer, fixedZ);
			break;
		case 27:
			//1-3-0
			if (_UnitIndex == 0)
				output = new Vector3 (-6.6f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-4.6f, -3.21f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-4.6f, 0.16f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-10.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-13.48f, -1.5f + yFixer, fixedZ);
			break;
		case 28:
			//2-1-2
			if (_UnitIndex == 0)
				output = new Vector3 (-11.77f, -3.08f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.77f, 0.18f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-6.58f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.3f, 2.82f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-2.3f, -6.27f + yFixer, fixedZ);
			break;
		case 29:
			//0-5-0
			if (_UnitIndex == 0)
				output = new Vector3 (-8.2f, -4.25f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-8.2f, 1.14f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-6.72f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.93f, 4.09f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-5.93f, -7.19f + yFixer, fixedZ);
			break;
		case 30:
			//2-0-3
			if (_UnitIndex == 0)
				output = new Vector3 (-12.45f, -2.74f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-12.45f, -0.37f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-3.19f, 1.5f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.19f, -4.55f + yFixer, fixedZ);
			break;
		case 31:
			//3-1-0
			if (_UnitIndex == 0)
				output = new Vector3 (-13.55f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-10.53f, -3.08f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-8.51f, -0.98f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-6.5f, 1.31f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-7.75f, -4.66f + yFixer, fixedZ);
			break;
		case 32:
			//2-2-1
			if (_UnitIndex == 0)
				output = new Vector3 (-12.48f, -2.8f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-12.48f, 0.15f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-7.27f, -2.8f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-7.27f, 0.15f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.61f, -1.5f + yFixer, fixedZ);
			break;
		case 33:
			//0-2-2
			if (_UnitIndex == 0)
				output = new Vector3 (-8.77f, -3.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-8.77f, 0.45f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.63f, -3.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-3.63f, 0.45f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-13.1f, -1.5f + yFixer, fixedZ);
			break;
		case 34:
			//4-0-1
			if (_UnitIndex == 0)
				output = new Vector3 (-11.09f, -2.71f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.09f, -0.14f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-12.73f, -4.64f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-12.73f, 1.84f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-1.97f, -1.5f + yFixer, fixedZ);
			break;
		case 35:
			//2-1-2
			if (_UnitIndex == 0)
				output = new Vector3 (-11.09f, -7f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.09f, -4.43f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-2.51f, 1.04f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.51f, 3.45f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-6.68f, -1.5f + yFixer, fixedZ);
			break;
		case 36:
			//3-1-1
			if (_UnitIndex == 0)
				output = new Vector3 (-10.38f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.97f, 0.54f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-11.97f, -3.5f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-2.51f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-6.68f, -1.5f + yFixer, fixedZ);
			break;
		case 37:
			//4-0-1
			if (_UnitIndex == 0)
				output = new Vector3 (-9.52f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-11.84f, -0.12f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-11.84f, -2.78f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-11.84f, 3.47f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.53f, -6.88f + yFixer, fixedZ);
			break;
		case 38:
			//0-2-3
			if (_UnitIndex == 0)
				output = new Vector3 (-3.78f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-3.78f, 1.06f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.78f, 3.68f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-5.73f, -3.65f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-8.35f, -3.65f + yFixer, fixedZ);
			break;
		case 39:
			//3-0-2
			if (_UnitIndex == 0)
				output = new Vector3 (-12.54f, -4.13f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-12.54f, -1.57f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-12.54f, 1.05f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-4.25f, 4.65f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-4.25f, -7.6f + yFixer, fixedZ);
			break;
		case 40:
			//2-0-3
			if (_UnitIndex == 0)
				output = new Vector3 (-10.34f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-12.72f, -0.35f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-3.38f, -3.6f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-3.38f, -6.19f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.38f, -8.75f + yFixer, fixedZ);
			break;
		case 41:
			//0-2-2
			if (_UnitIndex == 0)
				output = new Vector3 (-13.19f, -1.5f + yFixer, fixedZ);
			if (_UnitIndex == 1)
				output = new Vector3 (-8.3f, 0.81f + yFixer, fixedZ);
			if (_UnitIndex == 2)
				output = new Vector3 (-8.3f, -3.74f + yFixer, fixedZ);
			if (_UnitIndex == 3)
				output = new Vector3 (-3.4f, 3f + yFixer, fixedZ);
			if (_UnitIndex == 4)
				output = new Vector3 (-3.4f, -6.34f + yFixer, fixedZ);
			break;
		}
		
		return output;
	}
}

