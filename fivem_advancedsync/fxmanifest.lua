fx_version 'bodacious'
game 'gta5'

author 'Ethan4t0r'
version '1.0.0'
description 'Advanced sync for FiveM in C#'

fxdk_watch_command 'dotnet' {'watch', '--project', 'Client/fivem_advancedsync.Client.csproj', 'publish', '--configuration', 'Release'}
fxdk_watch_command 'dotnet' {'watch', '--project', 'Server/fivem_advancedsync.Server.csproj', 'publish', '--configuration', 'Release'}

file 'Client/bin/Release/**/publish/*.dll'

client_script 'Client/bin/Release/**/publish/*.net.dll'
server_script 'Server/bin/Release/**/publish/*.net.dll'
