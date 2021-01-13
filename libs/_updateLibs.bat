@echo off

set ONI_DATA_PATH=%ONI_PATH%\OxygenNotIncluded_Data\Managed

%SystemRoot%\System32\xcopy "%ONI_DATA_PATH%\Unity*.dll" %~dp0 /Y
%SystemRoot%\System32\xcopy "%ONI_DATA_PATH%\Assembly-CSharp*.dll" %~dp0 /Y
%SystemRoot%\System32\xcopy "%ONI_DATA_PATH%\0Harmony.dll" %~dp0 /Y
%SystemRoot%\System32\xcopy "%ONI_DATA_PATH%\Newtonsoft.Json.dll" %~dp0 /Y
