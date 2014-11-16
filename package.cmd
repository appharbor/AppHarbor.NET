if not exist Download\content\lib\net35 mkdir Download\content
if not exist Download\package\lib\net35 mkdir Download\package\lib\net35

copy AppHarbor.Net\bin\Release\AppHarbor.NET.xml Download\

copy LICENSE.txt Download

copy AppHarbor.Net\bin\Release\Web.config.transform Download\content
copy AppHarbor.Net\bin\Release\AppHarbor.NET.dll Download\Package\lib\net35\
copy AppHarbor.Net\bin\Release\AppHarbor.NET.xml Download\Package\lib\net35\

nuget pack appharbor.nuspec -BasePath Download\Package -Output Download