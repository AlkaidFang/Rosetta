@echo off
.\ProtoGen\protogen.exe -i:.\XMessage.proto -o:.\PacketMessage.cs
:xcopy .\*.cs ..\ /y/d
pause