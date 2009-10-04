Module_Name = 'CF_iViewer_Comm' 
(***********************************************************)
(*  FILE_LAST_MODIFIED_ON: 09/30/2008  AT: 12:08:49        *)
(***********************************************************)
(
	Dev idvTP, 						// The iViewer Physical Device
	Dev vdvTP[], 					// The Virtual Device Array Use To Have Multiple AMX TP Ports
	Char ciViewerPswd[],  // For Security
	Integer nLvlRange,		// Use To Set The Desire Level
	Char sFileName[],			// Name Of The CommandFusion GUI File
	Integer nDEBUG				// Use To Enable Debug Messages
)

(*
	How To Use:
	
	YOU MUST UPLOAD YOUR .GUI FILE, GENERATED FROM GUIDESIGNER, ONTO YOUR NELINX MASTER BEFORE LOADING THIS MODULE.
	
	When the Netlinx Master reboots, it will read, line by line, the .GUI file to gather the following information:
	
		- Project File Name.
		- Designer's Name.
		- IP Address that the iViewer will be connecting to.
		- The TCP Port (This will be use when the master opens that port.)
		- Page Name And Join Number.
	
	After this is complete, the master will open the TCP Port to allow your iViewer to connect.
	
	Note: This parsing can take a couple of minutes in this current version of this module.

*)

(*
	AMX Touch Panel Standards:
	
	Channel Range: 1 - 4000 button push and Feedback (per address port)
	Variable Text range: 1 - 4000 (per address port)
	Button States Range: 1 - 256 (0 = All states, for General buttons 1 = Off state and 2 = On state)
	Level Range: 1 - 600 (Default level value 0 - 255, can be set up to 1 - 65535)
	Address port Range: 1 - 100
*)

(*
	CF iViewer Touch Panel Rules:
	
	Channel Range: 1 - 1000 button push and Feedback (per address port)
	Variable Text range: 1 - 1000 (per address port)
	Button States Range: 1 - 256 (0 = All states, for General buttons 1 = Off state and 2 = On state)
	Level Range: 1 - 64 (Default level value 0 - 255, can be set up to 1 - 65535)
	Address port Range: 1 - 100
*)

//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

Define_Constant // IP Server Constants

cnOFF         = 0							// Constant Value OFF
cnON					= 1							// Constast Value ON

cnRELEASE 		= 0							//
cnPUSH    		= 1							//

cEndOfLine    = $03
cEquals       = '='

cHeartBeat    = 'h'						// Constant 'h' Heartbeat value Prefix
cButton 			= 'd'						// Constant 'd' Digital value Prefix
cLevel 			  = 'a'						// Constant 'a' Analog Value Prefix
cString 			= 's'						// Constant 's' Serial String Prefix
cPassword 		= 'p'						// Constant 'p' Password Validation Prefix
cInitialize 	= 'i'						// Constant 'i' Initialization Prefix
cOrientation 	= 'm'						// Constant 'm' Portrait/Landscape orentation 

cPortrait 		= 'portrait'		// Constant value for mode = Portrait
cLandscape 		= 'landscape'		// Constant value for mode = Landscape

cPswdPASS 		= 'ok'					// Constant value for correct password
cPswdFAIL 		= 'bad'					// Constant value for incorrect password

cActive       = '1'						// Reply With A 1 For The Heartbeat Message (Occurs every 3 seconds)

cnMinLvl      = 00000					//
cnMaxLvl			= 65535					//

cnModVer			= 1							//


Define_Constant

cnReadOnly		= 1
cnRWNew       = 2
cnRWAppend    = 3

cnOFF = 0
cnON  = 1

cnMaxPages    = 100
cnMaxPopUp    = 100

//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

Define_Type

Struct _iViewerPrt
{
	Char Address[16];
	Char Buffer[255];
	Char DatRcv[255];
}

Struct _iVPanel
{
	Char CmdRcv[255];
}

Struct _GUIInfo
{
	Char ProjName[25];
	Char Designer[25];
	Char IPAddress[25];
	Integer nIP_PORT;
}

Struct _Page
{
	Char sName[16];
	Integer nJoin;
}

Struct _PopUp
{
	Char sName[16];
	Integer nJoin;
}

//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

Define_Variable
_iViewerPrt iViewerPrt;
_iVPanel    iVPanel;
_GUIInfo    iGUIInfo;
_Page				iPage[cnMaxPages];
_PopUp			iPopUp[cnMaxPopUp];

