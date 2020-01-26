using System;
using System.Diagnostics;
using System.Text;
using Autofac;
using Autofac.Builder;
using ScooterBear.GTD.Application;
using ScooterBear.GTD.DynamoDb;
using ScooterBear.GTD.DynamoDb.Dynamo;
using ScooterBear.GTD.Patterns;
using Xunit;

namespace ScooterBear.GTD.IntegrationTests
{
    [CollectionDefinition("DynamoDbDockerTests")]
    public class RunDynamoDbDockerCollection : ICollectionFixture<RunDynamoDbDockerFixture>
    {

    }

    public class RunDynamoDbDockerFixture : IDisposable
    {
        public IContainer Container { get; }
        public RunDynamoDbDockerFixture()
        {
            this.Container = SetupAutofac().Build(ContainerBuildOptions.None);

            //NOTE:  If this fails - run MakeDnaymoDbDockerImage/BuildCustomDyanmoDbDocker.sh
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "run -d --rm --name integration-test -p 8000:8000 jayroblack/dynamodb-local:1.1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            processStartInfo.Environment["CONFIGURATION"] = "Debug";
            processStartInfo.Environment["COMPUTERNAME"] = Environment.MachineName;

            var process = Process.Start(processStartInfo);
            var std = new StringBuilder();
            while (!process.StandardOutput.EndOfStream)
                std.Append(process.StandardOutput.ReadLine());
            
            var err = new StringBuilder();
            while (!process.StandardError.EndOfStream)
                err.Append(process.StandardError.ReadLine());

            process.WaitForExit();
            if (process.ExitCode != 0)
                Assert.False(true, err.ToString());
            
            Assert.Equal(0, process.ExitCode);
            if( !WaitForContainerToBeOnline())
                Assert.False(true, "Failed to start up and query DynamoDb Local.");
        }

        public void Dispose()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments =
                    $"stop integration-test"
            };

            var process = Process.Start(processStartInfo);

            process.WaitForExit();
            Assert.Equal(0, process.ExitCode);
        }

        public bool WaitForContainerToBeOnline()
        {
            var timeout = TimeSpan.FromSeconds(60);

            var start = DateTime.Now;
            while (DateTime.Now - start < timeout)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "aws",
                    Arguments =
                        $"dynamodb list-tables --endpoint-url http://localhost:8000"
                };

                var process = Process.Start(processStartInfo);

                process.WaitForExit();

                if (process.ExitCode == 0)
                    return true;
            }

            return false;
        }

        public ContainerBuilder SetupAutofac()
        {
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterModule<ApplicationAutofacModule>();
            builder.RegisterModule<DynamoDbAutofacModule>();
            builder.RegisterModule<PatternsAutofacModule>();

            //Overrides
            builder.RegisterType<DynamoDBLoccalFactory>().As<IDynamoDBFactory>();
            return builder;
        }
    }
}
