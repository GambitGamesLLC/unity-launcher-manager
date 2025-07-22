#region IMPORTS

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace gambit.process
{

    /// <summary>
    /// Sample test code to see if our command line argument logic works as expected
    /// </summary>
    public class CommandLineArgumentsTest : MonoBehaviour
    {
        #region PRIVATE - VARIABLES

        // Sample command line arguments for testing
        private static string[ ] testArgs = new string[ ]
        {
            "path/to/executable.exe",
            "-level",
            "5",
            "-playerName",
            "Hero",
            "extraArgument",
            "-debug",
            "-fullscreen",
            "true",
            "anotherExtra"
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
            TestArgumentParsing( testArgs );

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

} //END gambit.process Namespace