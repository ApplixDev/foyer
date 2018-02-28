using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Foyer.Tests.Utilities
{
    public class Utility_Tests
    {
        private Dictionary<string, string> ProjectsAssembliesPath = new Dictionary<string, string>();
        private ITestOutputHelper _output;

        public Utility_Tests(ITestOutputHelper output)
        {
            _output = output;

            ProjectsAssembliesPath.Add("Test", @"C:\projects\Foyer\3.2.0\src\Tests\Foyer.Tests\bin\Debug");
            ProjectsAssembliesPath.Add("Migrator", @"C:\projects\Foyer\3.2.0\src\Tools\Foyer.Migrator\bin\Debug");
            ProjectsAssembliesPath.Add("Application", @"C:\projects\Foyer\3.2.0\src\Foyer.Application\bin\Debug");
            ProjectsAssembliesPath.Add("Core", @"C:\projects\Foyer\3.2.0\src\Foyer.Core\bin\Debug");
            ProjectsAssembliesPath.Add("EntityFramework", @"C:\projects\Foyer\3.2.0\src\Foyer.EntityFramework\bin\Debug");
            ProjectsAssembliesPath.Add("Web", @"C:\projects\Foyer\3.2.0\src\Foyer.Web\bin");
            ProjectsAssembliesPath.Add("WebApi", @"C:\projects\Foyer\3.2.0\src\Foyer.WebApi\bin\Debug");
        }

        [Fact]
        public void FindConflictingReferences()
        {
            var assemblies = GetAllAssemblies(ProjectsAssembliesPath["Core"]);

            var references = GetReferencesFromAllAssemblies(assemblies);

            var groupsOfConflicts = FindReferencesWithTheSameShortNameButDiffererntFullNames(references);

            foreach (var group in groupsOfConflicts)
            {
                _output.WriteLine("Possible conflicts for {0}:", group.Key);
                foreach (var reference in group)
                {
                    _output.WriteLine("{0} references {1}",
                                          reference.Assembly.Name.PadRight(25),
                                          reference.ReferencedAssembly.FullName);
                }
            }
        }

        private IEnumerable<IGrouping<string, Reference>> FindReferencesWithTheSameShortNameButDiffererntFullNames(List<Reference> references)
        {
            return from reference in references
                   group reference by reference.ReferencedAssembly.Name
                       into referenceGroup
                   where referenceGroup.ToList().Select(reference => reference.ReferencedAssembly.FullName).Distinct().Count() > 1
                   select referenceGroup;
        }

        private List<Reference> GetReferencesFromAllAssemblies(List<Assembly> assemblies)
        {
            var references = new List<Reference>();
            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                {
                    continue;
                }

                foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
                {
                    references.Add(new Reference
                    {
                        Assembly = assembly.GetName(),
                        ReferencedAssembly = referencedAssembly
                    });
                }
            }
            return references;
        }

        private List<Assembly> GetAllAssemblies(string path)
        {
            var files = new List<FileInfo>();
            var directoryToSearch = new DirectoryInfo(path);
            files.AddRange(directoryToSearch.GetFiles("*.dll", SearchOption.AllDirectories));
            files.AddRange(directoryToSearch.GetFiles("*.exe", SearchOption.AllDirectories));
            return files.ConvertAll(file =>
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(file.FullName);
                    return asm;
                }
                catch (System.BadImageFormatException)
                {
                    return null;
                }

            });
        }

        private class Reference
        {
            public AssemblyName Assembly { get; set; }
            public AssemblyName ReferencedAssembly { get; set; }
        }

    }
}
