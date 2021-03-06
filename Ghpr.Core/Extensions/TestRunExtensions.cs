﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class TestRunExtensions
    {
        public static ITestRun Update(this ITestRun target, ITestRun run)
        {
            if (target.TestInfo.Guid.Equals(Guid.Empty))
            {
                target.TestInfo.Guid = GuidConverter.ToMd5HashGuid(target.FullName);
            }
            target.Screenshots.AddRange(run.Screenshots);
            target.Events.AddRange(run.Events);
            return target;
        }

        public static string GetFileName(this ITestRun testRun)
        {
            return testRun.TestInfo.Finish.GetTestName();
        }
        
        public static void Save(this ITestRun testRun, string path, string name = "")
        {
            Paths.Create(path);
            var fullPath = Path.Combine(path, name.Equals("") ? testRun.GetFileName() : name);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }

        public static ITestRun GetTest(this List<ITestRun> testRuns, ITestRun testRun)
        {
            var tr = testRuns.FirstOrDefault(t => t.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
                          ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
                          ?? new TestRun();
            return tr;
        }
    }
}