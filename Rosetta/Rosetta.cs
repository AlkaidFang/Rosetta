using System;
using System.Collections.Generic;
using Framework;

namespace Rosetta
{
    public class Rosetta
    {
        public int GetRandomNum100()
        {
            Random r = new Random(DateTime.Now.Second);
            return r.Next(100);
        }

        public int GetRandomNum1000()
        {
            MyRandom r = new MyRandom();
            return r.GetRandomNum1000();
        }
    }
}

/*


for /r $(TargetDir) %%i in (*.pdb) do $(SolutionDir)\Framework\ExternalLibs\Mono\pdb2mdb.exe %%~dpni.dll
xcopy $(TargetDir)\*.dll $(SolutionDir)\Unity\Assets\Plugins /y /q
xcopy $(TargetDir)\*.mdb $(SolutionDir)\Unity\Assets\Plugins /y /q

*/