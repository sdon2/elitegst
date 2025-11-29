@echo off
REM BUILD EliteGST
SET BUILDER=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe
SET SOLUTION=%~dp0EliteGST.sln
SET FLAGS=/t:ReBuild /p:Configuration=Release
%BUILDER% %SOLUTION% %FLAGS%"
PAUSE
