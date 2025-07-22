# unity-process-manager
A Singleton Manager for Unity3D. Handles for starting, stopping, and sending command line arguments to application processes

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

## DEMO INSTRUCTIONS

Access the demo scene in located in the Assets/Demo folder\
\
The demo scene includes a "Demo" GameObject with two components used to test the ProcessManager package.

**ProcessManagerDemo**
- **Path** : Full path to the executable you want to run. Defaults to notepad.exe for Windows OS.
- **ArgumentKeys** : The command line argument keys you want to pass into the process defined at the path variable
- **ArgumentValues** : The command line argument values you want to pass into the process defined at the path variable
