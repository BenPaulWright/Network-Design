Net Design Phase 2

Files:
	Exes:
		Client.exe
		Server.exe
		
	SourceCode:
		NetDesign_Lib **:
			Packet.cs
			PacketHandler.cs
			Udp.cs
			Tcp.cs
		NetDesign_UC_Lib:
			Various UI Bits
		NetDesign Phase 2:
			Client:
				MainWindow.cs
			Server:
				MainWindow.cs

Compilation:			
	The submitted code contains a folder "NetDesign Phase 2" in the format of a Visual Studio workspace containing four projects. Client and Server are nearly identical and are made up almost entirely of methods from the class libraries NetDesign_Lib and NetDesign_UC_Lib.
	In order to build the projects, the workspace "NetDesign Phase 2.sln" must be opened with visual studio. The project is set to build both cliend and server in Release mode.

Running:
	If you do not wish to compile the code yourself, I have included two EXEs; Client.ex and Server.exe
	These can be run directly.
	
	Note ** The executables require .Net Runtime to be installed in order to launch