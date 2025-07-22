# unity-process-manager
A singleton manager for Unity that handles starting, stopping, and sending command-line arguments to external application processes. This tool is useful for applications that need to interact with and manage other executables.

**Assembly:**\
com.gambit.process

**GameObject Display Name:**\
gambit.process.ProcessManager (Singleton)

**Namespace:**\
gambit.process

**ASMDEF File:**\
gambit.process  

**Scripting Define Symbol:**\
GAMBIT_PROCESS

---

## FEATURES

**Start and Stop Processes:**\
Launch and terminate external processes from within Unity.

**Command-Line Arguments:**\
Send command-line arguments to the launched process.

**State Tracking:**\
Monitor the state of the managed process (e.g., running, not running).

**Singleton Design:**\
Easily access the process manager from anywhere in your project.

**Callbacks:**\
Use actions for success, failure, and state updates to respond to process events.

---

## DEMO INSTRUCTIONS

Access the demo scene in located in the Assets/Demo folder\
\
The demo scene includes a "Demo" GameObject with two components used to test the ProcessManager package.

**ProcessManagerDemo**
- **Path** : Full path to the executable you want to run. Defaults to notepad.exe for Windows OS.
- **ArgumentKeys** : The command line argument keys you want to pass into the process defined at the path variable
- **ArgumentValues** : The command line argument values you want to pass into the process defined at the path variable

---

## INSTALLATION INSTRUCTIONS

### Method 1: Unity Package Manager (via Git URL)

This is the recommended installation method.

1.  In your Unity project, open the **Package Manager** (`Window > Package Manager`).
2.  Click the **'+'** button in the top-left corner and select **"Add package from git URL..."**
3.  Enter the following URL:
    ```
    https://github.com/GambitGamesLLC/unity-process-manager.git?path=Assets/Plugins/Package
    ```
4.  To install a specific version, append the version tag to the URL:
    ```
    https://github.com/GambitGamesLLC/unity-process-manager.git?path=Assets/Plugins/Package#v1.0.0
    ```

**Alternatively, you can manually edit your project's `Packages/manifest.json` file:**

```json
{
  "dependencies": {
    "com.gambit.process": "https://github.com/GambitGamesLLC/unity-process-manager.git?path=Assets/Plugins/Package",
    ...
  }
}
```

### Method 2: Local Installation

1.  Download or clone this repository to your computer.
2.  In your Unity project, open the **Package Manager** (`Window > Package Manager`).
3.  Click the **'+'** button in the top-left corner and select **"Add package from disk..."**
4.  Navigate to the cloned repository folder and select the `package.json` file inside `Assets/Plugins/Package`.

---

## HOW TO USE
To get started, you need to create a ProcessSystem instance.

This is done by calling the ProcessManager.Create method and passing in an Options object.

```
using gambit.process;
using UnityEngine;

public class MyProcessController : MonoBehaviour
{
    private ProcessManager.ProcessSystem processSystem;

    void Start()
    {
        ProcessManager.Create(
            new ProcessManager.Options
            {
                path = "C:\\Path\\To\\Your\\Executable.exe",
                argumentKeys = new System.Collections.Generic.List<string> { "arg1", "arg2" },
                argumentValues = new System.Collections.Generic.List<string> { "value1", "value2" },
                showDebugLogs = true
            },
            OnSuccess: (system) => {
                processSystem = system;
                Debug.Log("Process system created successfully!");
            },
            OnFailed: (error) => {
                Debug.LogError("Failed to create process system: " + error);
            },
            OnStateUpdate: (system, state) => {
                Debug.Log("Process state updated: " + state);
            }
        );
    }
}
```

## LAUNCHING A PROCESS
Once the ProcessSystem is created, you can launch the process using the LaunchProcess method.

```
public void Launch()
{
    if (processSystem != null)
    {
        ProcessManager.LaunchProcess(processSystem,
            OnLaunchSuccess: () => {
                Debug.Log("Process launched successfully!");
            },
            OnLaunchFailed: (error) => {
                Debug.LogError("Failed to launch process: " + error);
            }
        );
    }
}
```

## STOPPING AND KILLING A PROCESS
You can stop a running process gracefully with StopProcess or force it to close with KillProcess.

```
public void Stop()
{
    if (processSystem != null)
    {
        ProcessManager.StopProcess(processSystem,
            OnSuccess: () => {
                Debug.Log("Process stopped successfully!");
            },
            OnFailed: (error) => {
                Debug.LogError("Failed to stop process: " + error);
            }
        );
    }
}

public void Kill()
{
    if (processSystem != null)
    {
        ProcessManager.KillProcess(processSystem,
            OnSuccess: () => {
                Debug.Log("Process killed successfully!");
            },
            OnFailed: (error) => {
                Debug.LogError("Failed to kill process: " + error);
            }
        );
    }
}
```

## READING COMMAND-LINE ARGUMENTS
The launched application can read the command-line arguments passed to it using the ReadArgumentKeys and ReadArgumentValues methods.

```
// In the launched application
using gambit.process;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentReader : MonoBehaviour
{
    void Start()
    {
        List<string> keys = ProcessManager.ReadArgumentKeys();
        List<string> values = ProcessManager.ReadArgumentValues();

        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log($"Argument: {keys[i]} = {values[i]}");
        }
    }
}
```

---

## DEPENDENCIES
This package relies on other open-source packages to function correctly. The required dependencies will be automatically installed by the Unity Package Manager.

-   **Gambit Static Coroutine** [[Gambit Repo]](https://github.com/GambitGamesLLC/unity-config-manager)  
    A utility for running coroutines without a MonoBehaviour instance

-   **Gambit Singleton** [[Gambit Repo]](https://github.com/GambitGamesLLC/unity-singleton)  
    Used as the base pattern for the singleton instance. It is recommended to use this package in any project with singletons to maintain a consistent pattern.

---
