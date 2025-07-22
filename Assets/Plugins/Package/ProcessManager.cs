/*********************************************************
 * ProcessManager.cs
 * 
 * Singleton manager used to start, stop, and send command line arguments with another application process
 * 
 **********************************************************/

#region IMPORTS

using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

#if GAMBIT_STATIC_COROUTINE
using gambit.staticcoroutine;
#endif

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

namespace gambit.process
{

    /// <summary>
    /// Singleton Manager for starting, stopping, and sending command line arguments to application processes
    /// </summary>
    public class ProcessManager: Singleton<ProcessManager>
    {

        #region PUBLIC - RETURN CLASS - LAUNCHER SYSTEM

        /// <summary>
        /// Process System generated when Create() is successfully called. Contains values important for future modification and communication with the Process Manager and the child process
        /// </summary>
        //-----------------------------------------//
        public class ProcessSystem
        //-----------------------------------------//
        {
            /// <summary>
            /// The options passed in during Create()
            /// </summary>
            public Options options = new Options();

            /// <summary>
            /// The current state of the Process system
            /// </summary>
            public State state = State.NotRunning;

            /// <summary>
            /// Unity action to call when the state of the process changes
            /// </summary>
            public Action<ProcessSystem, State> OnStateUpdate;

            /// <summary>
            /// The Process for the executable are launching
            /// </summary>
            public Process process = null;

        } //END ProcessSystem

        #endregion

        #region PUBLIC - CREATION OPTIONS

        /// <summary>
        /// Options object you can pass in to customize the spawned system
        /// </summary>
        //---------------------------------------------//
        public class Options
        //---------------------------------------------//
        {
            /// <summary>
            /// Should debug logs be printed to the console log?
            /// </summary>
            public bool showDebugLogs = true;

            /// <summary>
            /// The full path to the process you want to launch
            /// </summary>
            public string path = "";

            /// <summary>
            /// Optional command line arguments you want to pass into the process.
            /// This list contains the keys to the values
            /// Do not include the '-' you normally would include with a command line argument.
            /// Be aware that the process will need to respond to these message by listening for them.
            /// You can use the ReadArgumentKeys() function of this script to return these values to you. 
            /// Simply include this package in your child app and use the ProcessManager.ReadArgumentKeys() function to get this List<string> back.
            /// </summary>
            public List<string> argumentKeys = new List<string>();

            /// <summary>
            /// Optional command line arguments you want to pass into the process.
            /// This list contains the values
            /// Be aware that the process will need to respond to these message by listening for them.
            /// You can use the ReadArgumentValues() function of this script to return these values to you. 
            /// Simply include this package in your child app and use the ProcessManager.ReadArgumentValues() function to get this List<string> back.
            /// </summary>
            public List<string> argumentValues = new List<string>();

        } //END Options

        #endregion

        #region PUBLIC - ENUM STATE

        /// <summary>
        /// Possible states of the ProcessSystem
        /// </summary>
        //-----------------------//
        public enum State
        //-----------------------//
        {
        
            NotRunning,
            Updating,
            Running

        } //END State Enum

        #endregion

        #region PUBLIC - CREATE

        /// <summary>
        /// Creates a ProcessSystem and returns it.
        /// Used to Start, Stop, and listen to state changes with a child Process
        /// The path location must lead to an existing executable
        /// </summary>
        /// <param name="options">Options object that determines the settings used to Start, Stop, and send arguments to the process</param>
        /// <param name="OnSuccess">Callback action when the Process system successfully initializes</param>
        /// <param name="OnFailed">Callback action that returns a string with an error message when process initialization fails</param>
        /// <param name="OnStateUpdate">Callback action called when the process changes state</param>
        //----------------------------------//
        public static void Create
        (
            Options options = null,
            Action<ProcessSystem> OnSuccess = null,
            Action<string> OnFailed = null,
            Action<ProcessSystem, State> OnStateUpdate = null
        )
        //----------------------------------//
        {
#if !GAMBIT_STATIC_COROUTINE
            OnFailed?.Invoke( "ProcessManager.cs Create() missing GAMBIT_STATIC_COROUTINE scripting define symbol and/or package. Unable to continue." );
            return;
#endif

            if(options == null)
            {
                OnFailed?.Invoke( "ProcessManager.cs Create() options was passed in as null. Unable to continue." );
                return;
            }

            if(string.IsNullOrEmpty( options.path ))
            {
                OnFailed?.Invoke( "ProcessManager.cs Create() options.path is null or empty. Unable to continue." );
                return;
            }

            if( !File.Exists( options.path ))
            {
                OnFailed?.Invoke( "ProcessManager.cs Create() unable to locate file at options.path. Unable to continue." );
                return;
            }

            ProcessSystem system = new ProcessSystem();
            system.options = options;
            system.OnStateUpdate = OnStateUpdate;

            SetState( system, State.NotRunning );

            OnSuccess?.Invoke( system );

        } //END Create Method

