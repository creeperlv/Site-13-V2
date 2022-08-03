mkdir GitHub-Tools
cd GitHub-Tools
git clone https://github.com/creeperlv/ScalableRelativeImage.git
cd ScalableRelativeImage
cd SRI.CLI
dotnet build -c Release -o ..\..\bin\SRI\CLI\
cd ..
cd SRI.Editor.Main
dotnet build -c Release -o ..\..\bin\SRI\Editor\