#region IMPORTS

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace gambit.process.demo
{

    /// <summary>
    /// Sample test code to see if our command line argument logic works as expected
    /// </summary>
    public class CommandLineArgumentsTest : MonoBehaviour
    {
        #region PRIVATE - VARIABLES

        // Sample command line arguments for testing. Simulates a value coming from System.Environment.GetCommandLineArgs()
        public string[ ] commandLineArguments = new string[ ]
        {
            "path/to/executable.exe", //First value is the executable name and potentially a full path to the process
            "-level",                 //Each key starts with a '-'
            "5",                      //Each key is proceeded by a value
            "-playerName",            
            "Hero",
            "-debug",
            "true",
            "-fullscreen",
            "true"
        };

        #endregion

        #region PUBLIC - START

        /// <summary>
        /// Unity lifecycle method
        /// </summary>
        //-------------------------//
        public void Start()
        //-------------------------//
        {
            TestArgumentParsing( commandLineArguments );

        } //END Start Method

        #endregion

        #region PUBLIC - TEST ARGUMENT PASSING

        /// <summary>
        /// Tests if our command line argument parsing works as expected
        /// </summary>
        /// <param name="argsToTest"></param>
        //----------------------------------------------------------------//
        public static void TestArgumentParsing( string[ ] argsToTest )
        //----------------------------------------------------------------//
        {
            Debug.Log( "--- Testing Argument Parsing ---" );

            Debug.Log( "Simulated Command Line Args:" );

            foreach(string arg in argsToTest)
            {
                Debug.Log( $"- '{arg}'" );
            }

            Debug.Log( "------------------------------" );

            List<string> keys = ProcessManager.ReadArgumentKeys( argsToTest );


            Debug.Log( "Extracted Keys:" );

            if(keys.Count > 0)
            {
                foreach(string key in keys)
                {
                    Debug.Log( $"- '{key}'" );
                }
            }
            else
            {
                Debug.Log( "- No keys found." );
            }

            Debug.Log( "------------------------------" );

            List<string> values = ProcessManager.ReadArgumentValues( argsToTest );

            Debug.Log( "Extracted Values:" );

            if(values.Count > 0)
            {
                foreach(string value in values)
                {
                    Debug.Log( $"- '{value}'" );
                }
            }
            else
            {
                Debug.Log( "- No values found." );
            }

            Debug.Log( "------------------------------" );

        } //END TestArgumentParsing Method

        #endregion

    } //END CommandLineArgumentsTest Class

} //END gambit.process.demo Namespace