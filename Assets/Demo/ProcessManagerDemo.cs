#region IMPORTS

using UnityEngine;
using System.Collections.Generic;


#if GAMBIT_PROCESS
using gambit.process;
#endif

#endregion


namespace gambit.process.demo
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
            CreateProcessManager();

        } //END Start Method

        #endregion

        #region PRIVATE - CREATE PROCESS MANAGER

        /// <summary>
        /// Creates the process manager based on our options, then immediately launches the process
        /// </summary>
        //---------------------------------//
        private void CreateProcessManager()
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

        } //END CreateProcessManager

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

    } //END ProcessDemo Class

} //END gambit.process.demo Namespace