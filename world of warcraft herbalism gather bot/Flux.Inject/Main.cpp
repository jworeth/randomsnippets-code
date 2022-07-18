#include <Windows.h>
#include <exception>
#include <d3d9.h>

#pragma comment( lib, "d3d9" )
#pragma comment( lib, "user32" )

using namespace System;
using namespace System::Reflection;
using namespace System::Windows::Forms;

typedef HRESULT ( WINAPI * tEndScene )( IDirect3DDevice9* );
tEndScene pEndScene = nullptr;

namespace FluxAD
{
	ref class AssemblyLoader;

	public ref class FluxGlobal
	{
	public:
		static AppDomain^		Domain;
		static AssemblyLoader^	AssemblyLoader;
	};

	public ref class AssemblyLoader : MarshalByRefObject
	{
	public:
		void LoadAndRun( String^ strFile )
		{
			Assembly^	_Assembly	= Assembly::Load( strFile );
			MethodInfo^	MI			= _Assembly->EntryPoint;
			Object^		o			= _Assembly->CreateInstance( MI->Name );

			// Use Reflection to find a method named OnFluxFrame
			for each ( Type^ _Type in _Assembly->GetTypes() )
				for each ( MethodInfo^ _MethodInfo in _Type->GetMethods() )
					if ( _MethodInfo->Name == "OnFrame" && _MethodInfo->IsStatic )
						this->_MethodInfo = _MethodInfo;

			MI->Invoke( o, nullptr );
		}

		void OnFrame(IntPtr^ devicePtr)
		{
			try
			{
				if ( _MethodInfo != nullptr )
				{
					_MethodInfo->Invoke( nullptr, nullptr );
				}
			} catch ( System::Reflection::TargetInvocationException^){} // Bah! Humbug :(
		}

		MethodInfo^ _MethodInfo;
	};

	public ref class AD
	{
	public:
		AD()
		{
			try
			{
				//MessageBox::Show("Injected!");
				Assembly^ _Assembly			= Assembly::GetAssembly( AssemblyLoader::typeid );
				//Random^ rand = gcnew Random();
				//int name = rand->Next(0,500000);
				FluxGlobal::Domain			= AppDomain::CreateDomain( "FluxAppDomain" );				
				FluxGlobal::AssemblyLoader	= static_cast< AssemblyLoader^ > ( FluxGlobal::Domain->CreateInstanceFromAndUnwrap( _Assembly->Location, AssemblyLoader::typeid->FullName ) );
				FluxGlobal::AssemblyLoader->LoadAndRun( "Flux" );
			} catch ( Exception^ e )
			{
				MessageBox::Show( "[Flux.Inject]" + e->ToString() );
			} finally
			{
				FluxGlobal::AssemblyLoader = nullptr;
				AppDomain::Unload( FluxGlobal::Domain );
			}
		}
	};
}

HRESULT WINAPI hkEndScene( IDirect3DDevice9* pThis )
{
	using namespace FluxAD;

	HRESULT ret = pEndScene( pThis );

	try
	{
		if ( FluxGlobal::AssemblyLoader != nullptr )
			FluxGlobal::AssemblyLoader->OnFrame(nullptr);
	} catch ( Exception^ e ) {
		MessageBox::Show( e->ToString() );
	}

	return ret;
}

#pragma region UnmanagedCode

void *DetourFunc(BYTE *src, const BYTE *dst, const int len)
{
	BYTE *jmp = (BYTE*)malloc(len+5);
	DWORD dwback;

	VirtualProtect(src, len, PAGE_READWRITE, &dwback);

	memcpy(jmp, src, len);	jmp += len;

	jmp[0] = 0xE9;
	*(DWORD*)(jmp+1) = (DWORD)(src+len - jmp) - 5;

	src[0] = 0xE9;
	*(DWORD*)(src+1) = (DWORD)(dst - src) - 5;

	VirtualProtect(src, len, dwback, &dwback);
	VirtualProtect(jmp-len, len+5, PAGE_EXECUTE_READWRITE, &dwback );

	return (jmp-len);
}

LRESULT CALLBACK MainWndProc(HWND, UINT, WPARAM, LPARAM)
{
	return 1;
}

DWORD WINAPI ThreadMain( LPVOID lpParam )
{
	//MessageBoxA(NULL, "Injected in ThreadMain", NULL, MB_OK);
	try
	{
		WNDCLASSEXA wndClass;
		memset( &wndClass, 0, sizeof( WNDCLASSEX ) );
		wndClass.cbSize					= sizeof( WNDCLASSEX );
		wndClass.style					= CS_CLASSDC;
		wndClass.lpfnWndProc			= &MainWndProc;
		wndClass.lpszClassName			= "RandomClassName";

		if ( !RegisterClassExA( &wndClass ) )
			throw gcnew Exception( "Failed to register class" );

		HWND				hWnd		= CreateWindow( L"RandomClassName", L"AnotherRandomWindowTitle", 0, 0, 0, 1, 1, HWND_DESKTOP, 0, 0, 0 );
		
		if ( !hWnd )
			throw gcnew Exception( "Failed to create window" );
		
		IDirect3D9*			pDirect3D	= Direct3DCreate9( D3D_SDK_VERSION );
		IDirect3DDevice9*	pDevice		= NULL;

		if ( !pDirect3D )
			throw gcnew Exception( "Failed to create D3D9" );

		// Set up the structure used to create the D3DDevice
		D3DPRESENT_PARAMETERS d3dpp;
		memset(&d3dpp, 0, sizeof(D3DPRESENT_PARAMETERS));
		d3dpp.Windowed					= true;
		d3dpp.SwapEffect				= D3DSWAPEFFECT_DISCARD;
		d3dpp.BackBufferFormat			= D3DFMT_UNKNOWN;

		if ( FAILED( pDirect3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd, D3DCREATE_SOFTWARE_VERTEXPROCESSING, &d3dpp, &pDevice) ) )
			throw gcnew Exception( "Failed to create device" );

		PDWORD pdwTable					= *reinterpret_cast<PDWORD*>( pDevice );
		DWORD  dwEndScene				= pdwTable[42];
		pDevice->Release();
		pDirect3D->Release();
		DestroyWindow( hWnd );

		pEndScene = (tEndScene)DetourFunc( (BYTE*)dwEndScene, (BYTE*)hkEndScene, 5 );
		BOOL firstLoaded = FALSE;

		while ( 1 )
		{
			if (!firstLoaded)
			{
				firstLoaded = TRUE;
				gcnew FluxAD::AD;
			}
			
			if ( GetAsyncKeyState( VK_F1 )&1 )
			{
				// All thats needed, due to the awesome powah of try() catch() finally()
				gcnew FluxAD::AD;
			}

			Sleep( 10 );
		}
	} catch ( Exception^ e )
	{
		MessageBox::Show( e->ToString(), "Exception", MessageBoxButtons::OK, MessageBoxIcon::Error );
	}

	return 1;
}

#pragma unmanaged
BOOL WINAPI DllMain( HMODULE hDll, DWORD dwReason, LPVOID lpReserved )
{
	static HANDLE hThread;
	if ( dwReason == DLL_PROCESS_ATTACH )
	{
		hThread = CreateThread( NULL, 0, ThreadMain, NULL, 0, NULL );
	} else if ( dwReason == DLL_PROCESS_DETACH )
		TerminateThread( hThread, 0 );

	return TRUE;
}
#pragma endregion UnmanagedCode