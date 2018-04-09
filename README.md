# CQRS-Rx

My take on a simple proof of concept .NET Standard 2.0 CQRS library leveraging Reactive Extensions (Rx)

The following codebase is based on the previous work provided by:

* [Meanwhile... on the command side of my architecture](https://cuttingedge.it/blogs/steven/pivot/entry.php?id=91)
* [CQRS : A Cross Examination Of How It Works](https://www.codeproject.com/articles/991648/cqrs-a-cross-examination-of-how-it-works)
* [Rx works nicely with DDD and Event Sourcing](https://abdullin.com/post/rx-works-nicely-with-ddd-and-event-sourcing/)
* [Simple CQRS example](https://github.com/gregoryyoung/m-r)
* [CQRSlite](https://github.com/gautema/CQRSlite)

**STATUS**: This project is still a work in progress (WIP).

**DISCLAIMER**: This project has no intention of becoming a CQRS framework and it's only purpose is to be a tool of learning.

## Thoughts & Opinions

Since this is project is, as stated previously, a tool of learning here are my overall thoughts and opinions during development, without further ado here they are.

### On leveraging Rx for CQRS/ES

The more I try to force Rx semantics into CQRS the more I "regret it" overall.

For example, `IObservable<Change>` vs. `LoadFromHistory(...)`. I do prefer the latter approach since it expresses the intent clearly.

Regarding the previous point, using `OnNext(...)` to load the aggregate state looks broken by design since it enables unexpected behavior if a real stream on a different scheduler is used. Could possibly be avoided by locking the aggregate until 'OnCompleted()' is called, but seems a clunky hack.

In my point of view the only suitable candidate for Reactive Extensions so far seems to be the `IStorage`, which can publish the events once they are stored. On that point I don't see any value of publishing the `Change` on an aggregate, even though it's there as proof of concept.

### On the CQRSlite implementation of an [AggregateRoot](https://github.com/gautema/CQRSlite/blob/master/Framework/CQRSlite/Domain/AggregateRoot.cs) being thread safe

I don't see a use case where a domain object should be accessible/used by multiple threads. The operations should be as atomic as possible.

The concurrency issues should be handled when saving the aggregate on the storage (log as source of truth).

### On [Greg Young's example](https://github.com/gregoryyoung/m-r/blob/master/SimpleCQRS/InfrastructureCrap.DontBotherReadingItsNotImportant.cs) and [CQRSlite example](https://github.com/gautema/CQRSlite/blob/master/Framework/CQRSlite/Infrastructure/DynamicInvoker.cs) `Apply(...)` method.

Both approaches use some nifty tricks in order to `Apply` events on aggregate while keeping track of the changes without exposing the methods on the domain object.

I understand that both approaches forces the code to some extent to adhere to the Event Sourcing concepts.

In a real scenario I think it's probably too much though. People that use/want to use CQRS with ES should first really understand how it works (not that I do) before using it.

Therefore my approach in using `IEventHandler<TEvent>` and on each domain object explicitly implement them. While this approach creates more relaxed constraints, it helps to express the intent better in my opinion.~

### On consistency related issues

As of now if an event published by the storage to an aggregate is out of order a `StreamEventOutOfOrderException` is thrown, It could be a nice approach to try and mitigate that by caching the events out of order and the following ones until order can be restored.

If the order could not be restored until `OnCompleted()` is called then an exception would be thrown.

The storage itself seems a better candidate to have this responsibility though.