        #endregion

        #region PRIVATE - SET STATE

        /// <summary>
        /// Changes the state of the process system and sends out a message
        /// </summary>
        /// <param name="system"></param>
        //----------------------------------------------------------------------//
        private static void SetState( ProcessSystem system, State state )
        //----------------------------------------------------------------------//
        {
            if(system == null)
            {
                return;
            }

            system.state = state;

            system.OnStateUpdate?.Invoke( system, state );

        } //END SetState

        #endregion

        #region PUBLIC - LAUNCH PROCESS

        /// <summary>
        /// Using the path to the executable, launches a new process an submits any command line arguments in our options
        /// </summary>
        /// <param name="system"></param>
        //------------------------------------------------------------------//
        public static void LaunchProcess
        ( 
            ProcessSystem system,
            Action OnLaunchSuccess = null,
            Action<string> OnLaunchFailed = null
        )
        //------------------------------------------------------------------//
        {
#if !GAMBIT_STATIC_COROUTINE
            OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() missing GAMBIT_STATIC_COROUTINE scripting define symbol and/or package. Unable to continue." );
            return;
#endif

            if( system == null )
            {
                OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() passed in system is null. Unable to continue." );
                return;
            }

            if(system.options == null)
            {
                OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() system.options is null. Unable to continue." );
                return;
            }

            if( string.IsNullOrEmpty( system.options.path ) )
            {
                OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() system.options.path is null or empty. Unable to continue." );
                return;
            }

            if(!File.Exists( system.options.path ))
            {
                OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() Executable file at path does not exist. Unable to continue." );
                return;
            }

            if( system.state == State.Running || system.state == State.Updating )
            {
                OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() already running process, Unable to continue." );
                return;
            }

            if(system.options.showDebugLogs)
            {
                UnityEngine.Debug.Log( "ProcessManager.cs LaunchProcess() calling Process.Start() for process at path = " + system.options.path );
            }

            try
            {
                string arguments = ConvertArgumentListsToString( system.options.argumentKeys, system.options.argumentValues );

                if(system.options.showDebugLogs)
                {
                    UnityEngine.Debug.Log( arguments );
                }

                system.process = Process.Start( system.options.path, arguments );

                SetState( system, State.Running );

#if GAMBIT_STATIC_COROUTINE
                StaticCoroutine.Start( TrackProcess( system ) );
#endif

                OnLaunchSuccess?.Invoke();
            }
            catch( Exception e ) 
            {
                OnLaunchFailed?.Invoke( "ProcessManager.cs LaunchProcess() Error while launching process = " + e.ToString() );
                return;
            }

        } //END LaunchProcess Method

        #endregion

        #region PUBLIC - CONVERT ARGUMENT LISTS TO STRING

        /// <summary>
        /// Converts two List<string>, one for the keys, and one of the values that will be passed
        /// into a process as a single argument string during Process.Start()
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        //------------------------------------------------------------------------------------------------//
        private static string ConvertArgumentListsToString( List<string> argumentKeys, List<string> argumentValues )
        //------------------------------------------------------------------------------------------------//
        {
            if(argumentKeys == null || argumentValues == null)
            {
                //Debug.LogError( "Argument keys or values list is null." );
                return "";
            }

            if(argumentKeys.Count != argumentValues.Count)
            {
                //Debug.LogError( "Argument keys and values lists must have the same number of elements." );
                return "";
            }

            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < argumentKeys.Count; i++)
            {
                string key = argumentKeys[ i ];
                string value = argumentValues[ i ];

                if(string.IsNullOrEmpty( key ))
                {
                    //Debug.LogWarning( $"Empty key at index {i}. Skipping." );
                    continue;
                }

                if(key.StartsWith( "-" ))
                {
                    UnityEngine.Debug.LogError( $"Key '{key}' at index {i} starts with a '-'." );
                    continue; // Skip this argument
                }

                stringBuilder.Append( "-" + key );

                if(!string.IsNullOrEmpty( value ))
                {
                    stringBuilder.Append( " " );
                    stringBuilder.Append( value );
                }
                stringBuilder.Append( " " ); // Add a space to separate arguments
            }

