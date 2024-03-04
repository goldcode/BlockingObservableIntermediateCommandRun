// See https://aka.ms/new-console-template for more information


using SiLAGen.Client.TestService;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tecan.Sila2;
using Tecan.Sila2.Cancellation;
using Tecan.Sila2.Cancellation.CancelController;
using Tecan.Sila2.Client;
using Tecan.Sila2.Discovery;

// wait for server to startup.
await Task.Delay(2000);

var connector = new ServerConnector(new DiscoveryExecutionManager());
var discovery = new ServerDiscovery(connector);
var executionManagerFactory = new DiscoveryExecutionManager();

var server = discovery.GetServers(TimeSpan.FromSeconds(3), nic => true).FirstOrDefault() ?? 
    throw new InvalidOperationException("no servers found");

var silaClient = new TestServiceClient(server.Channel, executionManagerFactory);
var cancelControllerClient = new CancelControllerClient(server.Channel, executionManagerFactory);
silaClient.Initialize();


var command = silaClient.ObservableIntermediateCommandWithCancellation();
await foreach (var item in ReadChannelAsync(command))
{
    Console.WriteLine(item);
}



static async IAsyncEnumerable<T> ReadChannelAsync<T>(IIntermediateObservableCommand<T> command, [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    while (await command.IntermediateValues.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
    {
        if (command.IntermediateValues.TryRead(out var item))
        {
            yield return item;
        }
    }
}
