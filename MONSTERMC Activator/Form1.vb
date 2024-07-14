Imports System.Management
Imports System.Diagnostics

Public Class Form1
    Private Sub btnCheckActivation_Click(sender As Object, e As EventArgs) Handles btnCheckActivation.Click
        txtResults.Clear()
        CheckWindowsActivation()
        CheckOfficeActivation()
    End Sub

    Private Sub CheckWindowsActivation()
        Try
            Dim slmgrPath As String = System.IO.Path.Combine(Environment.SystemDirectory, "slmgr.vbs")
            If Not System.IO.File.Exists(slmgrPath) Then
                MessageBox.Show("slmgr.vbs not found in System32 directory.")
                Return
            End If

            Dim process As New Process()
            process.StartInfo.FileName = "cscript"
            process.StartInfo.Arguments = $"//nologo ""{slmgrPath}"" /dli"
            process.StartInfo.UseShellExecute = False
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.CreateNoWindow = True
            process.Start()

            Dim output As String = process.StandardOutput.ReadToEnd()
            process.WaitForExit()

            txtResults.AppendText("Windows Activation Status:" & Environment.NewLine)
            txtResults.AppendText(output & Environment.NewLine)
            txtResults.AppendText(New String("-"c, 50) & Environment.NewLine)
        Catch ex As Exception
            MessageBox.Show("Error checking Windows activation: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckOfficeActivation()
        Try
            Dim officePaths As String() = {
                "C:\Program Files\Microsoft Office\Office16\ospp.vbs",
                "C:\Program Files (x86)\Microsoft Office\Office16\ospp.vbs",
                "C:\Program Files\Microsoft Office\Office15\ospp.vbs",
                "C:\Program Files (x86)\Microsoft Office\Office15\ospp.vbs",
                "C:\Program Files\Microsoft Office\Office14\ospp.vbs",
                "C:\Program Files (x86)\Microsoft Office\Office14\ospp.vbs"
            }

            For Each path In officePaths
                If System.IO.File.Exists(path) Then
                    Dim process As New Process()
                    process.StartInfo.FileName = "cscript"
                    process.StartInfo.Arguments = $"//nologo ""{path}"" /dstatus"
                    process.StartInfo.UseShellExecute = False
                    process.StartInfo.RedirectStandardOutput = True
                    process.StartInfo.CreateNoWindow = True
                    process.Start()

                    Dim output As String = process.StandardOutput.ReadToEnd()
                    process.WaitForExit()

                    txtResults.AppendText($"Office Activation Status ({path}):" & Environment.NewLine)
                    txtResults.AppendText(output & Environment.NewLine)
                    txtResults.AppendText(New String("-"c, 50) & Environment.NewLine)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("Error checking Office activation: " & ex.Message)
        End Try
    End Sub
End Class
