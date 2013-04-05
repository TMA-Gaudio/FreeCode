Imports System.IO
Public Class Form1
    Private Delegate Sub dlgMessage(m As String)
    Private Delegate Sub dlg()
    Private _BaseDrive As String = String.Empty
    Private _folders As New List(Of Folder)
    Private _MaxScan As Integer = 500
    Private _ScanCount As Integer = 0
    Private _FoldersScanned As Long = 0
    Private _FilesScanned As Long = 0
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each dInfo As DriveInfo In My.Computer.FileSystem.Drives
            cboDrives.Items.Add(dInfo.Name)
        Next
        cboDrives.SelectedIndex = 0
    End Sub

    Private Sub btnScanNow_Click(sender As Object, e As EventArgs) Handles btnScanNow.Click
        _BaseDrive = cboDrives.Text
        Dim t As New Threading.Thread(AddressOf DoScan)
        t.IsBackground = True
        t.Start()
        btnScanNow.Enabled = False
    End Sub
    Private Sub DoScan()
        DoScan(_BaseDrive)
        ShowReport()
    End Sub
    Private Sub DoScan(mappe As String)
        AddLog(mappe)
        Dim Folder As New Folder
        Folder.Path = mappe
        _FoldersScanned += 1
        Try
            Dim files() As String = Directory.GetFiles(mappe)
            For Each fil As String In files
                Dim fi As New FileInfo(fil)
                Folder.Filecount += 1
                Folder.Size += fi.Length
                _FilesScanned += 1
            Next
        Catch ex As Exception
            Folder.HasAccess = False
        End Try
        '_ScanCount += 1
        'If _ScanCount > _MaxScan Then Exit Sub

        If Folder.HasAccess Then
            If Folder.Size > 0 Then
                _folders.Add(Folder)
            End If

            For Each SubFolder As String In Directory.GetDirectories(mappe)
                DoScan(SubFolder)
            Next
        End If

    End Sub
    Private Sub ShowReport()
        If dg.InvokeRequired Then
            Dim d As New dlg(AddressOf ShowReport)
            dg.Invoke(d)
        Else
            txtPath.Text = "Scan complete.."
            Dim ds As New DataSet
            Dim tab As DataTable = ds.Tables.Add
            tab.Columns.Add("Path", GetType(String))
            tab.Columns.Add("Filecount", GetType(Long))
            tab.Columns.Add("Size", GetType(Long))
            tab.Columns.Add("HumanSize", GetType(String))

            For Each f As Folder In _folders
                Dim row As DataRow = tab.Rows.Add
                row("path") = f.Path
                row("size") = f.Size
                row("filecount") = f.Filecount
                row("humansize") = f.HumanSize
            Next

            dg.DataSource = tab
            dg.Visible = True
        End If

    End Sub
    Private Sub AddLog(message As String)
        If txtPath.InvokeRequired Then
            Dim parms() As Object = {message}
            Dim dlg As New dlgMessage(AddressOf AddLog)
            txtPath.Invoke(dlg, parms)
        Else
            txtPath.Text = message
            txtScannedfiles.Text = _FilesScanned.ToString
            txtScannedfolders.Text = _FoldersScanned.ToString
        End If
    End Sub
End Class
