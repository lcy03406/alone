@echo off
setlocal enabledelayedexpansion
set R=%~dp0\..\Assets\Resources\Sprites
set A=%~dp0\Art
set T=%~dp0\..\Assets\Scripts\Schema\SpriteID.cs
cd %A%
echo using System; > %T%
echo namespace Schema { >> %T%
echo [Serializable] >> %T%
echo public enum SpriteID { >> %T%
set /a n=0
for /f %%i in ('dir /b') do (
echo 	%%~ni, >> %T%
set /a n=n+1
)
echo } >> %T%
echo } >> %T%
E:\tools\TexturePacker\bin\TexturePacker.exe --force-publish --format json --data %~dp0\tileset.json --texture-format png --png-opt-level 0 --sheet %R%\tileset.png --width 512 --height 512 --algorithm Basic --basic-sort-by name --border-padding 1 --shape-padding 2 --disable-rotation --trim-mode None %A%
cd %R%
%~dp0\spritesheet-artifact-fixer.py -f tileset.png -x 32 -y 32 -m 1 -s 2 -o tileset.png
cd %~dp0