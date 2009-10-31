Public Class Message
    Dim mMsgString, mFromIP As String
    Dim lClientID As Long
    Dim mDateRx As Date

    Public Property MsgString() As String
        Get
            Return mMsgString
        End Get
        Set(ByVal Value As String)
            mMsgString = Value
        End Set
    End Property

    Public Property FromIP() As String
        Get
            Return mFromIP
        End Get
        Set(ByVal Value As String)
            mFromIP = Value
        End Set
    End Property

    Public Property clientID() As Long
        Get
            Return lClientID
        End Get
        Set(ByVal Value As Long)
            lClientID = Value
        End Set
    End Property

    Public Property DateReceived() As Date
        Get
            Return mDateRx
        End Get
        Set(ByVal Value As Date)
            mDateRx = Value
        End Set
    End Property

    Public Sub New(ByVal msg As String, ByVal id As Long, ByVal ip As String, ByVal theDate As Date)
        mMsgString = msg
        mFromIP = ip
        mDateRx = theDate
        lClientID = id
    End Sub

End Class
