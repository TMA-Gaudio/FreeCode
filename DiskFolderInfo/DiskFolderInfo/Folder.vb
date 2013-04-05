Public Class Folder
    Public Property Path As String = String.Empty
    Public Property Size As Long = 0
    Public Property Filecount As Long = 0
    Public Property HasAccess As Boolean = True
    Public ReadOnly Property HumanSize As String
        Get
            If Size < 1024 Then
                Return Size & " BYTES"
            Else
                If Size < 1000000 Then
                    Return Math.Round(Size / 1024, 0) & " KB"
                Else
                    Return Math.Round(Size / 1024 / 1024, 0) & " MB"
                End If
            End If
        End Get
    End Property
End Class
