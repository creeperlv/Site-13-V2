mkdir GitHub-Tools
cd GitHub-Tools
cd ScalableRelativeImage
git pull
cd SRI.CLI
dotnet build -c Release -o ..\..\bin\SRI\CLI\
cd ..
cd SRI.Editor.Main
dotnet build -c Release -o ..\..\bin\SRI\Editor\
cd ..
cd ..
cd FlatNuGet
git pull
cd FlatNuGet
dotnet build -c Release -o ..\..\bin\FlatNuGet\
cd ..
cd Tools\CommonTools\
dotnet publish .\BTNodeCompiler\BTNodeCompiler.csproj -c:Release --self-contained true -r win-x64  -p:PublishSingleFile=true -o ..\..\GitHub-Tools\bin\BTNodeCompiler\
dotnet publish .\BTNode\BTNodeBuilder\BTNodeBuilder.csproj -c:Release --self-contained true -r win-x64  -p:PublishSingleFile=true -o ..\..\GitHub-Tools\bin\BTNodeBuilder\
cd ..
cd ..