// <copyright file="PexAssemblyInfo.cs">Copyright ©  2010</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;
using Microsoft.Pex.Linq;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "NUnit")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("Strive.Client.ViewModel")]
[assembly: PexInstrumentAssembly("WindowsBase")]
[assembly: PexInstrumentAssembly("Strive.Network.Messaging")]
[assembly: PexInstrumentAssembly("UpdateControls.XAML")]
[assembly: PexInstrumentAssembly("Strive.Common")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("PresentationCore")]
[assembly: PexInstrumentAssembly("UpdateControls")]
[assembly: PexInstrumentAssembly("Strive.Client.Model")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "WindowsBase")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Strive.Network.Messaging")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "UpdateControls.XAML")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Strive.Common")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "PresentationCore")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "UpdateControls")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Strive.Client.Model")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtBehavedSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]

// Microsoft.Pex.Linq
[assembly: PexLinqPackage]

