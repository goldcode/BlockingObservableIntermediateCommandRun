
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tecan.Sila2;

namespace Sila.Contracts
{
    /// <summary>
    /// Service to control the MyrImaging Device.
    /// </summary>
    [SilaFeature]
    public interface ITestService
    {
        /// <summary>
        /// Initialize the hardware of the instrument
        /// </summary>
        void Initialize();
        
        [Observable]
        IIntermediateObservableCommand<string> ObservableIntermediateCommandWithCancellation();

    }
}
