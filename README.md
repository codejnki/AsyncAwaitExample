# AsyncAwaitExample

I saw a tweet with a link to an [article](https://dev.to/sumaiyaasif/async-and-await-and-what-is-it-actually-doing-34l) by [Sumaiya Asif](https://twitter.com/asif_i_care) explaining `async` and `await` that made me realize I've been using this pattern all __wrong__ for years.

So I sat down and wrote this out in an attempt to seal this knowledge into my brain.

## The Wrong Way

So for years I've been trying to do things with a minimalist approach to the number of lines of code written.

```cs
public static async Task CookBreakfastSlow() {
  await FryEggAsync();
  await ToastBreadAsync();
  Console.WriteLine("Breakfast is ready.");
}
```

On the surface this looks great.

This is short and compact yet suffers from a complete misunderstanding of what awaiting tasks is supposed to do for us.

When the program hits an `await` statement it stops what it is doing, and __waits__.

This method does not take advantage of the compiler optimizations for leveraging background threads to do work.

You can see this by looking at the output this produces.

```console
Starting egg frying
Frying
Frying
Frying
Frying
Frying
Egg is done
Starting on toast
Toasting
Toasting
Toasting
Toasting
Toasting
Toast done.
Breakfast is ready.
```

## The Right Way

The right way to use this pattern is to create your tasks as early in your execution path as possible, and __then__ wait for them, preferably at the point immediately before you need their results.

```cs
public static async Task CookBreakfastFast() {
  Task eggTask = FryEggAsync();
  Task toastTask = ToastBreadAsync();

  await eggTask;
  await toastTask;

  Console.WriteLine("Breakfast is ready");
}
```

I know what you are thinking, at first glance this is a full on 28.5% explosion in code.  We've been told for years that fewer lines is better performing code.  But in the `async` and `await` world, these two extra lines of code are going to cut our total execution time by 50%.

Our tasks are now going to get put on background threads.  They each start doing their work, and __then__ we wait for them.

If we look at the console output we see a massive difference.

```console
Starting egg frying
Frying
Starting on toast
Toasting
Frying
Toasting
Frying
Toasting
Frying
Toasting
Frying
Toasting
Toast done.
Egg is done
Breakfast is ready
```

## Conclusion

The first method while compact, and in all fairness compiles and works, is basically wrong.  You might as well not be using the overhead of tasks because you aren't going to get any the benefits, especially if the tasks you are calling are they themselves calling additional tasks.  You are basically living synchronously in an asynchronous world.

Allowing your brain to step away from a minimalist view of lines of codes will actually allow the compiler to do it's job, and help your program find the quickest path to completion.

If you'll excuse me, I now have a lot of code to refactor.
