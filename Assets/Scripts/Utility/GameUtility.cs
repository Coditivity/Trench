using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameUtility : MonoBehaviour {



    public static void PauseEditor()
    {
        EditorApplication.isPaused = true;
    }
}
