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
cd ..