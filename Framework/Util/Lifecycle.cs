using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public interface Lifecycle
    {
        bool Init();

        void Tick();

        void Destroy();
    }
}
