/*********************************************************
 * LauncherManager.cs
 * 
 * Singleton manager used to update, launch, and communicate with child Unity3D apps
 * 
 **********************************************************/

#region IMPORTS

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering.VirtualTexturing;




#if GAMBIT_SINGLETON
using gambit.singleton;
#else
/// <summary>
/// Fallback Singleton base class if GAMBIT_SINGLETON is not defined.
/// </summary>
/// <typeparam name="T">Type of the MonoBehaviour singleton.</typeparam>
public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    /// <summary>
    /// Gets the singleton instance, creating it if necessary.
    /// </summary>
    //---------------------------------------------//
    public static T Instance
    //---------------------------------------------//
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject( typeof( T ).Name ).AddComponent<T>();
                GameObject.DontDestroyOnLoad( instance.gameObject );
            }
            return instance;
        }
    }

} //END Singleton<T> class
#endif

#endregion

namespace gambit.launcher
{

    /// <summary>
    /// Singleton Manager for updating, launching, and communicating with Unity3D child apps
    /// </summary>
    public class LauncherManager: Singleton<LauncherManager>
    {

        #region PUBLIC - VARIABLES

        #endregion

        #region PRIVATE - VARIABLES

        #endregion

        #region PUBLIC - START

        #endregion

        #region PUBLIC - UPDATE

        #endregion

        #region PUBLIC - RETURN CLASS - LAUNCHER SYSTEM

        /// <summary>
        /// Launcher System generated when Create() is successfully called. Contains values important for future modification and communication with the Launcher Manager and the child app
        /// </summary>
        //-----------------------------------------//
        public class LauncherSystem
        //-----------------------------------------//
        {
            /// <summary>
            /// The options passed in during Create()
            /// </summary>
            public Options options = new Options();

            /// <summary>
            /// The current state of the Launcher system
            /// </summary>
            public State state = State.NotInitialized;

            /// <summary>
            /// Unity action to call when child app has sent the launcher application a message
            /// </summary>
            public Action<LauncherSystem> OnMessageRecieved;

            /// <summary>
            /// Unity action to call when the state of the launcher system changes
            /// </summary>
            public Action<LauncherSystem, State> OnStateUpdate;

        } //END LauncherSystem

        #endregion

        #region PUBLIC - CREATION OPTIONS

        /// <summary>
        /// Options object you can pass in to customize the spawned NeuroGuide system
        /// </summary>
        //---------------------------------------------//
        public class Options
        //---------------------------------------------//
        {
            /// <summary>
            /// Should debug logs be printed to the console log?
            /// </summary>
            public bool showDebugLogs = true;

        } //END Options

        #endregion

        #region PUBLIC - ENUM STATE

        /// <summary>
        /// Possible states of the LauncherSystem
        /// </summary>
        //-----------------------//
        public enum State
        //-----------------------//
        {
        
            NotInitialized,
            Initialized,
            Updating,
            Running

        } //END State Enum

        #endregion

        #region PUBLIC - CREATE

        /// <summary>
        /// Creates a LauncherSystem and returns it, prepares to update, launch, and communicate with a child Unity3D app
        /// </summary>
        /// <param name="options">Options object that determines the settings used to download, update, launch, and communicate with a child Unity3D app</param>
        /// <param name="OnSuccess">Callback action when the Launcher system successfully initializes</param>
        /// <param name="OnFailed">Callback action that returns a string with an error message when launcher initialization fails</param>
        //----------------------------------//
        public static void Create(
            Options options = null,
            Action<LauncherSystem> OnSuccess = null,
            Action<string> OnFailed = null )
        //----------------------------------//
        {

            if(options == null)
            {
                OnFailed?.Invoke( "LauncherManager.cs Create() options was passed in as null. Unable to continue." );
                return;
            }

        } //END Create Method

        #endregion

        #region PUBLIC - DESTROY

        #endregion

    } //END LauncherManager Class

} //END gambit.launcher Namespace