# CheapConnect.NET
An API wrapper for the CheapConnect SMS Api.

# License
This package is licensed under an MIT License. This library is spread in the
hope that it is useful, since the CheapConnect SMS api has several flaws.

# Initialization
```csharp
using CheapConnect.NET;

// You can find your api key at https://account.cheapconnect.net/sms.php
var cheapconnect = new CheapConnectApi("API KEY HERE");
```

# Usage
## Send an SMS message
```csharp
cheapconnect.SendSmsMessage("31612345678", "31612345678", "Hello, world");

// or

cheapconnect.SendSmsMessageAsync("31612345678", "31612345678", "Hello, world");
```

You can also use the `TrySendSmsMessage()` and `TrySendSmsMessageAsync()` 
methods to send an SMS message. This method will return either `true` if the
message was sent successfully or `false` if an error occured.