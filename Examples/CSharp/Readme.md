# Usage
PlatformTarget should be set to `x64` or `x86`
```
<PlatformTarget>x64</PlatformTarget>
```


Download model file (example https://lindat.mff.cuni.cz/repository/xmlui/bitstream/handle/11234/1-2898/russian-syntagrus-ud-2.3-181115.udpipe?sequence=71&isAllowed=y )

Save ```russian-syntagrus-ud-2.3-181115.udpipe``` to c://tmp/

Execute

``
cd CSExample 
dotnet run
```

Expected output:

```
Loading model...
done
мама мыть рама мыло
```
