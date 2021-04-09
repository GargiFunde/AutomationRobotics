Imports System.Runtime.InteropServices

Public Module WindowsApi

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Structure StartupInfo
        Public cb As Integer
        Public reserved As String
        Public desktop As String
        Public title As String
        Public x As Integer
        Public y As Integer
        Public xSize As Integer
        Public ySize As Integer
        Public xCountChars As Integer
        Public yCountChars As Integer
        Public fillAttribute As Integer
        Public flags As Integer
        Public showWindow As UInt16
        Public reserved2 As UInt16
        Public reserved3 As Byte
        Public stdInput As IntPtr
        Public stdOutput As IntPtr
        Public stdError As IntPtr
    End Structure

    Public Structure ProcessInformation
        Public process As IntPtr
        Public thread As IntPtr
        Public processId As Integer
        Public threadId As Integer
    End Structure

    <DllImport("kernel32.dll", EntryPoint:="WTSGetActiveConsoleSessionId", SetLastError:=True)>
    Public Function WTSGetActiveConsoleSessionId() As UInteger
    End Function

    <DllImport("wtsapi32.dll", EntryPoint:="WTSQueryUserToken", SetLastError:=True)>
    Public Function WTSQueryUserToken(
        ByVal SessionId As UInteger, ByRef phToken As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("advapi32.dll", EntryPoint:="CreateProcessAsUserW", SetLastError:=True)>
    Public Function CreateProcessAsUserW(
        <InAttribute()> ByVal token As IntPtr,
        <InAttribute(), MarshalAs(UnmanagedType.LPWStr)> ByVal applicationName As String,
        ByVal commandLine As System.IntPtr,
        <InAttribute()> ByVal processAttributes As IntPtr,
        <InAttribute()> ByVal threadAttributes As IntPtr,
        <MarshalAs(UnmanagedType.Bool)> ByVal inheritHandles As Boolean,
        ByVal creationFlags As UInteger,
        <InAttribute()> ByVal environment As IntPtr,
        <InAttribute(), MarshalAsAttribute(UnmanagedType.LPWStr)> ByVal currentDirectory As String,
        <InAttribute()> ByRef startupInfo As StartupInfo,
        <OutAttribute()> ByRef processInformation As ProcessInformation) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

End Module
