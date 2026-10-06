@echo off
cd /d "%~dp0"
Terraria.exe -savedirectory "%CD%" -logfile "%CD%\Logs" -logerrors
