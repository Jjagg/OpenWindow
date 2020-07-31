
Interop with ObjC is a bit cumbersome.
Xamarin generates all the boilerplate, but I'm not sure we can use it without all the cruft.
We probably don't need a lot of NS classes, so we can make do with manual object management (with some helper methods).

ObjC interop basics:
http://jonathanpeppers.com/Blog/xamarin-ios-under-the-hood-calling-objective-c-from-csharp

Manual interop project:
https://github.com/Hitcents/iOS4Unity/blob/master/Assets/iOS4Unity/ObjC.cs

Xamarin generator:
https://github.com/xamarin/xamarin-macios/blob/5d2598af695c41c32712cd136d02c0e8cdfcac5e/src/generator.cs#L2276-L2294
