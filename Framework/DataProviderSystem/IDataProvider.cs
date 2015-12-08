using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public interface IDataProvider
    {
        string Path();

        void Load();

        bool Verify();

    }
}
