
Dim connectionId,sessionId,controlId,controlType,propertyToFetch,rowIndex,cellName,result,tableCtrlType
connectionId = WScript.Arguments(0)
sessionId = WScript.Arguments(1)
controlId = WScript.Arguments(2)
controlType = WScript.Arguments(3)
propertyToFetch = WScript.Arguments(4)
rowIndex = WScript.Arguments(5)
cellName = WScript.Arguments(6)

If Not IsObject(application) Then
   Set SapGuiAuto  = GetObject("SAPGUI")
   Set application = SapGuiAuto.GetScriptingEngine
End If
If Not IsObject(connection) Then
   Set connection = application.Children(CInt(connectionId))
End If
If Not IsObject(session) Then
   Set session    = connection.Children(CInt(sessionId))
End If
If IsObject(WScript) Then
   WScript.ConnectObject session,     "on"
   WScript.ConnectObject application, "on"
End If
 

If(propertyToFetch="ControlExists") Then

	on error resume next
	result = session.findById(controlId).type
	    If err.number = 0 Then
  	        result = "true"
	    Else
  	        result = "false"
	    End If
	on error goto 0	

Else

    Select case controlType
	    case "GuiTextField"
		    result = session.findById(controlId).text
	    case "GuiTextEdit"
		    result = session.findById(controlId).text
	    case "GuiTitlebar"
		    result = session.findById(controlId).text		
            case "GuiRadioButton"
		    result = session.findById(controlId).Selected
		    if(result=0) Then
			    result = "False"
		    Else
			    result = "True"
		    End If
	    case "GuiLabel"
		    result = session.findById(controlId).DisplayedText
            case "GuiCTextField"
		    result = session.findById(controlId).text
	    case "GuiComboBox"
		    result = session.findById(controlId).Value
	    case "GuiCheckBox"
		    result = session.findById(controlId).Selected
		    if(result=0) Then
			    result = "False"
		    Else
			    result = "True"
		    End If
	    case "GuiGridView"
            Select case propertyToFetch
                case "Default"
		            result = session.findById(controlId).GetCellValue(rowIndex,cellName)    
                case "RowCount"
                    result = session.findById(controlId).RowCount
                case "VisibleRowCount"
                    result = session.findById(controlId).VisibleRowCount
            End Select
        case "GuiTableControl"
            Select case propertyToFetch
                case "RowCount"
                    result = session.findById(controlId).RowCount
                case "VerticalScroll"
                    session.findById(controlId).verticalScrollbar.position = rowIndex
                    result = "ok"
                case "ControlValue"
                     tableCtrlType = session.findById(controlId).getAbsoluteRow(rowIndex).Item(CInt(cellName)).type
                     If(tableCtrlType="GuiRadioButton" OR tableCtrlType="GuiCheckBox") Then
                         result =  session.findById(controlId).getAbsoluteRow(rowIndex).Item(CInt(cellName)).Selected
                            If(result=0) Then
			                    result = "False"
		                    Else
			                    result = "True"
		                    End If
                     ElseIf(tableCtrlType="GuiComboBox") Then
                        result =  session.findById(controlId).getAbsoluteRow(rowIndex).Item(CInt(cellName)).Value
                     ElseIf(tableCtrlType="GuiLabel") Then
                        result =  session.findById(controlId).getAbsoluteRow(rowIndex).Item(CInt(cellName)).DisplayedText
		             Else
			            result =  session.findById(controlId).getAbsoluteRow(rowIndex).Item(CInt(cellName)).text
                        If(result=tableCtrlType) Then
                            result = ""
                        End If
		             End If
            End Select
        case "GuiStatusbar"
            result  = session.findById(controlId).Text
        
        case "GuiModalWindow"
             result = session.findById(controlId).Text
        case "GuiTree"
                Select case session.findById(controlId).getItemType(cellName,rowIndex)
                    case 2
                        result = session.findById(controlId).getItemText(cellName,rowIndex)
                    case 3
                        result = session.findById(controlId).getCheckBoxState(cellName,rowIndex)
                        if(result=0) Then
			                result = "False"
		                Else
			                result = "True"
		                End If                    
                End Select
             'End If             
    End Select

End If

WScript.Echo result