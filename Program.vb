Imports System.Runtime.InteropServices
Imports System.Threading

Module RdpIdleMateRemote

    <StructLayout(LayoutKind.Sequential)>
    Public Structure INPUT
        Public type As Integer
        Public mi As MOUSEINPUT
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure MOUSEINPUT
        Public dx As Integer
        Public dy As Integer
        Public mouseData As Integer
        Public dwFlags As Integer
        Public time As Integer
        Public dwExtraInfo As IntPtr
    End Structure

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function SendInput(nInputs As UInteger, ByRef pInputs As INPUT, cbSize As Integer) As UInteger
    End Function

    <DllImport("user32.dll")>
    Private Function GetLastInputInfo(ByRef plii As LASTINPUTINFO) As Boolean
    End Function

    <DllImport("kernel32.dll")>
    Private Function GetTickCount() As UInteger
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure LASTINPUTINFO
        Public cbSize As UInteger
        Public dwTime As UInteger
    End Structure

    Function GetIdleTimeMs() As UInteger
        Dim lii As LASTINPUTINFO
        lii.cbSize = CUInt(Marshal.SizeOf(GetType(LASTINPUTINFO)))
        If GetLastInputInfo(lii) Then
            Return GetTickCount() - lii.dwTime
        Else
            Return 0
        End If
    End Function

    Sub JIGGLE()
        Dim input As New INPUT With {
            .type = 0, ' INPUT_MOUSE
            .mi = New MOUSEINPUT With {
                .dx = 1,
                .dy = 0,
                .dwFlags = &H1 ' MOUSEEVENTF_MOVE
            }
        }

        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        ' Reverse it to avoid cursor drift
        input.mi.dx = -1
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Sub Main()
        Console.WriteLine("IdleMate (Remote RDP Edition) running. Ctrl+C to exit.")
        While True
            Dim idleMs = GetIdleTimeMs()

            If idleMs > 5000 Then
                JIGGLE()
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] IdleTrigger (Idle: {idleMs / 1000}s)")
            Else
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Active. No Idle Trigger Needed (Idle: {idleMs / 1000}s)")
            End If

            Thread.Sleep(5000)
        End While
    End Sub

End Module