Define_Variable

Volatile Integer nAppVer
Volatile Integer nVdvBtn
Volatile Integer nTempNum

Volatile Integer nPort
Volatile Integer nButton
Volatile Integer nBtnMode
Volatile Integer nLvlNum
Volatile Integer nTxtNum

Volatile Char sMODE[30]
Volatile Char sSTRING[255]
Volatile Integer nLEVEL[10]
Volatile Integer nSTATUS

Volatile Integer nLvlOpr

Volatile SLong fileIO
Volatile Char sTrash[16]

Volatile Integer nCount
Volatile Integer IDX
Volatile Char sPAGE[16]
Volatile Char sNAME[16]

Volatile Integer nTCP_PORT

Volatile Integer nTPAGE

//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

Define_Function runDebug		(Char sDATA[])
{
	Send_String 0,"'iViewerPrt Comm Mod -- ',sDATA"
}

Define_Function runOpenIPPort(Integer nIP_PORT)
{
	If(nDEBUG) 
	{
		runDebug("'runOpenIPPort() - Opening Server On TCP Port #',Itoa(iGUIInfo.nIP_PORT)")
	}
	nTCP_PORT = nIP_PORT
	ip_server_open(idvTP.PORT,nTCP_PORT,IP_TCP)
}

Define_Function runOpenFile (Char ThisFile[], Integer nFileMode)
{
	Stack_Var Char sFileIO[3]
	Stack_Var SLong Result
	
	nCount = 0
	IDX = 0
	fileIO = File_Open(ThisFile,nFileMode)
	
	If(fileIO > 0)
	{
		runReadLine(fileIO)
	}
	Else
	{
		sFileIO = Itoa(fileIO)
		Switch(sFileIO)
		{
			Case '-2' : runDebug("'fileIO Error - Invalid file path or name'")
			Case '-3' : runDebug("'fileIO Error - Invalid value supplied for IOFlag'")
			Case '-5' : runDebug("'fileIO Error - Disk I/O error'")
			Case '-14': runDebug("'fileIO Error - Maximum number of files are already open (max is 10)'")
			Case '-15': runDebug("'fileIO Error - Invalid file format'")
		}
		Result = File_Close(fileIO)
	}
}

