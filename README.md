# Flow .Net SDK

The Flow .Net SDK provides utilities to interact with the Flow blockchain (https://www.onflow.org).

Warning: This project is still under development and is likely to change.

## Install

```
Install-Package Flow.Net.Sdk
```

## Features

This SDK includes the following features:

Blocks:

- [x] retrieve a block by ID
- [x] retrieve a block by height
- [x] retrieve the latest block

Collections:

- [x] retrieve a collection by ID

Events:

- [x] retrieve events by name in the block height range

Scripts:

- [x] submit a script and parse the response
- [x] submit a script with arguments and parse the response

Accounts:

- [x] retrieve an account by address
- [x] create a new account
- [x] deploy a new contract to the account
- [x] remove a contract from the account
- [x] update an existing contract on the account

Transactions:

- [x] retrieve a transaction by ID
- [x] sign a transaction (single payer, proposer, authorizer or combination of multiple)
- [x] submit a signed transaction
- [x] sign a transaction with arguments and submit it

## Examples

Examples can be found [here](https://github.com/tyronbrand/flow.net/tree/main/examples)

## Documentation

Documentation can be found [here](https://github.com/tyronbrand/flow.net/tree/main/docs)

## Contributing

All contributions are welcome. Please read the [contributing guide](https://github.com/tyronbrand/flow.net/blob/main/CONTRIBUTING.md) to get started.