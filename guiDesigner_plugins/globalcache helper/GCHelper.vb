'Imports System.Windows.Forms
'Imports CommandFusion

'Public Class GCHelper
'    'Implements CFPlugin

'    'Private m_Name, m_Author As String
'    'Private m_Form As New frmGCHelper

'    'Public Event ToggleWindow(ByVal plugin As CFPlugin) Implements CommandFusion.CFPlugin.ToggleWindow
'    'Public Event WriteToLog(ByVal msg As String) Implements CommandFusion.CFPlugin.WriteToLog
'    'Public Event AddSystem(ByVal newSystem As SystemClass) Implements CommandFusion.CFPlugin.AddSystem
'    'Public Event AddCommand(ByVal ExistingSystem As SystemClass, ByVal newCommand As SystemCommand) Implements CommandFusion.CFPlugin.AddCommand
'    'Public Event AddMacro(ByVal ExistingSystem As SystemClass, ByVal newMacro As SystemMacro) Implements CommandFusion.CFPlugin.AddMacro

'    'Private Sub DoToggleWindow(ByVal sender As Object, ByVal e As System.EventArgs)
'    '    RaiseEvent ToggleWindow(Me)
'    'End Sub

'    'Public Sub DoWriteToLog(ByVal msg As String)
'    '    RaiseEvent WriteToLog(msg)
'    'End Sub

'    'Public Sub DoAddSystem(ByVal newSystem As SystemClass)
'    '    RaiseEvent AddSystem(newSystem)
'    'End Sub

'    'Public Sub DoAddCommand(ByVal theSystem As SystemClass, ByVal newCommand As SystemCommand)
'    '    RaiseEvent AddCommand(theSystem, newCommand)
'    'End Sub

'    'Public Sub DoAddMacro(ByVal theSystem As SystemClass, ByVal newMacro As SystemMacro)
'    '    RaiseEvent AddMacro(theSystem, newMacro)
'    'End Sub

'    'Public Property Author() As String Implements CommandFusion.CFPlugin.Author
'    '    Get
'    '        Return "CommandFusion"
'    '    End Get
'    '    Set(ByVal value As String)
'    '    End Set
'    'End Property

'    'Public Sub DisposePlugin() Implements CommandFusion.CFPlugin.Dispose
'    '    Me.Form.Close()
'    'End Sub

'    'Public Property Form() As System.Windows.Forms.Form Implements CommandFusion.CFPlugin.Form
'    '    Get
'    '        Return m_Form
'    '    End Get
'    '    Set(ByVal value As System.Windows.Forms.Form)

'    '    End Set
'    'End Property

'    'Public Sub Init(ByVal menu As System.Windows.Forms.MainMenu) Implements CommandFusion.CFPlugin.Init

'    '    m_Form.pluginClass = Me
'    '    Dim pluginMenu As New MenuItem("GC Helper")
'    '    Dim showHideMenu As New MenuItem("Toggle Window")
'    '    AddHandler showHideMenu.Click, AddressOf DoToggleWindow
'    '    pluginMenu.MenuItems.Add(showHideMenu)
'    '    menu.MenuItems.Add(pluginMenu)

'    '    m_Form.Init()

'    'End Sub

'    'Public Property NamePlugin() As String Implements CommandFusion.CFPlugin.Name
'    '    Get
'    '        Return "GC Helper"
'    '    End Get
'    '    Set(ByVal value As String)

'    '    End Set
'    'End Property

'End Class


