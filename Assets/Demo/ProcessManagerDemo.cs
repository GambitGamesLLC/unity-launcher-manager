#region IMPORTS

using UnityEngine;
using System.Collections.Generic;


#if GAMBIT_LAUNCHER
using gambit.process;
#endif

#endregion


namespace Gambit.ProcessManagerDemo
{

    /// <summary>
    /// Used to test Process Manager functionality
    /// </summary>
    public class ProcessManagerDemo: MonoBehaviour
    {

        #region PUBLIC - VARIABLES

        /// <summary>
        /// Path to the executable to launch as a new process
        /// </summary>
        public string path = "";

        /// <summary>
        /// Dictionary of arguments to pass into the process, this list is for the keys
        /// </summary>
        public List<string> argumentKeys = new List<string>();

        /// <summary>
        /// Dictionary of arguments to pass into the process, this list is for the values
        /// </summary>
        public List<string> argumentValues = new List<string>();

        /// <summary>
        /// Launcher System that handles interactions with the other process
        /// </summary>
        private ProcessManager.ProcessSystem system = null;

        #endregion

        #region PRIVATE - START

        /// <summary>
        /// Unity lifecyce method
        /// </summary>
        //-----------------------------//
        private void Start()
        //-----------------------------//
        {
            CreateLauncherManager();

        } //END Start Method

        #endregion

        #region PRIVATE - CREATE LAUNCHER MANAGER

        /// <summary>
        /// Launches the process
        /// </summary>
        //---------------------------------//
        private void CreateLauncherManager()
        //---------------------------------//
        {

            ProcessManager.Create
            (
                //OPTIONS
                new ProcessManager.Options()
                {
                    showDebugLogs = true,
                    path = path,
                    argumentKeys = argumentKeys,
                    argumentValues = argumentValues
                },

                //ON SUCCESS
                ( ProcessManager.ProcessSystem _system ) =>
                {
                    system = _system;
                    LaunchProcess();
                },

                //ON FAILED
                ( string error ) =>
                {
                    Debug.LogError( error );
                },

                //ON STATE UPDATE
                ( ProcessManager.ProcessSystem _system, ProcessManager.State state ) =>
                {
                    if( _system.options.showDebugLogs ) Debug.Log( "State Updated : " + state.ToString() );
                }
            );

        } //END CreateLauncherManager

        #endregion

        #region PRIVATE - LAUNCH PROCESS

        /// <summary>
        /// Launches the process
        /// </summary>
        //---------------------------------//
        private void LaunchProcess()
        //---------------------------------//
        {

            ProcessManager.LaunchProcess( system );

        } //END LaunchProcess

        #endregion

    } //END LauncherDemo Class

} //END LauncherDemo Namespace