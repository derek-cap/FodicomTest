using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.Autofac
{
    class AutofacTest
    {
        static void Run()
        {
            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();
            // Register types that expose interfaces...

            // Build the container to finalize registrations and prepare for object resolution.
            IContainer container = builder.Build();
        }
    }
}
