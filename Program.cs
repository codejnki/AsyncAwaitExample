using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitExample
{
  class Program
  {
    static async Task Main(string[] args){
      Console.WriteLine("We are going to make breakfast.");
      
      var watch = Stopwatch.StartNew();
      await CookBreakfastSlow();
      watch.Stop();
      Console.WriteLine($"It took {watch.ElapsedMilliseconds} ms to cook.");
      
      Console.WriteLine("That took a long time.  Let's try it a little faster");

      watch = Stopwatch.StartNew();
      await CookBreakfastFast();
      watch.Stop();
      Console.WriteLine($"It took {watch.ElapsedMilliseconds} ms to cook.");
    }


    public static async Task CookBreakfastSlow() {
      await FryEggAsync();
      await ToastBreadAsync();
      Console.WriteLine("Breakfast is ready.");
    }

    public static async Task CookBreakfastFast() {
      Task eggTask = FryEggAsync();
      Task toastTask = ToastBreadAsync();

      await eggTask;
      await toastTask;

      Console.WriteLine("Breakfast is ready");
    } 

    public static async Task FryEggAsync() {
      Console.WriteLine("Starting egg frying");
      await Task.Run(() => {
        for (var i = 0; i < 5; i++){
          Console.WriteLine("Frying");
          Thread.Sleep(1000);
        }
      });
      Console.WriteLine("Egg is done");
    }

    public static async Task ToastBreadAsync(){
      Console.WriteLine("Starting on toast");
      await Task.Run(() => {
        for (var i = 0; i < 5; i++){
          Console.WriteLine("Toasting");
          Thread.Sleep(1000);
        }
      });
      Console.WriteLine("Toast done.");
    }
  }
}
