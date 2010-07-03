Imports CommandFusion
Imports System.Windows.Forms

Public Class CFPluginDemo
    Implements CFPlugin


    Private m_Name, m_Author As String

    Public Event ToggleWindow(ByVal plugin As CFPlugin) Implements CommandFusion.CFPlugin.ToggleWindow
    Public Event WriteToLog(ByVal sender As CommandFusion.CFPlugin, ByVal msg As String) Implements CommandFusion.CFPlugin.WriteToLog
    Public Event AddCommand(ByVal sender As CommandFusion.CFPlugin, ByVal newCommand As CommandFusion.SystemCommand) Implements CommandFusion.CFPlugin.AddCommand
    Public Event AddFeedback(ByVal sender As CommandFusion.CFPlugin, ByVal newFB As CommandFusion.SystemFeedback) Implements CommandFusion.CFPlugin.AddFeedback
    Public Event AddMacro(ByVal sender As CommandFusion.CFPlugin, ByVal newMacro As CommandFusion.SystemMacro) Implements CommandFusion.CFPlugin.AddMacro
    Public Event AddSystem(ByVal sender As CommandFusion.CFPlugin, ByVal newSystem As CommandFusion.SystemClass) Implements CommandFusion.CFPlugin.AddSystem
    Public Event RequestSystemList(ByVal sender As CommandFusion.CFPlugin) Implements CommandFusion.CFPlugin.RequestSystemList
    Public Event AddMacros(ByVal sender As CommandFusion.CFPlugin, ByVal newMacros As System.Collections.Generic.List(Of CommandFusion.SystemMacro)) Implements CommandFusion.CFPlugin.AddMacros
    Public Event AppendSystem(ByVal sender As CommandFusion.CFPlugin, ByVal newSystem As CommandFusion.SystemClass) Implements CommandFusion.CFPlugin.AppendSystem

    Private m_ProjectOpen As Boolean = False

    Public ReadOnly Property NamePlugin() As String Implements CommandFusion.CFPlugin.Name
        Get
            Return "CF Demo Plugin"
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements CommandFusion.CFPlugin.Author
        Get
            Return "CommandFusion"
        End Get
    End Property

    Public ReadOnly Property Form() As Form Implements CommandFusion.CFPlugin.Form
        Get
            Return Me
        End Get
    End Property

    Public Overloads Sub Dispose() Implements CommandFusion.CFPlugin.DisposePlugin
        MyBase.Dispose()

        Me.Form.Close()
    End Sub

    Public Sub Init(ByVal menu As MainMenu) Implements CommandFusion.CFPlugin.Init
        Dim pluginMenu As New MenuItem(Me.NamePlugin)
        Dim showHideMenu As New MenuItem("Toggle Window")
        AddHandler showHideMenu.Click, AddressOf DoToggleWindow
        pluginMenu.MenuItems.Add(showHideMenu)
        menu.MenuItems.Add(pluginMenu)
    End Sub

    Private Sub DoToggleWindow(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent ToggleWindow(Me)
    End Sub

    Public Sub ProjectSelected(ByVal selected As Boolean) Implements CommandFusion.CFPlugin.ProjectSelected
        If selected Then
            Me.lblStatus.Text = "YES"
            Me.lblStatus.ForeColor = Drawing.Color.Green
        Else
            Me.lblStatus.Text = "NO"
            Me.lblStatus.ForeColor = Drawing.Color.Red
        End If

        ' Assign to a variable that can be used later to check a system is open in guiDesigner before actually adding anything to it
        m_ProjectOpen = selected
    End Sub

    Private Sub btnRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRequest.Click
        ' You first request the system list by using the following code:
        RaiseEvent RequestSystemList(Me)
    End Sub

    Public Sub UpdateSystemList(ByVal systemList As System.Collections.Generic.List(Of CommandFusion.SystemClass)) Implements CommandFusion.CFPlugin.UpdateSystemList
        ' A list of systems will be returned here for you to work with
        ' With one of these systems you can add commands and feedback, and even use its command list to create new macros
        cboSystems.Items.Clear()
        For Each aSystem As CommandFusion.SystemClass In systemList
            cboSystems.Items.Add(aSystem.Name)
        Next
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        MsgBox("This is an example plugin to use as a base for creating plugins for guiDesigner." & Environment.NewLine & _
                "For more help, please see our wiki: http://www.commandfusion.com/wiki")
    End Sub

    Public Sub AddDataToSystemManager()
        ' This is how you would add a system or macro to the currently open project in guiDesigner:

        ' First create the new system (or use a reference to a system from the 'UpdateSystemList' returned data above)
        Dim newSystem As New CommandFusion.SystemClass
        newSystem.Name = "Demo System Name"
        newSystem.PortDestination = 12345
        newSystem.PortOrigin = 12345
        newSystem.IPAddress = "192.168.0.100"
        newSystem.ProtocolUsed = SystemClass.Protocol.TCP
        newSystem.AlwaysOn = True

        ' Add any commands or feedback to the system:
        Dim newCmd As New CommandFusion.SystemCommand
        newCmd.Name = "Demo Command"
        newCmd.Value = "commandtosend\x0D"
        ' You need to both set the system the command belongs to and add it to the command list within the system
        newCmd.System = newSystem
        newSystem.Commands.Add(newCmd)

        Dim newFB As New CommandFusion.SystemFeedback
        newFB.Name = "Demo Feedback"
        newFB.Value = "regexhere(\d*)\x0D"
        newFB.System = newSystem
        newSystem.Feedback.Add(newFB)

        ' Add the system to the currently open project
        RaiseEvent AddSystem(Me, newSystem)

        ' Add a macro to the project (macros can contains commands from any system in the project)
        Dim newMacro As New CommandFusion.SystemMacro
        newMacro.Name = "Demo Macro"
        ' Use the command created above for example
        newMacro.Commands.Add(newCmd)
        ' Remember to also set the delay for the command as part of the macro
        newMacro.Delays.Add(100)
        ' Now add the macro to the project
        RaiseEvent AddMacro(Me, newMacro)

    End Sub

    Private Sub btnAddSystem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSystem.Click
        ' Before adding anything to a project, you should always check that one is currently open in guiDesigner first
        If m_ProjectOpen Then
            AddDataToSystemManager()
        Else
            MsgBox("You must open a project in guiDesigner before you can add a system")
        End If
    End Sub

End Class