// <copyright file="TestExecutionService.cs" company="Automate The Planet Ltd.">
// Copyright 2016 Automate The Planet Ltd.
// Licensed under the Apache License, Version 2.0 (the "License");
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Anton Angelov</author>
// <site>http://automatetheplanet.com/</site>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using log4net;
using MSTest.Console.Extended.Data;
using MSTest.Console.Extended.Interfaces;
using System.IO;

namespace MSTest.Console.Extended.Services
{
    public class TestExecutionService
    {
        private string ToscaPath;
        private readonly ILog log;

        private readonly IMsTestTestRunProvider microsoftTestTestRunProvider;

        private readonly IFileSystemProvider fileSystemProvider;

        private readonly IProcessExecutionProvider processExecutionProvider;

        private readonly IConsoleArgumentsProvider consoleArgumentsProvider;

        public TestExecutionService(
            IMsTestTestRunProvider microsoftTestTestRunProvider,
            IFileSystemProvider fileSystemProvider,
            IProcessExecutionProvider processExecutionProvider,
            IConsoleArgumentsProvider consoleArgumentsProvider,
            ILog log)
        {
            this.microsoftTestTestRunProvider = microsoftTestTestRunProvider;
            this.fileSystemProvider = fileSystemProvider;
            this.processExecutionProvider = processExecutionProvider;
            this.consoleArgumentsProvider = consoleArgumentsProvider;
            this.log = log;
        }
        
        public int ExecuteWithRetry()
        {
            File.Delete("C:\\AutomationTempFile\\ToscaPaths.txt");
            this.fileSystemProvider.DeleteTestResultFiles();
            this.processExecutionProvider.ExecuteProcessWithAdditionalArguments();
            this.processExecutionProvider.CurrentProcessWaitForExit();
            var testRun = this.fileSystemProvider.DeserializeTestRun();
            int areAllTestsGreen = 0;
            var failedTests = this.microsoftTestTestRunProvider.GetAllNotPassedTests(testRun.Results.ToList());
            foreach (var testRunUnitTestResult in failedTests)
            {
                System.Console.WriteLine("Run number 1 (Initial run)");
                System.Console.WriteLine(testRunUnitTestResult.testName + testRunUnitTestResult.Output.ErrorInfo.Message + testRunUnitTestResult.Output.ErrorInfo.StackTrace+"\n");
            }
            readToscaPathAndWriteToTempFile();
            System.Console.WriteLine("--------------- Full Output STDOUT -----------------");
            System.Console.WriteLine(testRun.ResultSummary.Output.StdOut);
            System.Console.WriteLine(ToscaPath);
            System.Console.WriteLine("--------------- Full Output End -----------------");
            int failedTestsPercentage = this.microsoftTestTestRunProvider.CalculatedFailedTestsPercentage(failedTests, testRun.Results.ToList());
            
            if (failedTestsPercentage <= this.consoleArgumentsProvider.FailedTestsThreshold)
            {
                for (int i = 0; i < this.consoleArgumentsProvider.RetriesCount; i++)
                {
                    this.log.InfoFormat("Start to execute again {0} failed tests.", failedTests.Count);
                    if (failedTests.Count > 0)
                    {
                        System.Console.WriteLine($"\n\n-------------------------New Run----------------------------");
                        string currentTestResultPath = this.consoleArgumentsProvider.TestResultPath.Replace(".trx","")+$"{i+2}"+".trx";
                        string retryRunArguments = this.microsoftTestTestRunProvider.GenerateAdditionalArgumentsForFailedTestsRun(failedTests, currentTestResultPath);
                   
                        this.log.InfoFormat("Run {0} time with arguments {1}", i + 2, retryRunArguments);
                        this.processExecutionProvider.ExecuteProcessWithAdditionalArguments(retryRunArguments);
                        System.Console.WriteLine($"New mstest run with parameters are created. Parameters are: {retryRunArguments}");
                        this.processExecutionProvider.CurrentProcessWaitForExit();
                        var currentTestRun = this.fileSystemProvider.DeserializeTestRun(currentTestResultPath);
                        failedTests = this.microsoftTestTestRunProvider.GetAllNotPassedTests(currentTestRun.Results.ToList());
                        var failedTestsNew = this.microsoftTestTestRunProvider.GetAllNotPassedTests(currentTestRun.Results.ToList());
                        if (failedTestsNew.Count==0)
                        {
                            System.Console.WriteLine($"No failed tests in run number {i + 2}. All tests successfully passed");
                        }
                        System.Console.WriteLine($"-------------------------Exceptions info----------------------------");
                        foreach (var testRunUnitTestResult in failedTestsNew)
                        {
                            System.Console.WriteLine($"Run number {i + 2} failed tests issues:");
                            System.Console.WriteLine(testRunUnitTestResult.testName + testRunUnitTestResult.Output.ErrorInfo.Message + testRunUnitTestResult.Output.ErrorInfo.StackTrace+"\n");
                        }
                        readToscaPathAndWriteToTempFile();
                        System.Console.WriteLine("--------------- Full Output STDOUT -----------------");
                        System.Console.WriteLine(currentTestRun.ResultSummary.Output.StdOut);
                        System.Console.WriteLine(ToscaPath);
                        System.Console.WriteLine("--------------- Full Output End -----------------");    
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            if (failedTests.Count > 0)
            {
                areAllTestsGreen = 0;
            }
            
            return areAllTestsGreen;
        }
        public void readToscaPathAndWriteToTempFile() {
            var allLines = File.ReadAllLines("C:\\AutomationTempFile\\BuildInfo.txt");
            ToscaPath = allLines[3];
            File.AppendAllText("C:\\AutomationTempFile\\ToscaPaths.txt", allLines[3] + Environment.NewLine);
        }
    }
}