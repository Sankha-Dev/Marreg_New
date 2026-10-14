Imports Microsoft.VisualBasic
<Serializable>
Public Class Marreg_Loginfo
    Dim _user_nm As String
    Dim _user_id As String
    Dim _role_id As String
    Dim _role_nm As String
    Dim _role_type_cd As String
    Dim _state_cd As String
    Dim _dist_cd As String
    Dim _subdiv_cd As String
    Dim _block_cd As String
    Dim _MARREG_Session As String
    Dim _Login_Status As String
    Dim _designation As String
    Dim _Ipaddress As String
    Dim _MO_cd As String
    Dim _MOname As String
    Dim _Status_cd As String
    Dim _Status_flag As String

    Public Property user_nm() As String
        Get
            Return _user_nm
        End Get
        Set(ByVal value As String)
            _user_nm = value
        End Set
    End Property

    Public Property user_id() As String
        Get
            Return _user_id
        End Get
        Set(ByVal value As String)
            _user_id = value
        End Set
    End Property

    Public Property role_id() As String
        Get
            Return _role_id
        End Get
        Set(ByVal value As String)
            _role_id = value
        End Set
    End Property

    Public Property role_nm() As String
        Get
            Return _role_nm
        End Get
        Set(ByVal value As String)
            _role_nm = value
        End Set
    End Property

    Public Property role_type_cd() As String
        Get
            Return _role_type_cd
        End Get
        Set(ByVal value As String)
            _role_type_cd = value
        End Set
    End Property

    Public Property state_cd() As String
        Get
            Return _state_cd
        End Get
        Set(ByVal value As String)
            _state_cd = value
        End Set
    End Property

    Public Property dist_cd() As String
        Get
            Return _dist_cd
        End Get
        Set(ByVal value As String)
            _dist_cd = value
        End Set
    End Property

    Public Property subdiv_cd() As String
        Get
            Return _subdiv_cd
        End Get
        Set(ByVal value As String)
            _subdiv_cd = value
        End Set
    End Property

    Public Property block_cd() As String
        Get
            Return _block_cd
        End Get
        Set(ByVal value As String)
            _block_cd = value
        End Set
    End Property

    Public Property MARREG_Session() As String
        Get
            Return _MARREG_Session
        End Get
        Set(ByVal value As String)
            _MARREG_Session = value
        End Set
    End Property

    Public Property Login_Status() As String
        Get
            Return _Login_Status
        End Get
        Set(ByVal value As String)
            _Login_Status = value
        End Set
    End Property

    Public Property designation() As String
        Get
            Return _designation
        End Get
        Set(ByVal value As String)
            _designation = value
        End Set
    End Property

    Public Property Ipaddress() As String
        Get
            Return _Ipaddress
        End Get
        Set(ByVal value As String)
            _Ipaddress = value
        End Set
    End Property

    Public Property MO_cd() As String
        Get
            Return _MO_cd
        End Get
        Set(ByVal value As String)
            _MO_cd = value
        End Set
    End Property
    Public Property MOname() As String
        Get
            Return _MOname
        End Get
        Set(ByVal value As String)
            _MOname = value
        End Set
    End Property
    Public Property status_cd() As String
        Get
            Return _Status_cd
        End Get
        Set(ByVal value As String)
            _Status_cd = value
        End Set
    End Property
    Public Property status_flag() As String
        Get
            Return _Status_flag
        End Get
        Set(ByVal value As String)
            _Status_flag = value
        End Set
    End Property


End Class