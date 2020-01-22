# Usage

Paket should be installed 

https://fsprojects.github.io/Paket/get-started.html

```
dotnet new tool-manifest
dotnet tool install -g paket
```

Run paket install command

```
paket install
```

Download model file (example https://lindat.mff.cuni.cz/repository/xmlui/bitstream/handle/11234/1-2898/russian-syntagrus-ud-2.3-181115.udpipe?sequence=71&isAllowed=y )

Save ```russian-syntagrus-ud-2.3-181115.udpipe``` to c://tmp/

Run Example.fsx in interactive mode, or with "fsianycpu Example.fsx" (fsianycpu should be in path)