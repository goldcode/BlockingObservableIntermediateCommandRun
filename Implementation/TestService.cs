using ProtoBuf;
using Sila.Contracts;
using System.Text;
using Tecan.Sila2;

namespace Implementation
{
    public class TestService : ITestService
    {

        public void Initialize()
        {

        }

        public Task SomeCommand(CancellationToken cancellationToken) => Task.Delay(int.MaxValue, cancellationToken);

        public async Task CancelCommandWorks(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(int.MaxValue, cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Console.Write(ex.Message);
            }
        }

        class ObservableIntermediateCommand : ObservableIntermediatesCommand<string>
        {
            public ObservableIntermediateCommand(CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource) { }

            public override async Task Run()
            {
                int id = 0;
                while(!CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        PushIntermediate($"Hello World {id++}");
                        //await Task.Delay(1000, CancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }                
            }
        }

        public IIntermediateObservableCommand<string> ObservableIntermediateCommandWithCancellation() => new ObservableIntermediateCommand(new CancellationTokenSource());


        public class ObservableIntermediateStreamCommand : ObservableIntermediatesCommand<Stream>
        {
            public ObservableIntermediateStreamCommand(CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource) { }

            public override async Task Run()
            {
                int idx = 0;
                while (!CancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(10, CancellationToken);
                    PushIntermediate(new MemoryStream(Encoding.UTF8.GetBytes(idx++.ToString())));
                }
            }
        }
        public IIntermediateObservableCommand<Stream> ObservableIntermediateCommandStream() => new ObservableIntermediateStreamCommand(new CancellationTokenSource());
    }




}
