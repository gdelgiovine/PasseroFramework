using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace PasseroFormGenerator
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(WisejFormGeneratorPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class WisejFormGeneratorPackage : AsyncPackage
    {
        public const string PackageGuidString = "b1b6a5c8-2a3d-4e8f-9c7d-1a2b3c4d5e6f";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await GeneratePasseroFormCommand.InitializeAsync(this);
        }
    }
}