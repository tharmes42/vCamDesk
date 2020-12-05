@ECHO OFF
REM This batch program links .exe and .dll-Files together and creates zip-File for release

cd "..\bin\Release"

REM get version of build
FOR /F "USEBACKQ" %%F IN (`powershell -NoLogo -NoProfile -Command ^(Get-Item "vCamDesk-non-packed.exe"^).VersionInfo.FileVersion`) DO (SET fileVersion=%%F)
echo Determined release from vCamDesk-non-packed.exe: %fileVersion%

REM merge .exe and .dll files together
%userprofile%\.nuget\packages\ilmerge\3.0.41\tools\net452\ILMerge.exe /log /ndebug /out:vCamDesk.exe vCamDesk-non-packed.exe AForge.Controls.dll AForge.dll AForge.Imaging.dll AForge.Math.dll AForge.Video.DirectShow.dll AForge.Video.dll Newtonsoft.Json.dll nucs.JsonSettings.dll System.Net.Http.Formatting.dll

REM zip everything
"C:\Program Files\7-Zip\7z.exe" a -tzip vCamDesk-release-v%fileVersion%.zip vCamDesk.exe ../../../README.md

echo Find the packed release here:
cd
pause