Define_Function runReadLine (SLong nFileIO)
{
	Stack_Var Char sXMLData[1000]
	Stack_Var SLong nFileHandle
	Stack_Var Char sFileHandle[3]
	Stack_Var SLong Result
	
	nFileHandle = File_Read_Line(nFileIO,sXMLData,Max_Length_String(sXMLData))
	
	If(nFileHandle > 0)
	{
		If(nDEBUG)
		{
			runDebug("'nFileHandle - The Value Is #',Itoa(nFileHandle)")
		}
		
		Select
		{
			Active(Find_String(sXMLData,"'<project>'",1)):
			{
				sTrash = Remove_String(sXMLData,"'<project>'",1)
				sNAME = Remove_String(sXMLData,"'<'",1)
				Set_Length_String(sNAME,(Length_String(sNAME) - 1))
				iGUIInfo.ProjName = sNAME
			}
			Active(Find_String(sXMLData,"'<designer>'",1)):
			{
				sTrash = Remove_String(sXMLData,"'<designer>'",1)
				sNAME = Remove_String(sXMLData,"'<'",1)
				Set_Length_String(sNAME,(Length_String(sNAME) - 1))
				iGUIInfo.Designer = sNAME
			}
			Active(Find_String(sXMLData,"'<hostname>'",1)):
			{
				sTrash = Remove_String(sXMLData,"'<hostname>'",1)
				sNAME = Remove_String(sXMLData,"'<'",1)
				Set_Length_String(sNAME,(Length_String(sNAME) - 1))
				iGUIInfo.IPAddress = sNAME
			}
			Active(Find_String(sXMLData,"'<commandPort>'",1)):
			{
				sTrash = Remove_String(sXMLData,"'<commandPort>'",1)
				sNAME = Remove_String(sXMLData,"'<'",1)
				iGUIInfo.nIP_PORT = Atoi(sNAME)
			}
			Active(Find_String(sXMLData,"'page name='",1)):
			{
				sTrash = Remove_String(sXMLData,"'page name="'",1)
				sPAGE = Remove_String(sXMLData,"'"'",1)
				Set_Length_String(sPAGE,(Length_String(sPAGE) - 1))
				
				sTrash = Remove_String(sXMLData,"'j="'",1)
				IDX = Atoi(Remove_String(sXMLData,"'"'",1))
				
				If(IDX == 0)
				{
					If(nDEBUG)
					{
						runDebug("'ParseLine Error - The Joint Is Set To 0, Will Not Be Added'")
					}
				}
				Else
				{
					iPage[IDX].sName = sPAGE
					If(nDEBUG)
					{
						runDebug("'Page Added - "',sPAGE,'" Was Added To Array Index #',Itoa(IDX)")
					}
				}
			}
		}
		nCount++
		
		If(nDEBUG) 
		{
			runDebug("'ParseLine Count - The Current Line Count Is #',Itoa(nCount)")
			runDebug("'sXMLData Text - The Data Stored Is ',sXMLData")
		}
		
		sXMLData = ''
		Wait 2 runReadLine(nFileIO)
	}
	Else
	{
		sFileHandle = Itoa(nFileIO)
		Switch(sFileHandle)
		{
			Case '-1': runDebug("'nFileHandle Error - invalid file handle'")
			Case '-9': runDebug("'nFileHandle Error - EOF (end-of-file) reached'")
			Case '-5': runDebug("'nFileHandle Error - disk I/O error'")
			Case '-6': runDebug("'nFileHandle Error - invalid parameter (buffer length must be greater than zero)'")
		}
		If(nDEBUG) 
		{
			runDebug("'File_Close - Closing FileIO #',Itoa(nFileIO)")
		}
		Result = File_Close(nFileIO)
		runOpenIPPort(iGUIInfo.nIP_PORT)
	}
}

Define_Function runParseData(Char sDATARCV[])
{
	Stack_Var Char EventType[1]
	EventType = Left_String(sDATARCV,1)
	
	Switch(EventType)
	{
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Case cHeartBeat:
		{
			// Where are sending a h=1$03 to keep connection active
			
			Send_String idvTP,"cHeartBeat,cEquals,cActive,cEndOfLine"
			If(nDEBUG)
			{
				runDebug("'Heartbeat Event - Sending Reply Now'")
			}
		}
		Case cButton:
		{
			Remove_String(sDATARCV,cButton,1)
			nButton  = Atoi(Remove_String(sDATARCV,'=',1))
			nBtnMode = Atoi(Left_String(sDATARCV,1))
			
			If(nButton < 1000)
			{
				nPort = 1
				runBtnEvent(nButton,nBtnMode,nPort)
			}
			Else
			{
				For(nPort = 1; nPort <= 64; nPort++)
				{
					If(nButton > 1000)
					{
						nPort = (nPort + 1)
						nButton = (nButton - 1000)
					}
					Else
					{
						Break;
					}
				}
				runBtnEvent(nButton,nBtnMode,nPort)
			}
			If(nDEBUG)
			{
				runDebug("'Button Event #',Itoa(nButton)")
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Case cString:
		{
			Remove_String(sDATARCV,cString,1)
			nTxtNum = Atoi(Remove_String(sDATARCV,'=',1))
			
			If(nButton < 1000)
			{
				nPort = 1
				runStrEvent(nTxtNum,sDATARCV,nPort)
			}
			Else
			{
				For(nPort = 1; nPort <= 64; nPort++)
				{
					If(nTxtNum > 1000)
					{
						nPort = (nPort + 1)
						nTxtNum = (nTxtNum - 1000)
					}
					Else
					{
						Break;
					}
				}
				runStrEvent(nTxtNum,sDATARCV,nPort)
			}
			If(nDEBUG)
			{
				runDebug("'String Event #',Itoa(nTxtNum)")
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Case cLevel:
		{
			Remove_String(sDATARCV,cLevel,1)
			nLvlNum = Atoi(Remove_String(sDATARCV,'=',1))
			nLEVEL[nLvlNum] = Atoi(Remove_String(sDATARCV,"$03",1))
			
			If(nLvlNum < 1000)
			{
				nPort = 1
			}
			Else
			{
				For(nPort = 1; nPort <= 64; nPort++)
				{
					If(nLvlNum > 1000)
					{
						nPort = (nPort + 1)
						nLvlNum = (nLvlNum - 1000)
					}
					Else
					{
						Break;
					}
				}
			}
			
			Send_String vdvTP[nPort],"'LEVEL:',Itoa(nLvlNum),':',Itoa(nLEVEL[nLvlNum] / 257)"
			
			if(nDEBUG)
			{
				runDebug("'Level Event #',Itoa(nLvlNum)")
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Case cOrientation:
		{
			Remove_String(sDATARCV,'m=',1)
			Send_String vdvTP[1],"'Orientation:',sDATARCV"
			(* NOTE: Only The First UI Will Handle This Event *)
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Case cPassword:
		{
			Remove_String(sDATARCV,'p=',1)
			If(Find_String(sDATARCV,"ciViewerPswd,$03",1))
			{
				Send_String idvTP,"'p=ok',$03"
				Send_String 0,"'iViewerPrt Comm Mod -- Password Has Been Validated'"
			}
			Else
			{
				Send_String idvTP,"'p=bad',$03"
				Send_String 0,"'iViewerPrt Comm Mod -- Password Is Incorrect (Password = ',sDATARCV,')'"
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Case cInitialize:
		{
			Remove_String(sDATARCV,'i=',1)
			nSTATUS = Atoi(Left_String(sDATARCV,1))
			If(nSTATUS)
			{
				Send_String vdvTP[1],"'Initialize'"
				Send_String 0,"'iViewerPrt Comm Mod -- The iViewer Client Has Sent The Initialize Command'"
			}
			Else
			{
				// No Command
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
	}
	Remove_String(sDATARCV,"$03",1)
}

Define_Function runParseCmd	(Char sCMDRCV[], Integer IDX)
{
	Select
	{
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Active(Find_String(sCMDRCV,'PAGE',1)):
		{
			sTrash = Remove_String(sCMDRCV,"'PAGE-'",1)
			runPageCntl(sCMDRCV)
		}
		Active(Find_String(sCMDRCV,'TPAGEON',1)):
		{
			nTPAGE = cnON;
		}
		Active(Find_String(sCMDRCV,'TPAGEOFF',1)):
		{
			nTPAGE = cnOFF;
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Active(Find_String(sCMDRCV,'@PPX',1)):
		{
			// Not Implemented At This Time
		}
		Active(Find_String(sCMDRCV,'@PPN',1)):
		{
			// Not Implemented At This Time
		}
		Active(Find_String(sCMDRCV,'@PPT',1)):		// "'@PPT-<popup page name>;<timeout>'"
		{
			// Not Implemented At This Time
		}
		Active(Find_String(sCMDRCV,'@PPK',1)):		// "'@PPK-<pop-up page name>'"
		{
			// Not Implemented At This Time
		}
		Active(Find_String(sCMDRCV,'@PPM',1)):		// "'@PPM-<popup page name>;<mode>'"
		{
			// Not Implemented At This Time
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Active(Find_String(sCMDRCV,'!T',1)):
		{
			// Not Implemented At This Time
		}
		Active(Find_String(sCMDRCV,'TEXT',1)):
		{
			Remove_String(sCMDRCV,'TEXT',1)
			nVdvBtn = Atoi(Remove_String(sCMDRCV,'-',1))
			
			If(nPort = 1)
			{
				// Do Nothing To The Data
			}
			Else
			{
				nVdvBtn = (((nPort - 1) * 1000) + nVdvBtn)
			}
			runSndText(nVdvBtn,sCMDRCV)
		}
		Active(Find_String(sCMDRCV,'@TXT',1)):
		{
			// Not Implemented At This Time
		}
		Active(Find_String(sCMDRCV,'^TXT',1)):
		{
			//example string : ^TXT-26,1&2,Mike's Study
			
			Remove_String(sCMDRCV,'^TXT-',1)
			nVdvBtn = Atoi(left_string(sCMDRCV,','))
			remove_string(sCMDRCV,',',1)
			
			//parse here for seperate on/off states. Generally we send on and off state together (1&2)
			//Currently iViewer only supports 1 state per "serial join" anyways, so this method works
			remove_string(sCMDRCV,',',1)  
				
			If(nPort == 1)
			{
				// Do Nothing To The Data
			}
			Else 
			{
				nVdvBtn = (((nPort - 1) * 1000) + nVdvBtn)
			}
			runSndText(nVdvBtn,sCMDRCV)
		}
		
		///////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////
		
		Active(Find_String(sCMDRCV,'^BMP',1)):
		{
			Remove_String(sCMDRCV,'^BMP-',1);
			nVdvBtn = Atoi(Remove_String(sCMDRCV,',',1));
			If(nPort = 1)
			{
				// Do Nothing To The Data
			}
			Else
			{
				nVdvBtn = (((nPort - 1) * 1000) + nVdvBtn);
			}
			runSndImage(nVdvBtn,sCMDRCV);
		}
	}
}

Define_Function runPopUp		(Integer nIDX, Integer nSET, Integer nTIMED)	// This Function Is To Show Or Hide A Popup And If It Is Has A Displayed Time
{
	// Not Implemented At This Time
}

Define_Function runPageCntl (Char PAGE[])
{
	Stack_Var Integer IDX;
	Stack_Var Integer MODE;
	
	For(IDX = 1; IDX <= cnMaxPages; IDX++)
	{
		If(iPage[IDX].sName == PAGE)
		{
			MODE = 1;
			Send_String idvTP,"cButton,Itoa(IDX),cEquals,Itoa(MODE),cEndOfLine";
			Break;
		}
	}
}

Define_Function runBtnEvent	(Integer nBTN, Integer nMODE, Integer IDX)
{
	Send_String 0,"'iViewerPrt Comm Mod -- Running runBtnEvent() Function: Btn #',Itoa(nBTN),' Mode:',Itoa(nMODE)"
	If(nMODE = cnPUSH)
	{
		Do_Push_Timed(vdvTP[IDX],nBTN,Do_Push_Timed_Infinite)
	}
	Else If(nMODE = cnRELEASE)
	{
		Do_Release(vdvTP[IDX],nBTN)
	}
}

Define_Function runLvlEvent	(Integer nLvlInt, Integer nValue, Integer IDX)
{
	Send_Level vdvTP[IDX],nLvlInt,nValue
}

Define_Function runStrEvent	(Integer nTXT, Char sDATA[], Integer IDX)
{	
	Select
	{
		Active(Find_String(sDATA,'build',1)):
		{
			Remove_String(sDATA,'v',1)
			nAppVer = Atoi(Left_String(sDATA,1))
			
			If(nAppVer != cnModVer)
			{
				Send_String 0,"'iViewerPrt Comm Mod -- This Module Version Does Not Match The App Version'"
			}
			Else
			{
				Send_String 0,"'iViewerPrt Comm Mod -- This Module Version Matches The App Version'"
			}
		}
	}
	Send_String vdvTP[IDX],"'TEXT',Itoa(nTXT),'-',sDATA"
}

(*----------------------------------------------------------*)

Define_Function runSndText	(Integer nTXT, Char sDATA[])
{
	Send_String idvTP,"cString,Itoa(nTXT),cEquals,sDATA,cEndOfLine"
	Send_String 0,"'iViewerPrt Comm Mod -- Send Text ',sDATA,'To #',Itoa(nTXT)"
}

Define_Function runSndImage	(Integer nTXT, Char sDATA[])
{
	Remove_String(sDATA,',',1)
	Send_String idvTP,"cString,Itoa(nTXT),cEquals,sDATA,cEndOfLine"
	Send_String 0,"'iViewerPrt Comm Mod -- Send Image ',sDATA,'To #',Itoa(nTXT)"
}

Define_Function runBtnFBck	(Integer nBTN, Integer nMode)
{
	Send_String idvTP,"cButton,Itoa(nBTN),cEquals,Itoa(nMode),cEndOfLine"
	If(nDEBUG)
	{
		runDebug("'Feedback -- Button #',Itoa(nBTN),' Has Been Set To ',Itoa(nMode)")
	}
}

Define_Function runShwLvl		(Integer nLvlInt, Integer nValue)
{
	Stack_Var Integer nValueLvl
	
	nValueLvl = nValue * nLvlOpr
	Send_String idvTP,"cLevel,Itoa(nLvlInt),cEquals,Itoa(nValue),cEndOfLine"
}

//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

Define_Start

Create_Buffer idvTP,iViewerPrt.DatRcv

nLvlOpr = (cnMaxLvl / nLvlRange)

runOpenFile(sFileName,cnReadOnly)

//////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////

Define_Event

Data_Event[vdvTP]{ // 
	Online:
	{
		Send_String 0,"'iViewerPrt Comm Mod -- The iViewer vdvTP Is Now Online'"
	}
	Offline:
	{
		Send_String 0,"'iViewerPrt Comm Mod -- The iViewer vdvTP Is Now Offline'"
	}
	Command:
	{
		Send_String 0,"'iViewerPrt Comm Mod -- The iViewer vdvTP Command Received Is: ',data.text"
		iVPanel.CmdRcv = "iVPanel.CmdRcv,data.text";
		
		nPort = Get_Last(vdvTP)
		
		runParseCmd(iVPanel.CmdRcv,nPort)
	}
	OnError:
	{
	
	}
}

Data_Event[idvTP]{ // 
	Online:
	{
		On[idvTP,255];
		Send_String 0,"'iViewerPrt Comm Mod -- The iViewer Client Has Connected To The Master'"
		iViewerPrt.Address = DATA.SOURCEIP;
		Send_String 0,"'iViewerPrt Comm Mod -- The User IP Address Is ',iViewerPrt.Address"
	}
	String:
	{	
		Send_String 0,"'iViewerPrt Comm Mod -- The Data Received Is: ',iViewerPrt.DatRcv"
		
		While(Find_String(iViewerPrt.DatRcv,"$03",1))
		{	
			runParseData(iViewerPrt.DatRcv);
		}
	}
	Offline:
	{
		Off[idvTP,255];
		Send_String 0,"'iViewerPrt Comm Mod -- The iViewer Client Has Disconnected'"
		
		Wait 5 
		{
			ip_server_open(idvTP.PORT,nTCP_PORT,IP_TCP) // Open A New IP Session
			Send_String 0,"'iViewerPrt Comm Mod -- Opening A New IP Session For iViewer Client Connection'"
			
			iViewerPrt.Address = ''
			Send_String 0,"'iViewerPrt Comm Mod -- Clearing Out The Previous Client iViewer IP Address'"
		}
	}
	OnError:
	{
		Send_String 0,"'iViewerPrt Comm Mod -- ERROR -- Connection Has Been Lost'"
	}
}

Channel_Event[vdvTP,0]
{
	On:
	{
		Stack_Var Integer nPORT
		Stack_Var Integer nCHNL
		
		nPORT = Get_Last(vdvTP)
		if(nPort = 1)
		{
			nCHNL = channel.channel
		}
		Else
		{
			nCHNL = (((nPORT - 1) * 1000) + channel.channel)
		}
		runBtnFBck(nCHNL,cnON)
		
		if(nDEBUG)
		{
			runDebug("'Channel Event: ON -> Channel #',Itoa(channel.channel)")
		}
	}
	Off:
	{
		Stack_Var Integer nPORT
		Stack_Var Integer nCHNL
		
		nPORT = Get_Last(vdvTP)
		if(nPort = 1)
		{
			nCHNL = channel.channel
		}
		Else
		{
			nCHNL = (((nPORT - 1) * 1000) + channel.channel)
		}
		runBtnFBck(nCHNL,cnOFF)
		if(nDEBUG)
		{
			runDebug("'Channel Event: OFF -> Channel #',Itoa(channel.channel)")
		}
	}
}

Level_Event[vdvTP,0]
{
	Stack_Var Integer nLVLCHNL
	Stack_Var Integer nNEWLEVEL
	Stack_Var Integer nPORT
	
	nPORT = Get_Last(vdvTP)
	nLVLCHNL = level.input.level
	nNEWLEVEL = level.value * nLvlOpr
	
	if(nPort = 1)
	{
		nLVLCHNL = level.input.level
	}
	Else
	{
		nLVLCHNL = (((nPORT - 1) * 1000) + level.input.level)
	}
	
	nLEVEL[nLVLCHNL] = nNEWLEVEL
	
	Send_String idvTP,"cLevel,Itoa(nLVLCHNL),cEquals,Itoa(nNEWLEVEL),cEndOfLine"
	
	if(nDEBUG)
	{
		runDebug("'Level Event - Level Channel Is #',Itoa(nLVLCHNL), ' Value Is #',Itoa(nNEWLEVEL)")
	}
}

Button_Event[vdvTP[1],1]
{
	Push:
	{
		runOpenFile(sFileName,cnReadOnly)
	}
}
(***********************************************************)
(*                     END OF PROGRAM                      *)
(*        DO NOT PUT ANY CODE BELOW THIS COMMENT           *)
(***********************************************************)
