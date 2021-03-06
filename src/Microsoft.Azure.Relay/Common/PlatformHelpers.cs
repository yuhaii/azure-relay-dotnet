﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.Relay
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    static class PlatformHelpers
    {
        public static ArraySegment<byte> GetArraySegment(this MemoryStream memoryStream)
        {
            Fx.Assert(memoryStream != null, "memoryStream is required");
            
            ArraySegment<byte> buffer;
#if NET45 || NET451 || NET452
            buffer = new ArraySegment<byte>(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
#else
            // .NET 4.6 and .NET Core added MemoryStream.TryGetBuffer()
            // .NET Core removed MemoryStream.GetBuffer()
            if (!memoryStream.TryGetBuffer(out buffer))
            {
                buffer = new ArraySegment<byte>(memoryStream.ToArray());
            }
#endif

            return buffer;
        }

        public static string GetRuntimeFramework()
        {
#if NET451 || NET452 || NET46 || NET461 || NET462 || NET47
            return RuntimeEnvironment.GetSystemVersion();
#else
            // Only available on .NET Framework starting with 4.7.1 (also netstandard1.1 and later)
            return RuntimeInformation.FrameworkDescription;
#endif
        }
    }
}
