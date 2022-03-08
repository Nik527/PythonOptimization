using Indicators;

const int testCount = 1_000_000_000;



//var random = new Random();

//var value = 0d;
//var startTimeRandom = DateTime.Now;
//for (int i = 0; i < testCount; i++)
//{
//    value = random.NextDouble() * 100d;
//}
//var endTimeRandom = DateTime.Now;

//var randomIndicator = new Average();
//var startTimeRandomIndicator = DateTime.Now;
//for (int i = 0; i < testCount; i++)
//{
//    randomIndicator.Add(random.NextDouble() * 100d);
//}
//var endTimeRandomIndicator = DateTime.Now;

var constantIndicator = new Average();
var startTimeConstantIndicator = DateTime.Now;
for (int i = 0; i < testCount; i++)
{
    constantIndicator.Add(99.9d);
}
var endTimeConstantIndicator = DateTime.Now;

//Random time: {endTimeRandom - startTimeRandom}, value: {value}
//Indicator time: {endTimeRandomIndicator - startTimeRandomIndicator}, value: {randomIndicator.Value}
Console.WriteLine(@$"Test iterations: {testCount}
Indicator time without random: {endTimeConstantIndicator - startTimeConstantIndicator}, value: {constantIndicator.Value}");



