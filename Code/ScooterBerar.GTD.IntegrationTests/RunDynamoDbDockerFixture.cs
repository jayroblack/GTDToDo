using System;
using System.Diagnostics;
using System.Text;
using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Logging;
using ScooterBear.GTD.Application;
using ScooterBear.GTD.AWS.DynamoDb;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Fakes;
using ScooterBear.GTD.MailMerge;
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
            //Bootstrap Autofac
            this.Container = SetupAutofac().Build(ContainerBuildOptions.None);

            //Bootstrap Docker
            RunCommandLine("docker", "build -t jayroblack/dynamodb-local:1.1 -f ../../../../DynamoDbDocker/DockerFile ../../../../DynamoDbDocker");
            RunCommandLine("docker", "run -d --rm --name integration-test -p 8000:8000 jayroblack/dynamodb-local:1.1");
            
            if( !WaitForContainerToBeOnline())
                Assert.False(true, "Failed to start up and query DynamoDb Local.");
        }

        public void Dispose()
        {
            //TODO: When debugging - discover a way to ensure that docker is stupped
            //IF you Get Stuck - open Git Bash and type `docker stop integration-test` 
            RunCommandLine("docker", "stop integration-test");
        }

        public void RunCommandLine(string fileName, string args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

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
            builder.RegisterModule<MailMergeAutofacModule>();
            builder.RegisterModule<FakesAutofacModule>();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddDebug()
                    .AddConsole();
            });

            //Add custom logic later to dynamically resolve at runtime.
            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
            builder.Register(c => c.Resolve<ILoggerFactory>().CreateLogger<DynamoDb>()).As<ILogger<DynamoDb>>();

            //Overrides
            builder.RegisterType<DynamoDBLoccalFactory>().As<IDynamoDBFactory>();
            return builder;
        }
    }
}