            // Remove the trailing space if there are arguments
            if(stringBuilder.Length > 0)
            {
                stringBuilder.Length--;
            }

            return stringBuilder.ToString();

        } //END ConvertArgumentListsToString

        #endregion

        #region PRIVATE - TRACK PROCESS

        /// <summary>
        /// Checks to see if the state of the process changes and responds to those events
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        //-----------------------------------------------------------------------------------------//
        private static System.Collections.IEnumerator TrackProcess( ProcessSystem system )
        //-----------------------------------------------------------------------------------------//
        {
            while(!system.process.HasExited)
            {
                yield return null; // Wait until the process has been exited
            }

            if( system.process.HasExited )
            {
                //End the process
                system.process.Close();
                system.process = null;

                SetState( system, State.NotRunning );
            }
            
        } //END TrackProcess Method

        #endregion

        #region PUBLIC - READ ARGUMENT KEYS

        /// <summary>
        /// Returns a List<string> of command line argument keys that were passed into this process when it started
        /// It assumes each key starts with a '-' delimiter
        /// It assumes each value comes after a key
        /// </summary>
        /// <returns></returns>
        //---------------------------------------//
        public static List<string> ReadArgumentKeys()
        //---------------------------------------//
        {

            //Use the string[] return by this Method to pull a List<string> of all the keys we previously added.
            //Each key will start with a '-' delimiter
            //Values follow keys, seperated by a space ' '
            //Key/Value pairs are also seperated by a 'space'

            return ReadArgumentKeys( System.Environment.GetCommandLineArgs() );

        } //END ReadArgumentKeys

        //---------------------------------------//
        /// <summary>
        /// Returns a List<string> of command line argument keys that were passed into this process when it started
        /// It assumes each key starts with a '-' delimiter
        /// It assumes each value comes after a key
        /// </summary>
        /// <returns></returns>
        public static List<string> ReadArgumentKeys( string[] args )
        //---------------------------------------//
        {
            List<string> result = new List<string>();

            if(args != null && args.Length > 0)
            {
                for(int i = 1; i < args.Length; i++) // Start from index 1 to skip the executable path
                {
                    if(args[ i ].StartsWith( "-" ))
                    {
                        result.Add( args[ i ] );
                    }
                    // If the current argument doesn't start with '-', it might be a value
                    // associated with the previous key, so we skip it in the key collection.
                }
            }

            return result;

        } //END ReadArgumentKeys

        #endregion

        #region PUBLIC - READ ARGUMENT VALUES

        /// <summary>
        /// Returns a List<string> of command line argument values that were passed into this process when it started.
        /// It assumes that values directly follow their corresponding keys.
        /// </summary>
        /// <returns></returns>
        //---------------------------------------//
        public static List<string> ReadArgumentValues()
        //---------------------------------------//
        {
            return ReadArgumentValues( System.Environment.GetCommandLineArgs() );

        } //END ReadArgumentValues

        /// <summary>
        /// Returns a List<string> of command line argument values that were passed into this process when it started.
        /// It assumes that values directly follow their corresponding keys.
        /// </summary>
        /// <returns></returns>
        //---------------------------------------//
        public static List<string> ReadArgumentValues( string[ ] args )
        //---------------------------------------//
        {
            List<string> result = new List<string>();

            if(args != null && args.Length > 1) // Ensure there are arguments beyond the executable path
            {
                for(int i = 1; i < args.Length; i++)
                {
                    // If the current argument does NOT start with '-', it's considered a value
                    if(!args[ i ].StartsWith( "-" ))
                    {
                        result.Add( args[ i ] );
                    }
                    // If it does start with '-', it's a key, and the subsequent argument (if it exists)
                    // would be its value. The next iteration will handle that.
                }
            }

            return result;

        } //END ReadArgumentValues

        #endregion

        #region PUBLIC - DESTROY

        /// <summary>
        /// Destroys a process system, cleaning up an variables, timers, callback handlers, etc
        /// </summary>
        /// <param name="system"></param>
        //------------------------------------------------------------------//
        public static void Destroy( ProcessSystem system )
        //------------------------------------------------------------------//
        {
            if(system == null)
            {
                return;
            }

            system.process = null;
            system = null;
        
        } //END Destroy Method

        #endregion

    } //END ProcessManager Class

} //END gambit.process Namespace