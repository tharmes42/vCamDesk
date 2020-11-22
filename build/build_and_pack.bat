@REM echo off
REM This batch program links .exe and .dll-Files together and creates zip-File for release
REM get version for the future
REM  https://stackoverflow.com/questions/1706892/how-do-i-retrieve-the-version-of-a-file-from-a-batch-file-on-windows-vista
cd "..\bin\Release"
..\..\packages\ILMerge.3.0.40\tools\net452\ILMerge.exe /log /ndebug /out:vCamDesk.exe vCamDesk-non-packed.exe AForge.Controls.dll AForge.dll AForge.Imaging.dll AForge.Math.dll AForge.Video.DirectShow.dll AForge.Video.dll Newtonsoft.Json.dll nucs.JsonSettings.dll
"C:\Program Files\7-Zip\7z.exe" a -tzip vCamDesk-pre-release-v0.xxx.zip vCamDesk.exe ../../backgrounds
pause