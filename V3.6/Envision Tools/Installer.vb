Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

  <RunInstaller(True)> Public Class Installer
    Inherits System.Configuration.Install.Installer

#Region " Component Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Component Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call

    End Sub

    'Installer overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
        If Not (components Is Nothing) Then
          components.Dispose()
        End If
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      components = New System.ComponentModel.Container
    End Sub

#End Region

    '=========================================================================
    '
    ' Override the Install and Uninstall functions that are implemented  
    ' in the Installer base class and use the RegistrationServices class�s 
    ' RegisterAssembly and UnregisterAssembly methods to register the components.
    '
    ' Make sure you use the SetCodeBase flag; this indicates that the code base 
    ' key for the assembly should be set in the registry.
    '
    '=========================================================================
  Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
    MyBase.Install(stateSaver)

    'Register the custom component.
    '-----------------------------

    'The default location of the ESRIRegAsm utility.
    'Note how the whole string is embedded in quotes because of the spaces in the path.
    Dim cmd1 As String = """" & Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ArcGIS\bin\ESRIRegAsm.exe" & """"

    'Obtain the input argument (via the CustomActionData Property) in the setup project.
    'An example CustomActionData property that is passed through might be something like:
    '/arg1="[ProgramFilesFolder]\[ProductName]\bin\ArcMapClassLibrary_Implements.dll",
    'which translates to the following on a default install:
    'C:\Program Files\MyGISApp\bin\ArcMapClassLibrary_Implements.dll.
    Dim part1 As String = Me.Context.Parameters.Item("arg1")

    'Add the appropriate command line switches when invoking the ESRIRegAsm utility.
    'In this case: /p:Desktop = means the ArcGIS Desktop product, /s = means a silent install.
    Dim part2 As String = " /p:Desktop /s"

    'It is important to embed the part1 in quotes in case there are any spaces in the path.
    Dim cmd2 As String = """" & part1 & """" & part2

    'Call the routing that will execute the ESRIRegAsm utility.
    Dim exitCode As Integer = ExecuteCommand(cmd1, cmd2, 10000)
  End Sub

  Public Overrides Sub Uninstall(ByVal savedState As System.Collections.IDictionary)
    MyBase.Uninstall(savedState)

    'Unregister the custom component.
    '---------------------------------

    'The default location of the ESRIRegAsm utility.
    'Note how the whole string is embedded in quotes because of the spaces in the path.
    Dim cmd1 As String = """" & Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ArcGIS\bin\ESRIRegAsm.exe" & """"

    'Obtain the input argument (via the CustomActionData Property) in the setup project.
    'An example CustomActionData property that is passed through might be something like:
    '/arg1="[ProgramFilesFolder]\[ProductName]\bin\ArcMapClassLibrary_Implements.dll",
    'which translates to the following on a default install:
    'C:\Program Files\MyGISApp\bin\ArcMapClassLibrary_Implements.dll.
    Dim part1 As String = Me.Context.Parameters.Item("arg1")

    'Add the appropriate command line switches when invoking the ESRIRegAsm utility.
    'In this case: /p:Desktop = means the ArcGIS Desktop product, /u = means unregister the Custom Component, /s = means a silent install.
    Dim part2 As String = " /p:Desktop /u /s"

    'It is important to embed the part1 in quotes in case there are any spaces in the path.
    Dim cmd2 As String = """" & part1 & """" & part2

    'Call the routing that will execute the ESRIRegAsm utility.
    Dim exitCode As Integer = ExecuteCommand(cmd1, cmd2, 10000)

  End Sub

  Public Shared Function ExecuteCommand(ByVal Command1 As String, ByVal Command2 As String, ByVal Timeout As Integer) As Integer

    'Set up a ProcessStartInfo using your path to the executable (Command1) and the command line arguments (Command2).
    Dim ProcessInfo As ProcessStartInfo = New ProcessStartInfo(Command1, Command2)
    ProcessInfo.CreateNoWindow = True
    ProcessInfo.UseShellExecute = False

    'Invoke the process.
    Dim Process As Process = Process.Start(ProcessInfo)
    Process.WaitForExit(Timeout)

    'Finish.
    Dim ExitCode As Integer = Process.ExitCode
    Process.Close()
    Return ExitCode
  End Function

End Class

