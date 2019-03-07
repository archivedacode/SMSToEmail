// SetupDLL.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include <windows.h>
#include <commctrl.h>

HINSTANCE g_hinstModule;

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            g_hinstModule = (HINSTANCE)hModule;
            break;
    }
    return TRUE;
}

// **************************************************************************
// Function Name: Install_Init
// 
// Purpose: processes the push message.
//
// Arguments:
//    IN HWND hwndParent  handle to the parent window
//    IN BOOL fFirstCall  indicates that this is the first time this function is being called
//    IN BOOL fPreviouslyInstalled  indicates that the current application is already installed
//    IN LPCTSTR pszInstallDir  name of the user-selected install directory of the application
//
// Return Values:
//    codeINSTALL_INIT
//    returns install status
//
// Description:  
//    The Install_Init function is called before installation begins.
//    User will be prompted to confirm installation.
// **************************************************************************
codeINSTALL_INIT Install_Init(
    HWND        hwndParent,
    BOOL        fFirstCall,     // is this the first time this function is being called?
    BOOL        fPreviouslyInstalled,
    LPCTSTR     pszInstallDir
    )
{
    int iReply = IDYES;

    iReply = MessageBox(hwndParent,
					    _T("Have you read and understood the End User Licence Agreement (EULA) and are now ready to install SMS To Email?"),
		                _T("EULA Confirmation"),
                        MB_YESNO | MB_ICONQUESTION);

    if (IDNO == iReply)
    {
        return codeINSTALL_INIT_CANCEL;
    }

    return codeINSTALL_INIT_CONTINUE;
}


// **************************************************************************
// Function Name: Install_Exit
// 
// Purpose: processes the push message.
//
// Arguments:
//    IN HWND hwndParent  handle to the parent window
//    IN LPCTSTR pszInstallDir  name of the user-selected install directory of the application
//
// Return Values:
//    codeINSTALL_EXIT
//    returns install status
//
// Description:  
//    Register query client with the PushRouter as part of installation.
//    Only the first two parameters really count.
// **************************************************************************
codeINSTALL_EXIT Install_Exit(
    HWND    hwndParent,
    LPCTSTR pszInstallDir,      // final install directory
    WORD    cFailedDirs,
    WORD    cFailedFiles,
    WORD    cFailedRegKeys,
    WORD    cFailedRegVals,
    WORD    cFailedShortcuts
    )
{
	return codeINSTALL_EXIT_DONE;
}


// **************************************************************************
// Function Name: Uninstall_Init
// 
// Purpose: processes the push message.
//
// Arguments:
//    IN HWND hwndParent  handle to the parent window
//    IN LPCTSTR pszInstallDir  name of the user-selected install directory of the application
//
// Return Values:
//    codeUNINSTALL_INIT
//    returns uninstall status
//
// Description:  
//    Query the device data using the query xml in the push message,
//    and send the query results back to the server.
// **************************************************************************
codeUNINSTALL_INIT Uninstall_Init(
    HWND        hwndParent,
    LPCTSTR     pszInstallDir
    )
{
	// Here we need to delete the XML DATA file

	// Prepare the path to the XML File
	TCHAR szPath[MAX_PATH]; 
	_tcscpy(szPath, pszInstallDir); 
	_tcscat(szPath, _T("\\")); 
	_tcscat(szPath, _T("DATA.xml")); 

	try
	{
		WIN32_FIND_DATA wfd;
		ZeroMemory(&wfd, sizeof(WIN32_FIND_DATA));
		
		HANDLE hSearch;

		hSearch = FindFirstFile (szPath, &wfd);

		if (hSearch != INVALID_HANDLE_VALUE)
		{
			DeleteFile(szPath);
			FindClose (hSearch);
		}
	}
	catch(...) { }

	try
	{
		RegDeleteKey(HKEY_LOCAL_MACHINE,
					 TEXT("Software\\Microsoft\\Inbox\\Rules\\OuchSmsToEmail\\MessageFilter"));

		RegDeleteKey(HKEY_LOCAL_MACHINE,
					 TEXT("Software\\Microsoft\\Inbox\\Rules\\OuchSmsToEmail"));

	}
	catch(...) { }

	return codeUNINSTALL_INIT_CONTINUE;
}


// **************************************************************************
// Function Name: Uninstall_Exit
// 
// Purpose: processes the push message.
//
// Arguments:
//    IN HWND hwndParent  handle to the parent window
//
// Return Values:
//    codeUNINSTALL_EXIT
//    returns uninstall status
//
// Description:  
//    Query the device data using the query xml in the push message,
//    and send the query results back to the server.
// **************************************************************************
codeUNINSTALL_EXIT Uninstall_Exit(
    HWND    hwndParent
    )
{
    return codeUNINSTALL_EXIT_DONE;
}

