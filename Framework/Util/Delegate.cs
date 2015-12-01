using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public delegate void Callback();
    public delegate void Callback<T0>(T0 arg0);
    public delegate void Callback<T0, T1>(T0 arg0, T1 arg1);
    public delegate void Callback<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2);
    public delegate void Callback<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    public delegate void Callback<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    public delegate T Function<out T>();
    public delegate T Function<out T, T0>(T0 arg0);
    public delegate T Function<out T, T0, T1>(T0 arg0, T1 arg1);
    public delegate T Function<out T, T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2);
    public delegate T Function<out T, T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    public delegate T Function<out T, T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}
