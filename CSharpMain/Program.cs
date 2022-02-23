using Indicators;

const int testCount = 100_000_000;



var random = new Random();

var value = 0d;
var startTimeRandom = DateTime.Now;
for (int i = 0; i < testCount; i++)
{
    value = random.NextDouble() * 100d;
}
var endTimeRandom = DateTime.Now;

var indicator = new Average();
var startTimeIndicator = DateTime.Now;
for (int i = 0; i < testCount; i++)
{
    indicator.Add(random.NextDouble() * 100d);
}
var endTimeIndicator = DateTime.Now;

Console.WriteLine(@$"Test iterations: {testCount}
Random time: {endTimeRandom - startTimeRandom}, value: {value}
Indicator time: {endTimeIndicator - startTimeIndicator}, value: {indicator.Value}
Indicator time without random: {endTimeIndicator - startTimeIndicator - (endTimeRandom - startTimeRandom)}");



