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
    }




}
