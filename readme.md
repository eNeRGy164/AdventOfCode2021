# Advent of Code 2021

These are my solutions to the Advent of Code 2021.

I used this challenge to dive into .NET 6 and C# features. During the challenges I (re)discovered many possibilities with C#, some that are long available but maybe not that known.

## General

For every solution I created a console application using the new .NET 6 Console template.  
This template uses some recent features:

* Top level statements *(C# 9.0)*
* Global using directive *(C# 10.0)*
* Implicit using directives *(C# 10.0)*
* Nullable reference types *(C# 9.0)*

I added the following global usings to be available as well:

* `System.Diagnostics`
* `System.Text`

### Style

I have applied specific style rules in my solutions. Some for readability, some for performance.  
In general:

* No LINQ
* `for` instead of `foreach`
* [`var`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/var?WT.mc_id=DT-MVP-5004268) instead of explicit types
* No default access modifiers declarations
* Single line statements
* Auto properties

### Measuring performance

To measure the performance of each solution I use the [`Stopwatch` class](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch?WT.mc_id=DT-MVP-5004268&view=net-6.0).

The stopwatch start after loading the input from disk using the [`File.ReadAllLines` method](https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readalllines?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_IO_File_ReadAllLines_System_String_) and stops after all code is executed, except for writing the results to the console.

The measurements on this page are based on my local machine (Intel® Core i7-6700 3.4GHz) and running the application in *release* mode from Visual Studio 2022 *(`CTRL+F5`)*.

## Solutions

For each solution I put in some remarks about .NET / C# constructs I use that day. Some are very familiar to me, some were new. This list is to inspire you with classes, methods, or other possibilities you might be unfamiliar with.

There is a note after a remark if it is about something introduced after .NET Framework 1.1.

There is only one remark about a specific construct on the first day it is used.

### Day 1: [Sonar Sweep](https://adventofcode.com/2021/day/1)

Duration: **2ms**  
Code: [Day01.cs](./Day01/Day01.cs)

#### Remarks

* Using the [`Int32.MaxValue` field](https://docs.microsoft.com/en-us/dotnet/api/system.int32.maxvalue?WT.mc_id=DT-MVP-5004268&view=net-6.0) as a placeholder.
* Initializing [arrays](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/?WT.mc_id=DT-MVP-5004268) with a length (and [default values](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/?WT.mc_id=DT-MVP-5004268#default-value-behaviour))
* Using [`for`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements?WT.mc_id=DT-MVP-5004268#the-for-statement) to iterate is faster then using [`foreach`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements?WT.mc_id=DT-MVP-5004268#the-foreach-statement) or [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/linq/?WT.mc_id=DT-MVP-5004268)'s [`Select`](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?WT.mc_id=DT-MVP-5004268&view=net-6.0) method.

### Day 2: [Dive!](https://adventofcode.com/2021/day/2)

Duration: **0ms** (~80 ticks)  
Code: [Day02.cs](./Day02/Day02.cs)

#### Remarks

* Use the [`String.Chars[]` property](https://docs.microsoft.com/en-us/dotnet/api/system.string.chars?WT.mc_id=DT-MVP-5004268&view=net-6.0) to get a specific char from a [`String` class](https://docs.microsoft.com/en-us/dotnet/api/system.string?WT.mc_id=DT-MVP-5004268&view=net-6.0) by index
* [`Index` struct](https://docs.microsoft.com/en-us/dotnet/api/system.index?WT.mc_id=DT-MVP-5004268&view=net-6.0) *index from end* `^` (hat) operator to get the last item of an array. *(C# 8.0)*
* Convert a `char` to `int` by subtracting `48`. The digits 0 to 9 have the positions 48 to 57 in the [Unicode table](https://en.wikipedia.org/wiki/List_of_Unicode_characters#Basic_Latin).  
  Therefore, if it is known the input is in the 0 to 9 range, we can just subtract 48. C# will convert the char's `ushort` value to an `int` value implicitly.

### Day 3: [Binary Diagnostic](https://adventofcode.com/2021/day/3)

Duration: **0ms** (~7000 ticks)  
Code: [Day03.cs](./Day03/Day03.cs)

#### Remarks

* Instead of building a `string` of `0`'s and `1`'s and use the `Convert.ToInt32` method to create the integer value, it is also possible to shift the `1` bits to their position with the [*left-shift* `<<` operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators?WT.mc_id=DT-MVP-5004268#left-shift-operator-) and assign the resulting value with the [*logical OR* `|` operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators?WT.mc_id=DT-MVP-5004268#logical-or-operator-).
* Convert an int value to a char by adding `48` and explicitly casting it to `char`.

### Day 4: [Giant Squid](https://adventofcode.com/2021/day/4)

Duration: **7ms**  
Code: [Day04.cs](./Day04/Day04.cs)

#### Remarks

* When iterating through all values of a [multi-dimensional array](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/multidimensional-arrays?WT.mc_id=DT-MVP-5004268), [using `foreach`](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/using-foreach-with-arrays?WT.mc_id=DT-MVP-5004268) is faster.
* [`String.Split(Char, StringSplitOptions) method`](https://docs.microsoft.com/en-us/dotnet/api/system.string.split?WT.mc_id=DT-MVP-5004268&view=net-6.0) with [`StringSplitOptions.RemoveEmptyEntries`](https://docs.microsoft.com/en-us/dotnet/api/system.stringsplitoptions?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_StringSplitOptions_RemoveEmptyEntries) to remove empty entries from the resulting array. *(.NET Framework 2.0)*
* [`record class`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record?WT.mc_id=DT-MVP-5004268) to get a class with a lot of default behavior like auto-implemented properties and constructors. *(C# 9.0)*
* [`goto` jump statement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/statements/jump-statements?WT.mc_id=DT-MVP-5004268#the-goto-statement) to get out of multiple levels of iterators.
* [`Range` struct](https://docs.microsoft.com/en-us/dotnet/api/system.range?WT.mc_id=DT-MVP-5004268&view=net-6.0) to [get a slice](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges?WT.mc_id=DT-MVP-5004268) of an array. *(C# 8.0)*
* Use the [`List<T>(Int32)` constructor](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.-ctor?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Collections_Generic_List_1__ctor_System_Int32_) with a capacity to prevent (multiple) resize operations. *(.NET Framework 2.0)*

### Day 5: [Hydrothermal Venture](https://adventofcode.com/2021/day/5)

Duration: **30ms**  
Code: [Day05.cs](./Day05/Day05.cs)

#### Remarks

* [`String.Split(String)` method](https://docs.microsoft.com/en-us/dotnet/api/system.string.split?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_String_Split_System_String_System_StringSplitOptions_) with a `string` as separator. *(.NET Core 2.0)*
* [`HashSet<T>` class](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?WT.mc_id=DT-MVP-5004268&view=net-6.0) to keep a unique set of elements. *(.NET Framework 3.5)*
* [Digit separators](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-7.0/digit-separators?WT.mc_id=DT-MVP-5004268) to make large numbers more readable in code. *(C# 7.0)*
* The [`using` directive](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive?WT.mc_id=DT-MVP-5004268) with the [`static` modifier](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive?WT.mc_id=DT-MVP-5004268#static-modifier) to access static members without specifying the type. *(C# 6.0)*
* [Record structs](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-10.0/record-structs?WT.mc_id=DT-MVP-5004268) like `record class`, but now as `ValueType` *(C# 10.0)*
* Positional records [implement the Deconstruct method](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/records?WT.mc_id=DT-MVP-5004268#deconstruct) by default *(C# 9.0)*

### Day 6: [Lanternfish](https://adventofcode.com/2021/day/6)

Duration: **1ms**  
Code: [Day06.cs](./Day06/Day06.cs)

#### Remarks

* Combine the [`List<T>(IEnumerable<T>) constructor`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.-ctor?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Collections_Generic_List_1__ctor_System_Collections_Generic_IEnumerable__0__) with a new array of `long` of a specific length (e.g. `new long[9]`) to initialize the list with default values. *(.NET Framework 2.0)*
* [`List<T>.RemoveAt(Int32)` method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.removeat?WT.mc_id=DT-MVP-5004268&view=net-6.0) with the value `0` to pop the first value from a list. Sadly, the value of the item is not returned with this statement
* Use the `L` suffix to make an [integer literal](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types?WT.mc_id=DT-MVP-5004268#integer-literals) of type `long`

### Day 7: [The Treachery of Whales](https://adventofcode.com/2021/day/7)

Duration: **17ms**  
Code: [Day07.cs](./Day07/Day07.cs)

#### Remarks

* Use a [`Func<T, TResult>` delegate](https://docs.microsoft.com/en-us/dotnet/api/system.func-2?WT.mc_id=DT-MVP-5004268&view=net-6.0) as parameter of a method to be able to use different algorithms that have `T` as a parameter and return a value of type `TReturn` *(.NET Framework 3.5)*

### Day 8: [Seven Segment Search](https://adventofcode.com/2021/day/8)

Duration: **8ms**  
Code: [Day08.cs](./Day08/Day08.cs)

#### Remarks

* [Tuple types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples?WT.mc_id=DT-MVP-5004268) with declared [field names](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples?WT.mc_id=DT-MVP-5004268#tuple-field-names) *(C# 7.0)*
* [`String.ToCharArray()` method](https://docs.microsoft.com/en-us/dotnet/api/system.string.tochararray?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_String_ToCharArray) to get a copy of all `char`s in a `string`
* [`Array.Sort(Array)` method](https://docs.microsoft.com/en-us/dotnet/api/system.array.sort?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Array_Sort_System_Array_) to sort an array in the fastest was possible
* [`String(Char[])` constructor](https://docs.microsoft.com/en-us/dotnet/api/system.string.-ctor?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_String__ctor_System_Char___) to create a new `string` from a `char` array

### Day 9: [Smoke Basin](https://adventofcode.com/2021/day/9)

Duration: **1ms**  
Code: [Day09.cs](./Day09/Day09.cs)

#### Remarks

* Use an [`Action<T1, T2>` delegate](https://docs.microsoft.com/en-us/dotnet/api/system.action-2?WT.mc_id=DT-MVP-5004268&view=net-6.0) as parameter of a method to be able to use different algorithms that have `T1` and `T2` as a parameter and not return value *(.NET Framework 3.5)*
* [`List<T>.ToArray()` method](https://docs.microsoft.com/en-us/dotnet/api/system.action-2?WT.mc_id=DT-MVP-5004268&view=net-6.0) to copy all list values into an array
* Combine the `Index` `^` (hat) operator with a `Range` operator to get the last *n* elements of an array (e.g. `array[^3..]` returns the last 3 elements) *(C# 8.0)*

### Day 10: [Syntax Scoring](https://adventofcode.com/2021/day/10)

Duration: **5ms**  
Code: [Day10.cs](./Day10/Day10.cs)

#### Remarks

* Collection initializers *(C# 3.0)*
* Collection initializers with indexed elements *(C# 6.0)*
* Use the [`Stack<T>` class](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?WT.mc_id=DT-MVP-5004268&view=net-6.0) to hold elements in a *last-in-first-out* order. *(.NET Framework 2.0)*
* [`Stack<T>.Push(T)` method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1.push?WT.mc_id=DT-MVP-5004268&view=net-6.0) to add an element to the top of the stack *(.NET Framework 2.0)*
* [`Stack<T>.Peek(T)` method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1.peek?WT.mc_id=DT-MVP-5004268&view=net-6.0) to look at the top element without removing it *(.NET Framework 2.0)*
* [`Stack<T>.Pop(T)` method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1.pop?WT.mc_id=DT-MVP-5004268&view=net-6.0) to return and remove the element on the top of the stack *(.NET Framework 2.0)*

### Day 11: [Dumbo Octopus](https://adventofcode.com/2021/day/11)

Duration: **5ms**  
Code: [Day11.cs](./Day11/Day11.cs)

#### Remarks

* Combine multiple conditions in the *conditional* section of a `for` loop

### Day 12: [Passage Pathing](https://adventofcode.com/2021/day/12)

Duration: **273ms**  
Code: [Day12.cs](./Day12/Day12.cs)

#### Remarks

* [`HashSet<T>.TryGetValue(T, T)` method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1.trygetvalue?WT.mc_id=DT-MVP-5004268&view=net-6.0) to look up a value that has more complete data than the current value, although their comparer functions indicate they are equal *(.NET Framework 4.7.2)*
* The [null-forgiving operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/nullable-reference-types-specification?WT.mc_id=DT-MVP-5004268#the-null-forgiving-operator) to limit warnings about possible `null` values when runtime the value is not expected to be `null` *(C# 9.0)*
* Override `IEquatable<T>` on a record to have a custom equality comparison. *(C# 9.0)*
* Auto-properties with an [`init` only setter](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/init?WT.mc_id=DT-MVP-5004268) *(C# 9.0)*

### Day 13: [Transparent Origami](https://adventofcode.com/2021/day/13)

Duration: **13ms**  
Code: [Day13.cs](./Day13/Day13.cs)

#### Remarks

* [Target-typed `new` expressions](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/target-typed-new?WT.mc_id=DT-MVP-5004268) *(C# 9.0)*
* `List<T>(IEnumerable<T>) constructor` to efficiently copy all values of the list *(.NET Framework 2.0)*
* Use Unicode characters directly as `char`, no need for fancy Unicode escape sequences (e.g. `'█'`)
* [Switch expression](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/patterns?WT.mc_id=DT-MVP-5004268#switch-expression) *(C# 8.0)*
* [`with` expression](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/records?WT.mc_id=DT-MVP-5004268#with-expression) to copy and modify (struct) records.

### Day 14: [Extended Polymerization](https://adventofcode.com/2021/day/14)

Duration: **13ms**  
Code: [Day14.cs](./Day14/Day14.cs)

#### Remarks

* [`String.Concat(Object, Object)` method](https://docs.microsoft.com/en-us/dotnet/api/system.string.concat?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_String_Concat_System_Object_System_Object_) to quickly create a string from two `char`s
* [`Dictionary<TKey, TValue>.TryAdd(TKey, TValue)` method](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.tryadd?WT.mc_id=DT-MVP-5004268&view=net-6.0) to add an item without throwing an exception when it already exists *(.NET Core 2.0)*

### Day 15: [Chiton](https://adventofcode.com/2021/day/15)

Duration: **390ms**  
Code: [Day15.cs](./Day15/Day15.cs)

#### Remarks

* [`SortedSet<T>` class](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedset-1?WT.mc_id=DT-MVP-5004268&view=net-6.0) to have the elements in the set always in a certain order *(.NET Framework 4.0)*
* [`SortedSet<T>(IComparer<T>)` constructor](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedset-1.-ctor?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Collections_Generic_SortedSet_1__ctor_System_Collections_Generic_IComparer__0__) to provide a specific comparer *(.NET Framework 4.0)*
* [`yield return`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield?WT.mc_id=DT-MVP-5004268) to return a conditional set of elements

### Day 16: [Packet Decoder](https://adventofcode.com/2021/day/16)

Duration: **4ms**  
Code: [Day16.cs](./Day16/Day16.cs)

#### Remarks

* [`Convert.ToInt32(String, Int32)` method](https://docs.microsoft.com/en-us/dotnet/api/system.convert.toint32?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Convert_ToInt32_System_String_System_Int32_) with `fromBase` value `16` to convert hexadecimal `string` values to `int`
* [`Convert.ToString(Int32, Int32)` method](https://docs.microsoft.com/en-us/dotnet/api/system.convert.tostring?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Convert_ToString_System_Int32_System_Int32_) with `toBase` value `2` to convert `int` values to a binary `string` representation

### Day 17: [Trick Shot](https://adventofcode.com/2021/day/17)

Duration: **28ms**  
Code: [Day17.cs](./Day17/Day17.cs)

#### Remarks

* [`String.Split(Char[])` method](https://docs.microsoft.com/en-us/dotnet/api/system.string.split?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_String_Split_System_Char___) to split a string on several `char`s at once

### Day 18: [Snailfish](https://adventofcode.com/2021/day/18)

Duration: **430ms**  
Code: [Day18.cs](./Day18/Day18.cs)

#### Remarks

* [Overloading](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading?WT.mc_id=DT-MVP-5004268) the [*addition* `+` operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/addition-operator?WT.mc_id=DT-MVP-5004268#operator-overloadability)
* Marking a class as [`sealed`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed?WT.mc_id=DT-MVP-5004268) improves performance

### Day 19: [Beacon Scanner](https://adventofcode.com/2021/day/19)

Duration: **60s**  
Code: [Day19.cs](./Day19/Day19.cs)

#### Remarks

* [`String.Trim(Char[])` method](https://docs.microsoft.com/en-us/dotnet/api/system.string.trim?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_String_Trim_System_Char___) to trim a string on several `char`s at once
* [`Flags` attribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?WT.mc_id=DT-MVP-5004268&view=net-6.0) to allow an `enum` to be used as a bit field.
* Overloading the [*substraction* `-` operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/subtraction-operator?WT.mc_id=DT-MVP-5004268#operator-overloadability)
* [*Logical AND* `&` operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators?WT.mc_id=DT-MVP-5004268#logical-and-operator-) to test if a flag is set
* [*Bitwise complement* `~` operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators?WT.mc_id=DT-MVP-5004268#bitwise-complement-operator-) to flip specific bits

### Day 20: [Trench Map](https://adventofcode.com/2021/day/20)

Duration: **72ms**  
Code: [Day20.cs](./Day20/Day20.cs)

#### Remarks

* [`Array.Fill<T>(T[], T)` method](https://docs.microsoft.com/en-us/dotnet/api/system.array.fill?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Array_Fill__1___0_____0_) to quickly set every element in the array to the same value *(.NET Core 2.0)*
* [`Array.Copy(Array, Int32, Array, Int32, Int32)` method](https://docs.microsoft.com/en-us/dotnet/api/system.array.copy?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Array_Copy_System_Array_System_Int32_System_Array_System_Int32_System_Int32_) to copy the values from one array into another, but with an offset

### Day 21: [Dirac Dice](https://adventofcode.com/2021/day/21)

Duration: **22ms**  
Code: [Day21.cs](./Day21/Day21.cs)

#### Remarks

* The `using` directive to [create an alias](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive?WT.mc_id=DT-MVP-5004268#using-alias) of a generic `Dictionary<TKey, TValue>`
* Use the `UL` suffix to make an integer literal of type `ulong`
* [`StructLayout` attribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.structlayoutattribute?WT.mc_id=DT-MVP-5004268&view=net-6.0) to control the physical layout of the data fields of a structure, [`LayoutKind.Explicit`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.layoutkind?WT.mc_id=DT-MVP-5004268&view=net-6.0#System_Runtime_InteropServices_LayoutKind_Explicit) makes this explicit and with the [`FieldOffset` attribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.fieldoffsetattribute?WT.mc_id=DT-MVP-5004268&view=net-6.0) the position of a property is set

### Day 22: [Reactor Reboot](https://adventofcode.com/2021/day/22)

Duration: **37ms**  
Code: [Day22.cs](./Day22/Day22.cs)

### Day 23: [Amphipod](https://adventofcode.com/2021/day/23)

Duration: **2m03s**  
Code: [Day23.cs](./Day23/Day23.cs)

#### Remarks

* `for` loop can be executed on a range of `char`

### Day 24: [Arithmetic Logic Unit](https://adventofcode.com/2021/day/24)

Duration: **2600ms**  
Code: [Day24.cs](./Day24/Day24.cs)

#### Remarks

* Implicit cast of `string` to the [`ReadOnlySpan<T>` class](https://docs.microsoft.com/en-us/dotnet/api/system.readonlyspan-1?WT.mc_id=DT-MVP-5004268&view=net-6.0) where T is `char` *(.NET Core 2.1)*

### Day 25: [Sea Cucumber](https://adventofcode.com/2021/day/25)

Duration: **214ms**  
Code: [Day25.cs](./Day25/Day25.cs)

#### Remarks

* Declare an [enumeration type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum?WT.mc_id=DT-MVP-5004268) with `ushort` to allow `char` as value in code
