using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using HealthMonitoring.Monitors;

namespace HealthMonitoring.UnitTests.Helpers
{
    internal class TestableHealthMonitor : IHealthMonitor
    {
        private readonly ConcurrentQueue<Tuple<string, TimeSpan>> _calls = new ConcurrentQueue<Tuple<string, TimeSpan>>();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public string Name { get { return "test"; } }
        public IEnumerable<Tuple<string, TimeSpan>> Calls { get { return _calls; } }

        public void StartWatch()
        {
            _stopwatch.Start();
        }

        public Task<HealthInfo> CheckHealthAsync(string address, CancellationToken cancellationToken)
        {
            _calls.Enqueue(Tuple.Create(address, _stopwatch.Elapsed));
            return Task.FromResult(new HealthInfo(HealthStatus.Healthy, TimeSpan.FromMilliseconds(1), new Dictionary<string, string>()));
        }
    }
}